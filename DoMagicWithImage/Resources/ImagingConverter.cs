using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

public static class ImagingConverter
{
    private static bool InvokeRequired
    {
        get { return Dispatcher.CurrentDispatcher != Application.Current.Dispatcher; }
    }
    private static BitmapSource BitmapToBitmapSource(Stream stream)
    {
        BitmapDecoder bitmapDecoder = BitmapDecoder.Create(
            stream,
            BitmapCreateOptions.PreservePixelFormat,
            BitmapCacheOption.OnLoad);

        // This will disconnect the stream from the image completely...
        WriteableBitmap writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
        writable.Freeze();

        return writable;
    }

    public static Bitmap BitmapSourceToBitmap(BitmapSource bitmapImage)
    {
        using (var outStream = new MemoryStream())
        {
            BitmapEncoder enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            var bitmap = new Bitmap(outStream);

            return new Bitmap(bitmap);
        }
    }
    public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
    {
        if (bitmap == null)
            throw new ArgumentNullException("bitmap");

        if (Application.Current.Dispatcher == null)
            return null; // Is it possible?

        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // You need to specify the image format to fill the stream. 
                // I'm assuming it is PNG
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Make sure to create the bitmap in the UI thread
                if (InvokeRequired)
                    return (BitmapSource)Application.Current.Dispatcher.Invoke(
                        new Func<Stream, BitmapSource>(BitmapToBitmapSource),
                        DispatcherPriority.Normal,
                        memoryStream);

                return BitmapToBitmapSource(memoryStream);
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}