   M S B A M L   `   `   `  ����L  HCalculatorExample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CalculatorExampleCalculatorExample  5   	   I ECalculatorView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY=clr-namespace:CalculatorView.Controls;assembly=CalculatorViewCalculatorView.Controls 5   	   "    CalculatorExample.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CalculatorExample     5   	   Rcontrols=clr-namespace:CalculatorView.Controls;assembly=CalculatorView     5   	     � DataContext$   ��5	   	    � Title$ 
MainWindow��5
   	   $	��200��6   $	��525��6)    � 	Resources 5      0  *CalculatorView.Controls.TextBoxFileBrowser �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value	 �� RelativeSource
 �� Path!   CalculatorExample.ViewModel   vm��%�   '        &	  �     �� 5   
   "  6    5      P� 5       �@=�HasError . . �5      ��  �@�ToolTip 60   �� 	 �� *Self+$2
 ,(Validation.Errors).CurrentItem.ErrorContent�5      5        5   
   6   5      .��5      � #  �   6    � RowDefinitions 5   
   �� 5      $��35 �6   �� 5      $��35 �6   �� 5      $��35 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $��* �6   .+�5"   
   �� $��SQLite Database:��6   $��0��6+   $��0��68   $��5q�6H     5#   
   $��0��6&   $��1��63   $��5q�6C      Path 6N   �� $
 FilePath� �� Mode$ TwoWay=� �� ValidatesOnDataErrors . �� 5%   
   $��Table Name:��6   $��1��6&   $��0��63   $��5q�6C   �� 5&   
   $��1��6   $��1��6"    �� Orientation$ 
Horizontal=�62   .+�5'      �� -   5        $��Update��5'      $��5q�6&   61   �� 5(      -   5        $��CMBTableNames��5(      $��1��6T   $��1��6a   $	��300��6q   $��5q�6}   ��6,   �� $
 
TableNames�6}   5)      �� 5+   
   $��Column Name:��6   $��2��6'   $��0��64   $��5q�6D   �� 5,   
   $��2��6   $��2��6"   $ 
Horizontal=�62   .+�5-      �� -   5        $��Update��5-      $��5q�6&   61   �� 5.      -   5        $��CMBColumnNames��5.      $	��300��6V   $��5q�6b   ��6-   �� $
 ColumnNames�6b   5/      �� 51   
   -   5        $��3��51      $��1��6   $��Right=�6.   $��Bottom=�6J   $��5q�6e   $��Launch Calculator��6p   6�   52      53      