   M S B A M L   `   `   `  ����J  Fcampbelljproj2d, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:campbelljproj2dcampbelljproj2d  5   	        campbelljproj2d.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:campbelljproj2d     5   	     � Title$  Trivia Maze��5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�5
   	   $	��910��5   	   $	��785��5   	   .��5      ��  �� Orientation$ Vertical=�6   
�������.+�5   
   o� -   5         o�Name$ mainMenu��5      $
��Left=�6!   $��20��6<   $	��Top=�6H   $	��792��6`   
�������
���   �$��1��6�   .@�5      m� -   5         m�Name$ menuItemFile��5      $��_File��6-   
���   �.@�5      m� -   5        $ menuItemNew��5      $��	_New Game��60   {�. 
���   � � ToolTip 5      _� 5      .��6"   Start a New Game64   5      m� 5      -   5        $ menuItemSave��5      $��
_Save Game��61   {�.  
���   � 5      _� 5      .��6"   Save the Game61   5      m� 5!      -   5        $ menuItemLoad��5!      $��
_Load Game��61   {�. 
���   � 5"      _� 5#      .��6"   Load the Game61   5%      m� 5'      -   5        $ menuItemQuit��5'      $��
_Quit Game��61   {�.  
���   � 5(      _� 5)      .��6"   Quit the Current Game69   5+      �� 5,      m� 5.      -   5        $ menuItemExit��5.      $��E_xit��61   {�. 
���   � 5/      _� 50      .��6"   Exit the Game61   52      53      m� 5:      -   5        $ menuItemHelp��5:      $��_Help��6-   .@�5<      m� -	   5        $ menuItemInstructions��5<      $��_Instructions��69    5=      _� 5>      .��6"   Game Instructions65   5@      m� 5B      -
   5        $ menuItemUpdateDB��5B      $"��_Admin Mode: Update Database��65   {�.   5C      _� 5D      .��6"   :8Admin Tool to update the Database tables from text files6\   5F      m� 5H      -   5        $ menuItemAbout��5H      $��_About��62    5I      _� 5J      .��6"   About the Game62   5L      5M      5O      c� 5R   
   -   5         c�Name$ toolBar��5R      $
��Left=�6#   $��40��6>   $��0,0,0,0q�6J   $	��Top=�6[   $	��792��6s   
�������
���   � `� IsLocked . $��2��6�   .@�5U      �� -   5        	 ��Name$	 btnNew��5U      $��32��6%   $��Center=�61   $��32��6L   $��	10,0,10,0q�6W   {�. .��5V      �� -   5        
 ��Name$
 imgNew��5V      $$��icons/iconmonstr-file-2-48.png��6(    5W      _� 5X      .��6   Start New Game6.   5Z      �� 5]      -   5        $	 btnSave��5]      $��32��6&   $��Center=�62   $��32��6M   $��	10,0,10,0q�6X   {�.  .��5^      �� -   5        $
 imgSave��5^      $$��icons/iconmonstr-save-5-48.png��6)    5_      _� 5`      .��6   	Save Game6)   5b      �� 5e      -   5        $	 btnLoad��5e      $��32��6&   $��Center=�62   $��32��6M   $��	10,0,10,0q�6X   {�. .��5f      �� -   5        $
 imgLoad��5f      $%��icons/iconmonstr-share-8-48.png��6)    5g      _� 5h      .��6   	Load Game6)   5j      �� 5l      �� 5o      -   5        $	 btnInstructions��5o      $��32��6.   $��Center=�6:   $��32��6U   $��	10,0,10,0q�6`   {�. .��5p      �� -   5        $
 imgInstructions��5p      $%��icons/iconmonstr-info-11-48.png��61    5q      _� 5r      .��6   	Load Game6)   5t      �� 5w      -   5        $	 btnAbout��5w      $��32��6'   $��Center=�63   $��32��6N   $��	10,0,10,0q�6Y   {�. .��5x      �� -   5        $
 imgAbout��5x      $%��icons/iconmonstr-info-10-48.png��6*    5y      _� 5z      .��6   	Load Game6)   5|      �� 5~      �� 5�      -   5        $	 btnExit��5�      $��32��6&   $��Center=�62   $��32��6M   $��	20,0,20,0q�6X   {�. .��5�      �� -   5        $
 	imgReadDB��5�      $$��icons/iconmonstr-door-6-48.png��6+    5�      _� 5�      .��6   Exit the Game6-   5�      �� 5�      5�      �� 5�   
   -   5         ��Name$ myCanvas��5�      {�. $��Center=�5�      $��Center=�5�      $��3��5�      
������ $��0,0,0,0q�5�      $	��720��5�      $	��720��5�      5�      �� 5�   
   -   5         ��Name$ 
statusBar1��5�      $��4��5�      $
��Left=�5�      $��Bottom=�5�      $	��779��5�      $��50��5�      
�������
���   �.@�5�      �� -   5         ��Name$ sbPlayerNameLabel��5�      $�� Player:��5�      $
��Left=�5�      $	��Top=�5�      $��0,10,0,0q�5�      $��64��5�      $��30��5�      $��18(�5�      �� 5�      -   5        $ sbPlayerName��5�      $�� ��5�      $
��Left=�5�      $	��Top=�5�      $��0,10,0,0q�5�      $	��170��5�      $��30��5�      $��18(�5�      �� 5�      �� 5�      -   5        $ sbPlayerModeLable��5�      $��Mode:��5�      $
��Left=�5�      $	��Top=�5�      $��	10,10,0,0q�5�      $��60��5�      $��30��5�      $��18(�5�      �� 5�      -   5        $ sbPlayerMode��5�      $�� ��5�      $
��Left=�5�      $	��Top=�5�      $��0,10,0,0q�5�      $	��417��5�      $��30��5�      $��18(�5�      5�      �� 5�   
   -   5        $ 
statusBar2��5�      $��5��5�      $
��Left=�5�      $��Bottom=�5�      $	��779��5�      $��50��5�      
�������
���   �.@�5�      �� -    5        $ sbCurrentLocationLabel��5�      $��Current Location:��5�      $
��Left=�5�      $��Right=�5�      $	��Top=�5�      $��Center=�5�      $��0,10,0,0q�5�      $	��145��5�      $��30��5�      $��18(�5�      �� 5�      -!   5        $ sbCurrentLocation��5�      $�� ��5�      $
��Left=�5�      $
��Left=�5�      $	��Top=�5�      $��Center=�5�      $��0,10,0,0q�5�      $	��262��5�      $��30��5�      $��18(�5�      �� 5�      �� 5�      -"   5        $ sbStatusContentLabel��5�      $��Status:��5�      $
��Left=�5�      $��Right=�5�      $	��Top=�5�      $��Center=�5�      $��0,10,0,0q�5�      $��61��5�      $��30��5�      $��18(�5�      �� 5     -#   5        $ sbStatusContent��5     $��Start or Load Game��5     $
��Left=�5     $
��Left=�5     $	��Top=�5     $��Center=�5     $��0,10,0,0q�5     $	��296��5	     $��30��5
     $��18(�5     5     5     5     