   M S B A M L   `   `   `  ����K  Gcampbelljmidterm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:campbelljmidtermcampbelljmidterm  5   	   '    !campbelljmidterm.ChoosePathWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:campbelljmidterm     5   	     � Title$#  Choose the path for Filewatch��5   	   $	��209��6/   $	��525��6<   .��5	      � -   5         �Name$ gridChoosePath��5	      
�������.+�5   
   �� -   5         ��Name$ lblInstructions��5      $S��MType in the field or browse for the Path for the Filewatch Program to monitor��6)   $��Center=�6�   $
��Left=�6�   $��	10,10,0,0q�6�   $	��Top=�6�   $��25��6�   $	��480��6�   �� 5   
   -   5        $ lblPath��5      $��Path:��6!   $
��Left=�61   $��Right=�6L   $��	10,50,0,0q�6o   $	��Top=�6�   $��25��6�   $��70��6�   �� 5   
   -   5         ��Name$ tbPath��5      $
��Left=�6"   $��25��6=   $��Center=�6I   $��	85,50,0,0q�6k    �� TextWrapping$
 Wrap=�6~   $�� ��6�   $	��Top=�6�   $	��325��6�   
�������6�   �� 5   
   -   5         ��Name$ 	btnBrowse��5      $��Browse��6$   $
��Left=�65   $��
415,50,0,0q�6P   $	��Top=�6d   $��25��6|   $��75��6�   
�������6�   �� 5   
   -   5        $ lblExtension��5      $��
Extension:��6&   $
��Left=�6;   $��Right=�6V   $��	10,90,0,0q�6y   $	��Top=�6�   $��Center=�6�   $��25��6�   $��70��6�   �� 5   
   -   5         ��Name$ 
stackPanel��5       � 	Resources 5      #  campbelljmidterm.FileExtTypes   fileExtTypes��%   &	          5      6%   5      .+�5      �� -   5         ��Name$ cbExtension��5      $
��Left=�5      $	��Top=�5      $��	85,90,0,0q�5      $	��253��5      $��25��5      #��[  5      	 �� 
IsEditable	 . 
 �� 
IsReadOnly
 . 
�������
���   � �� Text$ Select from Dropdown��5"      5$      �� 5%   
   -	   5        $ lblOrExtension��5%      $	��or ��6(   $
��Left=�66   $��
343,90,0,0q�6Q   $	��Top=�6e   $��25��6}   $��36��6�   �� 5&   
   -
   5        $ tbExtension��5&      $
��Left=�6'   $��Center=�6B   $��25��6d   $��
379,90,0,0q�6p   $
 Wrap=�6�   $	��Top=�6�   $	��111��6�   ��6�   ��  �� ElementName$ cbExtension�� �� Path$ SelectedItem� �� Mode$ OneWay=�6�   �� 5(   
   -   5        $ 	btnCancel��5(      $��Cancel��6$   $
��Left=�65   $��
24,130,0,0q�6P   $	��Top=�6d   $��25��6|   $��75��6�   
�������6�   �� 5)   
   -   5        $ 	btnSubmit��5)      $��Submit��6$   $
��Left=�65   $��415,130,0,0q�6P   $	��Top=�6e   $��25��6}   $��75��6�   
�������6�   5+      5,      