   M S B A M L   `   `   `  ����C  ?NBATeams, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:NBATeamsNBATeams  5   	       NBATeams.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:NBATeams     5   	     � Title$	  NBA��5   	   $	��450��6   $	��800��6"    � Icon$ unnamed.png��6.   .��5	      � .+�5   
   �� -   5         ��Name$ 	ComboBox1��5      $
��Left=�6&   $	��Top=�6A   $	��120��6Y   $��
81,110,0,0q�6e   .@�5      �� $
��Team��6   �� 5      $��Speler��6   �� 5      $��Coach��6   5      �� 5   
   -   5         ��Name$ TextBox1��5      $
��Left=�6$   $��23��6?   $��
81,170,0,0q�6K    �� TextWrapping$
 Wrap=�6_   $�� ��6s   $	��Top=�6{   $	��120��6�   �� 5   
   -   5         ��Name$ Button1��5      $��	Toevoegen��6"   $
��Left=�66   $��
81,225,0,0q�6Q   $	��Top=�6e   $��75��6}   6�   �� 5   
   -   5         ��Name$ 
TextBlock1��5      $
��Left=�6(   $
��Wrap=�6C   $D��>Selecteer hier of u een team, speler of coach toe wilt voegen:��6W   $	��Top=�6�   $��	81,55,0,0q�6�   $��50��6�   $	��120��6�   �� 5   
   -   5        $ 
TextBlock2��5      $
��Left=�6(   $��
81,149,0,0q�6C   $
��Wrap=�6W   $��Naam:��6k   $	��Top=�6x   �� 5   
   -   5        $ 	ComboBox2��5      $
��Left=�6&   $��236,170,0,0q�6A   $	��Top=�6V   $	��120��6n   5      �� 5   
   -   5        $ 
TextBlock3��5      $
��Left=�6(   $��236,149,0,0q�6C   $
��Wrap=�6X   $
��Team��6l   $	��Top=�6x   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 	DataGrid1��5      $
��Left=�6D   $	��187��6_   $	��Top=�6l   $	��273��6�   $��
421,58,0,0q�6�   ��6&   �� *Teams+6�   �� 5   
   -	   5        $ Button2��5      $��Update��6"   $
��Left=�63   $��179,225,0,0q�6N   $	��Top=�6c   $��75��6{   6�   �� 5   
   -
   5        $ Button3��5      $��Delete��6"   $
��Left=�63   $��281,225,0,0q�6N   $	��Top=�6c   $��75��6{   6�   �� 5   
   -   5        $ Button4��5      $��Confirm��6"   $
��Left=�64   $��
81,266,0,0q�6O   $	��Top=�6c   $��75��6{   6�   �� 5   
   -   5        $ TextBox2��5      $
��Left=�6$   $��23��6?   $��
81,222,0,0q�6K   $
 Wrap=�6_   $�� ��6s   $	��Top=�6{   $	��120��6�   �� 5   
   -   5        $ 	ComboBox3��5      $
��Left=�6&   $��236,222,0,0q�6A   $	��Top=�6V   $	��120��6n   6z   �� 5   
   -   5        $ Button5��5      $��Return��6"   $
��Left=�63   $��179,266,0,0q�6N   $	��Top=�6c   $��75��6{   6�   �� 5   
   -   5        $ 
TextBlock4��5      $
��Left=�6(   $��
81,201,0,0q�6C   $
��Wrap=�6W   $��Nieuwe Naam:��6k   $	��Top=�6   �� 5    
   -   5        $ 
TextBlock5��5       $
��Left=�6(   $��236,201,0,0q�6C   $
��Wrap=�6X   $��
Nieuw Team��6l   $	��Top=�6~   5#      5$      