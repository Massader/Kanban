   M S B A M L   `   `   `  ����G  CPresentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Presentation.ViewPresentation.View  5   	   %    Presentation.View.ConfirmWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Presentation.View     5   	     � Title$  ConfirmWindow��5   	   $	��160��6   $	��200��6,   .��5	      � ���	#FFF7F7F7 � RowDefinitions 5
   
   �� 5      �� 5      �� 5      .+�5   
   �� $��Are you sure?��6   $��Center=�6(   $��Center=�6E   $��18(�6`   $��34��6n   $	��118��6z   �� 5   
   -   5        $��Message��5      $��Center=�6    
���  ��$��Center=�6U   $��1��6r   ��5      �� 5       �� TextWrapping$
 Wrap=�6   ��61   ��  �� Path$ Message� �� Mode$ OneWay=�5      �� 5   
   -   5        $	��Yes��5      $��2��6   $��50��6,   $��16(�67   $
��Left=�6E   $��	110,9,0,9q�6`   6s   �� 5   
   -   5        $��No��5      $��2��6   $��50��6+   $��16(�66   $
��Left=�6D   $��40,9,0,9q�6_   6q   5      5      