   M S B A M L   `   `   `  ����K  Gcampbelljmidterm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:campbelljmidtermcampbelljmidterm  5   	   *    $campbelljmidterm.QueryDatabaseWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:campbelljmidterm     5   	     � Title$  Database Results��5   	   $��843.5��6"   $��1225.5��61   .��5	      � -   5         �Name$ gridQueryDatabase��5	      
�������.+�5   
   �� -   5         ��Name$ lblExtension��5      $��Search by Extension:��6&   $
��Left=�6E   $
��Left=�6`   $��	10,20,0,0q�6�   $	��Top=�6�   $��Center=�6�   $��25��6�   $	��120��6�   �� 5   
   -   5         ��Name$ 
stackPanel��5       � 	Resources 5      #  campbelljmidterm.FileExtTypes   fileExtTypes��%   &	          5      6%   5      .+�5      �� -   5         ��Name$ cbExtension��5      $
��Left=�5      $	��Top=�5      $��
135,20,0,0q�5      $	��253��5      $��25��5      #��[  5       �� 
IsEditable .  �� 
IsReadOnly . 
�������
���   � �� Text$ Select from Dropdown��5      5      �� 5   
   -   5        $ lblOrExtension��5      $	��or ��6(   $
��Left=�66   $��
400,20,0,0q�6Q   $	��Top=�6e   $��25��6}   $��36��6�   �� 5    
   -   5        	 ��Name$	 tbExtension��5       $
��Left=�6'   $��Center=�6B   $��25��6d   $��
436,20,0,0q�6p   
 �� TextWrapping$

 Wrap=�6�   $	��Top=�6�   $	��111��6�   ��6�   ��  �� ElementName$ cbExtension�� �� Path$ SelectedItem� �� Mode$ OneWay=�6�   �� 5!   
   -   5         ��Name$ btnApply��5!      $��Apply��6#   $
��Left=�63   $��
570,20,0,0q�6N   $	��Top=�6b   $��75��6z   $��25��6�   
�������
���   � J� RenderTransformOrigin$ -1.059,0.52&�6�   6�   �� 5$   
   -   5         ��Name$ lvDatabaseResults��5%      $
��Left=�5&      $	��649��5'      $��	10,65,0,0q�5(      $	��Top=�5)      
�������
���   �$
��1172��5,       �� View 5-      �� 5.      .P�5/      �� $��ID��50       �� Width$ 45��51       �� DisplayMemberBinding 52      �� *Id+�� 53      $��	Timestamp��54      $	 125��55       56      �� *	Timestamp+�� 57      $
��Name��58      $	 250��59       5:      �� *Name+�� 5;      $��Action��5<      $ 75��5=       5>      �� *Action+�� 5?      $��Absolute Path��5@      $	 675��5A       5B      �� *AbsolutePath+5C      5E      �� 5F   
   -	   5        $ 
btnEmptyDB��5G      $��Empty Database��5H      $
��Left=�5I      $	��Top=�5J      $��123,728,0,34q�5K      $	��100��5L      $��40��5M      
�������
���   �$ -2.967,0.117&�5Q      �� 5R   
   -
   5        $ 	btnCancel��5S      $��Close��5T      $
��Left=�5U      $��1088,728,0,0q�5V      $	��Top=�5W      $��75��5X      $��40��5Y      $ 0.796,-0.008&�5Z      
�������
���   �5]      5^      5_      