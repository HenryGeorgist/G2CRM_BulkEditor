   M S B A M L   `   `   `  ����  	QuickHelp    QuickHelp.helpdialog   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � Title$
  Help��5      $	��500��6   $	��400��6    � Icon$C =pack://application:,,,/QuickHelp;component/Resources/Help.ico��6+   .��5%      � -   5         �Name$ mygrid��5%       � RowDefinitions 5&   
   �� 5'      -   5         �Name$ DocumentDescriptionRow��5'      $
��Auto �6:   �� 5(      -   5        $ TopicListRow��5(      $
��Auto �60   �� 5)      $��1* �6   .+�5-   
   �� -   5         ��Name$ 	TopicList��5-      $��1��6&   $
��Left=�63   $��5q�6N   $��Center=�6Y   $	��205��6t    �� 
IsEditable . 6�   �� 5/   
   -   5         ��Name$ FilterStackPanel��5/      $��1��6/   $��Right=�6<   $��Center=�6X   $��5q�6s    �� Orientation$ 
Horizontal=�6~   .+�50      �� -   5        $��
FilterText��50   c   $��2��6   $��23��6*   $��0,0,5,0q�66   $��Right=�6G   $��Center=�6u   $��97��6�   �� 51      -   5        $��FilterButton��51   A   $��1��6   $��Right=�6%   $��Center=�6U   $��23��6p   $��23��6}   .��52      �� $I��Cpack://application:,,,/QuickHelp;component/Resources/SearchIcon.bmp��6   53      54      � 56   
   -   5        $ DisplayGrid��56      $��2��6$   5:      5>      5?      