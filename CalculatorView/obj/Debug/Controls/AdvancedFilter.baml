   M S B A M L   `   `   `  ����I  ECalculatorView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:CalculatorView.ControlsCalculatorView.Controls  5   	   ,    &CalculatorView.Controls.AdvancedFilter   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:CalculatorView.Controls     5   	     � Title$  Advanced Filter��5	   	   $	��450��6!   $	��525��6.    � DataContext 5   	   ��  �� RelativeSource �� *Self+ � 	Resources 5
         	BoolToVis��%   &	        �� 5   
   6'   5      .��5      � -   5        $��MainGrid��5       � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $	��200 �6   �� 5      $��25 �6   �� 5      -   5         �Name$ ExpressionTextBoxRow��5      $��* �68   �� 5      $��30 �6   .+�5   
   � $��2��6    � ColumnDefinitions 5      �� 5      $��1* �6#   �� 5      $��1* �6#   .+�5      �� $��5,0,0,0q�6    �� Orientation$ 
Horizontal=�6*   $��0��6C   .+�5      �� $��Select from��6   �� 5      ��6   �� *	TableName+ � ToolTip 66   �� *
ColumnType+�� 5      $��where:��6   5      �� 5      $��1��6   $��Center=�6'   ��6B   �� *SelectedCount+	 d� 	IsChecked	 6d   �� *UseSelection+y�6�   �� *AllowSelectionOption+
 �� 	Converter#
 [  5       � 5!   
   $��1��6    5"      �� 5#      $��1* �6#   �� 5$      $��1* �6#   .+�5&      8   2CalculatorView.Controls.AvailableFunctionsTreeView  -   5          Name$ AvailableFunctions��5&   J   $��1��6/   $��5q�6?   6J   R� 5'      -   5        $��AvailableFields��5'   2   $��0��6   $��5q�6'   6I   5(      /   )CalculatorView.Controls.ExpressionTextBox  5)   
   -   5          Name$ 
TestWindow��5)   :   $��3��6"   $��5q�6/   6P   �� 5+   
   -   5        $��Result��5+      $��4��6   $
��Left=�6,   $��Center=�6G   �� 5-   
   $��4��6   $ 
Horizontal=�6"   $��Right=�6;   $��2q�6W   .+�5.      �� -   5        $��IsCaseSensitive��5.      $��
Match Case��6.   $��Right=�6C   $��3q�6_   $��4��6j   $��Center=�6w   $
	 TrueY�6�   6�   �� 5/      -   5        $��CmdErrorLog��5/      $��	Error Log��6(   $��Right=�6<   $��3q�6X   $��2��6c   $��Center=�6p   $��75��6�   6�   �� 50      -	   5        $��
CmdExecute��50      $��Execute��6'   $��Right=�69   $��3q�6U   $��2��6`   $��Center=�6m   $��75��6�   6�   51      52      53      