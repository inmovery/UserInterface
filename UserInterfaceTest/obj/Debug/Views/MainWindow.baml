   M S B A M L   `   `   `  ����L  HUserInterfaceTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:UserInterfaceTest.ViewsUserInterfaceTest.Views  5   	   K*clr-namespace:UserInterfaceTest.ConvertersUserInterfaceTest.Converters  5   	   K*clr-namespace:UserInterfaceTest.ViewModelsUserInterfaceTest.ViewModels  5   	   (    "UserInterfaceTest.Views.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:UserInterfaceTest.Views     5   	   A
converters*clr-namespace:UserInterfaceTest.Converters     5   	   9vm*clr-namespace:UserInterfaceTest.ViewModels     5   	     � WindowStyle$
  None=�5	   	    � AllowsTransparency .  � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ 
MainWindow��5   	   $
��1280��5   	   $	��720��5   	   $
��1280��5   	   $	��720��5   	    � 	Resources 5      4   .UserInterfaceTest.Converters.ProgressConverter   ProgressConverter(   "UserInterfaceTest.Views.MainWindow �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� Padding �� FallbackValue ��Name  BackgroundLightBrush �� CornerRadius J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Opacity J� OpacityMask !� Visual �� ElementName � RowDefinitions K� ZIndex � ColumnDefinitions  	Hoverless  
HeaderText �� Orientation  WindowControlButton  IconMinimize  IconMaximize  WindowCloseButton  IconExit �� Color �� Opacity��%x  &	        '       7  7  7  7  7  7  7  7  7    5   
   6'   �� 5   
   "  6   .�5      ��  � �� 5      �� 5      "	 �6&   .R�5      �� 
 6!   �� *OuterMarginSizeThickness+$ 5��._�5      � .+�5    "   �� $ 	Container��5!   %   8��   5#   %   �� *WindowCornerRadius+$ 0���� 5&   "    6)   �� *WindowCornerRadius+$ 0����5'   )   �� *ForegroundLightBrush+$ White�� 5(   &     5)   *   $ 0Q�6;   $	 0.2Q�6K   5+   #   � 5.   "    51   &   !� 52   *    66   �� $ 	Container�� 55   &   �� 57   *   ��68   �� *TitleHeightGridLength+$ 35���� 59   *   $
��Auto �68   �� 5;   *   $��* �68   .+�5?   &   � $��0��6+   $ 1��68    5A   *   �� 5B   .   $
��Auto �6?   �� 5D   .   $��* �6?   .+�5I   *   �� $��0��61   8�� ��6d   �� *MenuCommand+.��5J   .   �� 8�� $��Image Downloader��6\   $
��Auto��6t   5K   +   �� 5M   *   $��1��65   $ 
Horizontal=�6E   $��Right=�6^   .+�5O   .   �� $��
10 15 10 0q�6Y   8�� ��65   �� *MinimizeCommand+.��5P   2   �� 8�� 6A   5Q   /   �� 5R   .   $��	10 4 10 0q�6Y   8�� ��65   �� *MaximizeCommand+.��5S   2   �� 8�� 6A   5T   /   �� 5U   .   8�� ��65   �� *CloseCommand+.��5V   2   �� 8�� 6A   5W   /   5X   +   5Z   '   �� 5]   &   $��1��6-   $��6��6:   $��0 1 0 0q�6E   ��5_   *   �� 5`   .   $ Black��6>   $
 0.05Q�6L   5c   '   �� 5f   &   $��2��6-   
 6:   �� *InnerContentPadding+._�5g   *   �� #��z��6;   5h   '   5i   #   5j      5k      5l      5n      5o      5p      '  !System.Windows.Shell.WindowChrome   WindowChrome 5r        5s   
      CornerRadius$ 0��5v         GlassFrameThickness$ 0q�5w         ResizeBorderThickness 5t      �� *ResizeBorderThickness+   CaptionHeight 5u      �� *TitleHeight+5w      .��5z      ]� $��10q�5{   	   ��. .+�5}   
   �� $��Bottom=�6   .+�5~      �� $��Загрузить все��5�      $��5q�5�       	 RegularButton#��[	 5�      ��5      �� *DownloadAllCommand+5�      � 5�      -   5         �Name$ ProgressBarView��5�      $��30��5�      $��5q�5�       
 GeneralBrush#��[
 5�        ProgressBarTestStyle#���  5�        � Value  5�      �� *86TotalDownloadingProgress.TotalDownloadingProgressValue+! �� Mode$! OneWay=�$ 0��5�      �� 5�      $ 
Horizontal=�6   $��5 0 0 0q�62   .+�5�      �� $%��Статус загрузки: ��5�      $��20(�5�        MontserratMedium#��[ 5�      �� 5�      $��20(�5�      #��[ 5�      ��5�      �� *86TotalDownloadingProgress.TotalDownloadingProgressValue+$! OneWay=�" �� 	Converter#" [  5�      5�      5�      � 5�   
   .+�5�      �� $��Disabled=�5�      $��Disabled=�5�      $��Stretch=�5�      $��Stretch=�5�      $��0q�5�      
������ ��5�      �� *ImageDownloadersView+��5�      �� 5�      .R�5�      A� # A� Rows$# 1��6&   $ A� Columns$ 6/   �� *ImageDownloaders.Count+5�      ��5�      �� 5�      .R�5�      1   +UserInterfaceTest.Views.ImageDownloaderView  % � DataContext% 64   �� 5�      ��5�      �� 5�      " ��6   .�5�      ��  � ��$ 0q�6<   �� 5�       � ��
 ���� 68   �� 5�       � �� 5�      �� 5�   "   "	 ��62   .R�5�   &   � & �Name$
& Grid��6+   .+�5�   *   �� $ SimpleBackground��5�   -   $y�Visible=�5�   -   #��z��5�   -   #��z��5�   -   #��z��5�   -   $��Stretch=�5�   -   $��Stretch=�5�   -   �� 5�   *   $ OnSelectedBackground��5�   -   $y�	Collapsed=�5�   -   #��z��5�   -   #��z��5�   -   
������ $��Stretch=�5�   -   $��Stretch=�5�   -   �� 5�   *   ' ��Name$' ContentPresenter��6;   5�   '   ( �� Triggers( 5�   &   P� 5�   *   ) P� Property) �@��
IsSelected* P� Value* . . �5�   .   �� + �� 
TargetName$+ OnSelectedBackground��65    � y�$ Visible=�6m   6   5�   +   P� 5�   *   ) �@��
IsSelected* .  . �5�   .   �� $+ OnSelectedBackground��65    � y�$ 	Collapsed=�6m   6�   5�   +   5�   #   5�      5�      5�      5�      5�      5�      