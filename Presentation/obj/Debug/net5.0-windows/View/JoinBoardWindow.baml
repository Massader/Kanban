   M S B A M L   `   `   `  ����G  CPresentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Presentation.ViewPresentation.View  5   	   '    !Presentation.View.JoinBoardWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Presentation.View     5   	     � Title$  JoinBoardWindow��5   	   $	��320��6!   $	��370��6.   .��5	      � ���	#FFF7F7F7 � RowDefinitions 5
   
   �� 5      �� 5      $	��200 �6   �� 5      .+�5      �� $��
Join Board��6   $��Center=�6)   $��Center=�6F   $
��Bold"�6a   $��22(�6s   � 5   
   $��1��6   .+�5      �� $��14(�66   $	��350��6D   ��6   �� *Boards+ �� View 5      �� 5       �� AllowsColumnReorder$
 True��6   .P�5      �� $��
Board Name��6T    �� Width$	 180��6h    �� DisplayMemberBinding 6)   ��  �� Path$
 Name�6h   �� 5      $��Creator��6Z   $	 150��6k    6)   �� $ 
AdminEmail�6k   5      ��5      �� 5       �� 
TargetType" ��6   .�5      -   5        5      5      5      5      5       