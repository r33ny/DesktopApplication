   M S B A M L   `   `   `  ����  SmartOfficeMetro    SmartOfficeMetro.Login   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  5      -   5          � Title$  Login��5      $	��350��5      $	��525��5       � 
ResizeMode$ NoResize=�5       � 	Resources 5	          BooleanToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   �� -   5         ��Name$ iconUser��5      $
��76px��6#   $
��76px��60    J� Clip4 ��        � ��    � �� ���     ���        $��Center=�6f   $��Center=�6�   $��1��6�   .+�5      F� $��76��6   $��76��6    �� Stretch$
 Fill=�6*   ���	#FFFFFFFF�������C��!��d���!���<R�o�X���LU���4��`��4��x��4��D��쮷~�������ɓ@@�4Y�N����#$0P��4�0P��;��d��TI����0P���x��Tn5��ĉE���@�e ���@�e@�e���!@�e���! ���<!B����<R��4���x�� �0P���TI��Z��;���Z��4��镲9@@�#$�Q�Nl����ɓ@@���~����D�����x�����`���U���(;�X������<R<F���!��C��!6J   5      �� 5   
   -   5         ��Name$ textBoxUsername��5      $��23��6+   $��	10,14,0,0q�67   	 �� TextWrapping$
	 Wrap=�6J   $��Center=�6^   $	��160��6y   
 � ToolTip$ 
 Please enter your username��6�   $��1��6�   $��1��6�   G� 5    
   -   5         G�Name$ passwordBox��5       $��	10,11,0,0q�6+   $��Center=�6>   $	��160��6Y   $ 
 Please enter your password��6e   $��23��6�   $��1��6�   $��2��6�   �� 5!   
   -   5         ��Name$ label��5!      $��
Username: ��6   $��
103,10,0,0q�64   $��Center=�6H   $��1��6c   $��Right=�6p   �� 5"   
   -   5        $ label1��5"      $��
Password: ��6    $��Right=�65   $��
107,11,0,0q�6Q   $��Center=�6e   $��2��6�   �� 5#   
   -   5         ��Name$ buttonLogin��5#      $��Login��6&   $��	10,17,0,0q�66   $	��Top=�6I   $	��160��6a   $��1��6m   $��3��6}   6�   �� 5&   
   -   5         ��Name$ progressBackground��5&      $��3��60   ���	#56FFFFFF$
��Left=�6U   $	��321��6p   $��5��6}   $	��Top=�6�   $	��519��6�   y�6�   �� *	isVisible+ �� 	Converter# [  )  #MahApps.Metro.Controls.ProgressRing  5'   
   -	   5          Name$ 
inProgress��5'       $��1��64   $
��Left=�6D   $��60��6_   $��	43,23,0,0q�6k   $	��Top=�6~   $��82��6�   $��1��6�   $��2��6�   y�6�   �� *	isVisible+# [  5*      (  "MahApps.Metro.Controls.MetroWindow   Flyouts 5.      +  %MahApps.Metro.Controls.FlyoutsControl  5/   
   -
   5          Name$ flyoutsControl��5/   "   .@�51      #  MahApps.Metro.Controls.Flyout     Header$ Settings��6      Position$ Right=�52         Theme$ Accent=�53      .��54      � $��10q�6   $	��400��55       56      �� 57      $
��30px �6(   �� 58      $
��30px �6(   �� 59      $
��30px �6(   �� 5:      $
��30px �6(   .+�5<      �� $C��=This flyout has the inverse theme of the current window theme��6    $��0��6e   5?      5@      5B      5E      