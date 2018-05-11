using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DoMagicWithImage
{
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        WriteableBitmap wb = new WriteableBitmap(1280, 720, 96d, 96d, PixelFormats.Bgra32, null);
        byte[,] colors = new byte[1280 * 720, 4];
        int calcIterNum = 250;

        double scale = 300d;
        Vector offset = new Vector(-180d, 0d);

        double scaleStride = 60d;
        double scaleStrideCoef = 2;
        double offsetStride = 100d;

        float invCalcIterNum;

        int startMesTime;
        int endMesTime;

        bool isMouseDown = false;
        System.Windows.Point lastPos = new System.Windows.Point();

        enum Bgra32 { B, G, R, A }
        
        public MainWindow()
        {
            InitializeComponent();

            invCalcIterNum = 1f / (calcIterNum - 1);

            image.Source = wb;
            UpdateFractal();
            UpdateOffsetLabels();
        }

        void WritePixelsInWB(WriteableBitmap wrb)
        {
            wrb.WritePixels(new Int32Rect(0, 0, wrb.PixelWidth, wrb.PixelHeight), colors, wrb.PixelWidth * 4, 0);
        }
        
        void UpdateFractal()
        {
            int calcMode = (int)Math.Round(calcModeSlider.Value);
            switch(calcMode)
            {
                case 0:
                    SequentialCPUCSharpUpdateFractal();
                    break;
                case 1:
                    SequentialCPUCppUpdateFractal();
                    break;
                case 2:
                    ParallelCPUCppUpdateFractal();
                    break;
                case 3:
                    ParallelGPUUpdateFractal();
                    break;
                default:
                    break;
            }
        }

        void PaintTheBelongingPoint(int x, int y)
        {
            for (int j = 0; j < 3; j++)
                colors[y * wb.PixelWidth + x, j] = 0;
        }

        void PaintTheNotBelongingPoint(int x, int y, int iter)
        {
            colors[y * wb.PixelWidth + x, (int)Bgra32.B] = 
                (byte)(255 - (calcIterNum - iter) * invCalcIterNum * 255f);
            colors[y * wb.PixelWidth + x, (int)Bgra32.R] = 
                (byte)(255 - (calcIterNum - 0.5d * iter) * invCalcIterNum * 255f);
        }

        void SequentialCPUCSharpUpdateFractal()
        {
            startMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;

            for (int i = 0; i < wb.PixelHeight * wb.PixelWidth; i++)
            {
                colors[i, (int)Bgra32.A] = 255;
            }

            double mathX = 0d;
            double mathY = 0d;

            for (int y = 0; y < wb.PixelHeight; y++)
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    mathX = (x - wb.PixelWidth / 2 + offset.X) / scale;
                    mathY = (wb.PixelHeight / 2 - y - offset.Y) / scale;

                    //
                    // Begin with the 1-st iteration. (Not with 0-th)
                    Complex c = new Complex(mathX, mathY);
                    Complex z = new Complex(0d, 0d);
                    Complex zPrev = new Complex(c.Real, c.Imaginary);

                    int iter = 1;
                    while (iter < calcIterNum &&
                        z.Real * z.Real + z.Imaginary * z.Imaginary < 4d)
                    {
                        z = zPrev * zPrev + c;
                        zPrev = z;

                        iter++;
                    }
                    if (iter >= calcIterNum)
                        PaintTheBelongingPoint(x, y);
                    else
                        PaintTheNotBelongingPoint(x, y, iter);
                }

            WritePixelsInWB(wb);

            endMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            lastdtLabel.Content = (endMesTime - startMesTime).ToString();
        }
        
        [DllImport("CPUFractalCalc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void SequentialCPUFractalCalc(byte* colors, int width, int height, float scale, float offsetX, float offsetY, int calcIterNum);
        unsafe void SequentialCPUCppUpdateFractal()
        {
            startMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            fixed (byte* cols = &(colors[0, 0]))
                SequentialCPUFractalCalc(cols, wb.PixelWidth, wb.PixelHeight, (float)scale, (float)offset.X, (float)offset.Y, calcIterNum);

            WritePixelsInWB(wb);

            endMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            lastdtLabel.Content = (endMesTime - startMesTime).ToString();
        }

        [DllImport("CPUFractalCalc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void ParallelCPUFractalCalc(byte* colors, int width, int height, float scale, float offsetX, float offsetY, int calcIterNum);
        unsafe void ParallelCPUCppUpdateFractal()
        {
            startMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            fixed (byte* cols = &(colors[0, 0]))
                ParallelCPUFractalCalc(cols, wb.PixelWidth, wb.PixelHeight, (float)scale, (float)offset.X, (float)offset.Y, calcIterNum);

            WritePixelsInWB(wb);

            endMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            lastdtLabel.Content = (endMesTime - startMesTime).ToString();
        }

        [DllImport("GPUFractalCalc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void ParallelGPUFractalCalc(byte* colors, int width, int height, float scale, float offsetX, float offsetY, int calcIterNum);
        unsafe void ParallelGPUUpdateFractal()
        {
            startMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            fixed (byte* cols = &(colors[0, 0]))
                ParallelGPUFractalCalc(cols, wb.PixelWidth, wb.PixelHeight, (float)scale, (float)offset.X, (float)offset.Y, calcIterNum);

            WritePixelsInWB(wb);

            endMesTime = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            lastdtLabel.Content = (endMesTime - startMesTime).ToString();
        }

        void UpdateOffsetLabels()
        {
            offsetXLabel.Content = offset.X.ToString();
            offsetYLabel.Content = offset.Y.ToString();
        }
        
        private void plusScaleButton_Click(object sender, RoutedEventArgs e)
        {
            double scaleCoef = (scale + scaleStride) / scale;
            scale += scaleStride;
            scaleStride = scale / scaleStrideCoef;
            
            offset.X *= scaleCoef;
            offset.Y *= scaleCoef;

            UpdateFractal();
            UpdateOffsetLabels();
        }
        private void minusScaleButton_Click(object sender, RoutedEventArgs e)
        {
            double scaleCoef = (scale - scaleStride) / scale;
            scale -= scaleStride;
            scaleStride = scale / scaleStrideCoef;

            offset.X *= scaleCoef;
            offset.Y *= scaleCoef;

            UpdateFractal();
            UpdateOffsetLabels();
        }
        private void righterButton_Click(object sender, RoutedEventArgs e)
        {
            offset += new Vector(offsetStride, 0d);

            UpdateFractal();
            UpdateOffsetLabels();
        }
        private void lefterButton_Click(object sender, RoutedEventArgs e)
        {
            offset += new Vector(-offsetStride, 0d);

            UpdateFractal();
            UpdateOffsetLabels();
        }
        private void downnerButton_Click(object sender, RoutedEventArgs e)
        {
            offset += new Vector(0d, offsetStride);

            UpdateFractal();
            UpdateOffsetLabels();
        }
        private void upperButton_Click(object sender, RoutedEventArgs e)
        {
            offset += new Vector(0d, -offsetStride);

            UpdateFractal();
            UpdateOffsetLabels();
        }

        private void calcModeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            calcModeSlider.Value = Math.Round(calcModeSlider.Value);
        }

        private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            lastPos = Mouse.GetPosition(mainWindow);
        }
        private void mainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                offset -= Mouse.GetPosition(mainWindow) - lastPos;
                lastPos = Mouse.GetPosition(mainWindow);

                UpdateFractal();
                UpdateOffsetLabels();
            }
        }
        private void mainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            lastPos = Mouse.GetPosition(mainWindow);
        }
        private void mainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleCoef;
            if (e.Delta > 0)
            {
                scaleCoef = (scale + scaleStride) / scale;
                scale += scaleStride;
            }
            else
            {
                scaleCoef = (scale - scaleStride) / scale;
                scale -= scaleStride;
            }
            scaleStride = scale / scaleStrideCoef;

            offset.X *= scaleCoef;
            offset.Y *= scaleCoef;

            UpdateFractal();
            UpdateOffsetLabels();
        }
        private void MainWindowSizeChanged()
        {
            colors = new byte[((int)mainWindow.Width - 7) * ((int)mainWindow.Height - 29), 4];
            wb = new WriteableBitmap(
                (int)mainWindow.Width - 7,
                (int)mainWindow.Height - 29,
                96d,
                96d,
                PixelFormats.Bgra32,
                null);

            image.Source = wb;
            UpdateFractal();
        }
        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindowSizeChanged();
        }

        //
        // It doesn't work.
        private void mainWindow_StateChanged(object sender, EventArgs e)
        {
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Normal;

                //colors = new byte[
                //    //((int)(SystemParameters.PrimaryScreenWidth * SystemParameters.CaretWidth) - 7) * 
                //    //((int)(SystemParameters.PrimaryScreenHeight * SystemParameters.CaretWidth) - 29), 
                //    //((int)SystemParameters.WorkArea.Width - 7) *
                //    //((int)SystemParameters.WorkArea.Height - 29), 
                //    (1920 - 7) *
                //    (1080 - 29),
                //    4];
                //wb = new WriteableBitmap(
                //    1920 - 7,
                //    1080 - 29,
                //    96d,
                //    96d,
                //    PixelFormats.Bgra32,
                //    null);

                //image.Source = wb;
                //UpdateFractal();
            }
            //else if (mainWindow.WindowState == WindowState.Normal)
            //{
            //    MainWindowSizeChanged();
            //}
        }
    }
}
