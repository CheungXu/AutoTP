   M S B A M L   `   `   `  ����J  FAutoEvalueSharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AutoEvalueSharpWPFAPPAutoEvalueSharpWPFAPP  5   	   &     AutoEvalueSharpWPFAPP.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:AutoEvalueSharpWPFAPP     5   	     � Title$  AutoEvalueSharp��5   	   $	��200��6!   $	��400��6.    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6   $��检测结果文件夹��5      $��Center=�6"   $��Center=�6=   �� 5   
   -   5        $��DetectTextBox��5      $��0��5      $��1��6   $��Center=�5   '   �� 5   
   -   5        $��DetectButton��5      $��0��5      $��2��6   $��选择��5   %   $��	10,5,10,5q�62   $��5q�6F   6Q   �� 5   
   $��1��6   $��0��6   $��真值文件夹��5      $��Center=�6    $��Center=�6;   �� 5   
   -   5        $��TrueTextBox��5       $��1��5      $��1��6   $��Center=�5    %   �� 5!   
   -   5        $��
TrueButton��5"      $��1��5!      $��2��6   $��选择��5"   #   $��	10,5,10,5q�60   $��5q�6D   6O   �� 5#   
   $��2��6   $��0��6   $��数据输出文件夹��5$      $��Center=�6"   $��Center=�6=   �� 5%   
   -   5        $��OutputTextBox��5&      $��2��5%      $��1��6   $��Center=�5&   '   �� 5'   
   -   5        $��OutputButton��5(      $��2��5'      $��2��6   $��选择��5(   %   $��	10,5,10,5q�62   $��5q�6F   6Q   �� 5)   
   -   5        $��StartButton��5*      $��3��5)      $��2��6   $��开始��5*   $   $��	10,5,10,5q�61   $��5q�6E   6P   5+      5,      