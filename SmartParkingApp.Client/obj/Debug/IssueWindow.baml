   M S B A M L   `   `   `  ����Q  MSmartParkingApp.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SmartParkingApp.ClientSmartParkingApp.Client  5   	   (    "SmartParkingApp.Client.IssueWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:SmartParkingApp.Client     5   	   R NMahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5	   	     � WindowStartupLocation$  CenterScreen=�5   	    � WindowStyle$
 None=�5
   	    � 
ResizeMode$ NoResize=�5   	    � Title$ IssueWindow��5   	   $	��200��6   $	��450��6*   $��2q�66   '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�6      ResizeBorderThickness$ 2q�6)   6E   ��5      �� 5   
    �� Color$ Black��6    �� Opacity$	 0.5Q�6(   68   .��5      ]� ��. .+�5   
   � -   5        $	��Top=�5      $��35��6$   ���#FFAFBD	 J� Effect	 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      
   
BlurRadius$
 1Q�6#      ShadowDepth$ 1Q�62      Color$ 	#FFACACAC��6B   .+�5      �� $��Center=�6   $
��auto��65   $��Center=�5       J� Opacity$	 0.7Q�63   �� 5      
���   �$��Arial)�6)   $��30(�6<   .�6J   Issue6Q   5      �� 5      $��
0, 2, 2, 2q�6    �� Orientation$ 
Horizontal=�6-   $��Right=�6F   .+�5      �� -   5         ��Name$ BtnClose��5      ���#FFAFBD$��40��5      $��2q�5       .��5!      .  (MahApps.Metro.IconPacks.PackIconMaterial  E AControlzEx, Version=3.0.2.4, Culture=neutral, PublicKeyToken=null�  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null]]   Kind$ WindowClose=�5"      $
��auto��6(   $	 0.7Q�5#      5$      5%      5&      �� 5'   
   $ Vertical=�6   $��0, 15, 0, 0q�6,   .+�5(        $ Alert=�6)   $��Center=�66   $��50��6S   $��50��6^   �� 5)      -   5         ��Name$ IssueTextBlock��5)      $��
0, 5, 0, 0q�60   $$��Smart Parking App Registration��6D   $
��Wrap=�5*      $��Center=�5+      $��Center=�5,      $��Light"�5-      $��arial)�5.      $	��434��5/      $��85��50      $��20(�51       �� TextAlignment$ Center=�52      54      ��55   
   �� 56      .��57      �  � Color$ #FFAFBD��6    � Offset$	 0.0Q�6/   � 58      $ #FFC3A0��6   $ 1Q�6/   59      5;      5=      