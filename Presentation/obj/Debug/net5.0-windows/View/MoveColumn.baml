   M S B A M L   `   `   `  ����G  CPresentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Presentation.ViewPresentation.View  5   	   "    Presentation.View.MoveColumn   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Presentation.View     5   	     � Title$  
MoveColumn��5   	   $	��200��6   $	��200��6)   .��5	      � ���	#FFF7F7F7 � RowDefinitions 5
   
   �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��Move Column��6   $��Center=�6&   $��Center=�6C   $
��Bold"�6^   $��22(�6p   �� 5   
   $��New Column Position:��6   $��1��6/   $
��Left=�6<   $��Center=�6W   $��5,0,0,0q�6r   �� 5   
   $��40��62   $��40��6=   $��1��6I   $��Right=�6V   $��0,0,25,0q�6r   ��6   ��  �� Path$ 	ShiftSize�6r   �� 5   
   -   5        $��Message��5      $��Center=�6   
���  ��$��Center=�6T   $��46��6q   $	��200��6}   $��2��6�   ��5      �� 5       �� TextWrapping$
 Wrap=�6   ��61   �� $ Message� �� Mode$ OneWay=�5      �� 5   
   -   5        $
��Save��5      $��3��6    $��50��6-   $��35��68   6D   5      5      