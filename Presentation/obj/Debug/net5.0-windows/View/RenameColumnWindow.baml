   M S B A M L   `   `   `  ����G  CPresentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Presentation.ViewPresentation.View  5   	   *    $Presentation.View.RenameColumnWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Presentation.View     5   	     � Title$  RenameColumnWindow��5   	   $	��200��6$   $	��200��61   .��5	      � ���	#FFF7F7F7 � RowDefinitions 5
   
   �� 5      �� 5      $��30 �6   �� 5      �� 5      �� 5      .+�5   
   �� $��Rename Column��6   $��Center=�6(   $��Center=�6E   $
��Bold"�6`   $��22(�6r   �� 5   
   $��New Column Name:��6   $��1��6+   $
��Left=�68   $��Center=�6S   $��5,0,0,0q�6n   �� 5   
   $	��170��6K   $��30��6W   $��2��6c   $��Center=�6p   $��Center=�6�   ��6   ��  �� Path$ NewColumnName� �� Mode$ OneWayToSource=�6�   �� 5   
   -   5        $��Message��5      $��Center=�6   
���  ��$��Center=�6T   $��40��6q   $	��200��6}   $��4��6�   ��5      �� 5       �� TextWrapping$
 Wrap=�6   ��61   �� $ Message�$ OneWay=�5      �� 5   
   -   5        $
��Save��5      $��3��6    $��50��6-   $��30��68   $��Center=�6Y   $��Center=�6v   5      5      