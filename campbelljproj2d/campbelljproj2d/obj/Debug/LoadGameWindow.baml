   M S B A M L   `   `   `  ����J  Fcampbelljproj2d, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:campbelljproj2dcampbelljproj2d  5   	   $    campbelljproj2d.LoadGameWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:campbelljproj2d     5   	     � Title$  Load a Saved Game��5   	   $��346.123��5	   	   $	��600��5   	    � WindowStartupLocation$ CenterScreen=�6   .��5      � .+�5   
   �� -   5         ��Name$ tbLoad��5      $
��Left=�6$   $��	10,10,0,0q�6?   $
��Wrap=�6R   $'��!Select the Game you want to load:��6f   $	��Top=�6�   $	��274��6�   $��16(�6�   $��30��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 	dgPlayers��5         AutoGenerateColumns .     CanUserAddRows .  $
��Auto=�5      $
��Left=�5      $	��560��5      $��
10,45,0,10q�5      $��14(�6!   $��2��6/   ��5      ��  �� Source$ Player��   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Select��6)   	   CellTemplate	 5      �� 5      .R�5      �� $��	btnSelect��6%   -   5        $��Select��5   6   6G   5      5!      0  *System.Windows.Controls.DataGridTextColumn  5"      $ Player Name��6%   
   
IsReadOnly
 . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   ��  �� Path$
 Name�6Z     5#      $ Trivia Category��6%   
 .  6>   �� $ Category�6b     5$      $ 	Grid Size��6%   
 .  68   �� $ GridSize�6\   5(      5+      5,      