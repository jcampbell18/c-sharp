   M S B A M L   `   `   `  ����J  Fcampbelljproj2d, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:campbelljproj2dcampbelljproj2d  5   	   "    campbelljproj2d.PlayerWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:campbelljproj2d     5   	     � Title$  Player Information��5   	    � WindowStartupLocation$ CenterScreen=�5	   	   $	��285��5
   	   $	��355��5   	   .��5      � .+�5   
   �� -   5         ��Name$ lblPlayerName��5      $��Player's Name:��5      $
��Left=�5      $��Right=�5      $	��Top=�5      $��Center=�5      $��	10,20,0,0q�5      $��90��5      $��40��5      �� 5   
   -   5         ��Name$ tbPlayerName��5      $�� ��5       �� TextWrapping$
 Wrap=�5       �� 	MaxLength$ 15��5      $
��Left=�5      $
��Left=�5      $	��Top=�5      $��Center=�5      $��
105,20,0,0q�5       $	��200��5!      $��40��5"      �� 5#   
   -   5        $ lblCharRestrict��5$      $��(up to 15 characters)��5%      $
��Left=�5&      $��Center=�5'      $	��Top=�5(      $	��Top=�5)      $��
105,60,0,0q�5*      $	��200��5+      $��25��5,      $��Italic$�6   $��10(�6,   �� 5.   
   -   5        $ lblMazeSize��5/      $��
Maze Size:��50      $
��Left=�51      $��Right=�52      $	��Top=�53      $��Center=�54      $��	10,87,0,0q�55      $��90��56      $��40��57      �� 59   
   -   5         ��Name$ 
cbMazeSize��59      $
��Left=�5:      $	��Top=�5;      $��
105,95,0,0q�5<      $	��200��5=      .@�5?      �� .��6   2x2 (4 rooms)6*   �� 5@       �� 
IsSelected . .��6-   3x3 (9 rooms)6<   �� 5A      .��6   4x4 (16 rooms)6+   �� 5B      .��6   5x5 (25 rooms)6+   5D      �� 5F   
   -   5        $ lblCategory��5G      $��Trivia Category��5H      $
��Left=�5I      $��Center=�5J      $	��Top=�5K      $��Center=�5L      $��
10,130,0,0q�5M      $��90��5N      $��40��5O      �� 5Q   
   -   5        $ 
cbCategory��5Q      $
��Left=�5R      $	��Top=�5S      $��105,140,0,0q�5T      $	��200��5U      .@�5W      �� .��6   Mixed Trivia6)   �� 5X      .��6   	Geography6&   �� 5Y       . .��6-   Entertainment6<   5_      �� 5a   
   -   5         ��Name$ 	btnSubmit��5b      $��Submit��5c      $
��Left=�5e      $	��Top=�5f      $��230,190,0,0q�5g      $��75��5h      $��40��5i      5k      5l      