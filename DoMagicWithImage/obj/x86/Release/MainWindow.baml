   M S B A M L   `   `   `  ���� K  GDoMagicWithImage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DoMagicWithImageDoMagicWithImage  !    DoMagicWithImage.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:DoMagicWithImage     -     �Name$  
mainWindow�� � Title$ DoMagicWithImage��$	��750��$
��1288�� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$ mainGrid��.+��� -    ��Name$ image��$$��Resources/md_5a1d0330c30b2.png��$
��None=��� -    ��Name$ plusScaleButton��$��+��$
��Left=�$��	42,43,0,0q�$	��Top=�$��40��$��40��$��22(�|�.  ��
� .���  � Color$ 	#FFF3F3F3�� � Offset$ 1Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFCDCDCD���� -   $ righterButton��$��>��$
��Left=�$��	87,52,0,0q�$	��Top=�$��25��$��25��$��16(�|�.  ��
� .��� $ 	#FFF3F3F3��$ 1Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFCDCDCD���� -   $ lefterButton��$��<��$
��Left=�$��	12,52,0,0q�$	��Top=�$��25��$��25��$��16(�|�.  ��
� .��� $ 	#FFF3F3F3��$ 1Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFCDCDCD���� -   $ upperButton��$��>��$
��Left=�$��	49,12,0,0q�$	��Top=�$��25��$��25��$��16(�|�.   J� RenderTransformOrigin$ 0.5,0.5&���
� .��� $ 	#FFF3F3F3��$ 1Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFCDCDCD��z�V� .~��� �� �� 	 �� Angle$		 -90Q�T� �� -   $ downnerButton��$��>��$
��Left=�$��	49,88,0,0q�$	��Top=�$��25��$��25��$��16(�|�.  $ 0.5,0.5&���
� .��� $ 	#FFF3F3F3��$ 1Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFCDCDCD��z�V� .~��� �� �� $	 90Q�T� �� -	   $ minusScaleButton��$��-��$
��Left=�$��9,10,0,0q�$	��Top=�$��30��$��30��$��16(�|�.  ��
� .��� $ 	#FFF3F3F3��$ 1Q�� $ 	#FFEBEBEB��$	 0.5Q�� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFCDCDCD���� -
   $ paintManyColorsButton��$��?��$
��Left=�$��1220,660,0,0q�$	��Top=�$��50��$��50��$��36(��� -   
 ��Name$
 label��$��Offset:��$
��Left=�$��
1190,0,0,0q�$	��Top=�$��90��$��18(�
�������
���   ��� -   $
 offsetXLabel��$
��9999��$
��Left=�$��1190,32,0,0q�$	��Top=�$��18(�
�������$��90��
���   ��� -   $
 offsetYLabel��$
��9999��$
��Left=�$��1190,60,0,0q�$	��Top=�$��18(�
�������$��0q�
���   �$��90���� -   $
 
label_Copy��$��dt (ms):��$
��Left=�$��1190,97,0,0q�$	��Top=�$��90��$��18(�
�������
���   ��� -   $
 lastdtLabel��$
��9999��$
��Left=�$��1190,129,0,0q�$	��Top=�$��18(�
�������$��90��
���   ��� -    ��Name$ calcModeSlider��$
��Left=�$��	9,157,0,0q�$	��Top=� � Maximum$ 3Q� �� Orientation$ Vertical=�$	��123�� � SmallChange$ 1Q�$ 0.5,0.5&�z�V� .~��� �� �� $		 180Q�T� �� -   $
 label1��$��Sequential C#��$
��Left=�$��
42,157,0,0q�$	��Top=�$��14(�
�������
���   ��� -   $
 label1_Copy��$��Sequential C++��$
��Left=�$��
42,189,0,0q�$	��Top=�$��14(�
�������
���   ��� -   $
 label1_Copy1��$��Parallel CPU C++��$
��Left=�$��
42,221,0,0q�$	��Top=�$��14(�
�������
���   ��� -   $
 label1_Copy2��$��Parallel GPU C++��$
��Left=�$��
42,253,0,0q�$	��Top=�$��14(�
�������
���   �