   M S B A M L   `   `   `  ���� K  GSmartOfficeMetro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:SmartOfficeMetroSmartOfficeMetro  E'clr-namespace:SmartOfficeMetro.CommandsSmartOfficeMetro.Commands  !    SmartOfficeMetro.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  0SmartOfficeclr-namespace:SmartOfficeMetro  0fsxaml#http://github.com/fsprojects/FsXaml  \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  5command'clr-namespace:SmartOfficeMetro.Commands    � Title$  
MainWindow��$	��458��$��784.5�� � WindowState$ 	Maximized=�(  "MahApps.Metro.Controls.MetroWindow   RightWindowCommands +  %MahApps.Metro.Controls.WindowCommands  .@��� $5��/pack://siteoforigin:,,,/Resources/icon-gear.png���� -   $��Settings�� � 	Resources  �� MergedDictionaries��  %    .���  � ColumnDefinitions �� �� $��4* � � RowDefinitions �� $
��Auto ��� �� $
��2.5* ��� $
��Auto �.+�o� -    o�Name$
 menu��$��
0,10,10,10q�$��2���� -    ��Name$ labelWelcome��$��	Welcome, ��$��1��$��33��$��	10,10,0,0q�$��1��$��Center=�$	��375��$��20(�$
��Left=��� -   	 ��Name$	 imageUserIcon��$��1��$
��Fill=�$��	20,5,21,4q����� *imgSrc+3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -   
  Name$
 popUp��   DoubleClickCommand    SmartOfficeMetro.testClass  "   DoubleClickCommandParameter ��  �� RelativeSource �� *Self+�� $��2��.+��� -    ��Name$ buttonDelivery��$��Delivery��$��2��$	��Top=�$��2q��� -   $ buttonOrderCoffee��$��Order Coffee��$��2��$	��Top=�$��2q�]� -    ]�Name$ 	dockPanel��$��1��$
��Auto����. $��2��$
��Auto��$��Stretch=�$��Stretch=�   Flyouts +  %MahApps.Metro.Controls.FlyoutsControl  -	     Name$ flyoutsControl��.@�#  MahApps.Metro.Controls.Flyout     Header$ Inverse��   Position$ Right=�   Theme$ Accent=�.��� $��10q�$	��400�� �� $
��30px ��� $
��30px ��� $
��30px ��� $
��30px �.+��� $C��=This flyout has the inverse theme of the current window theme��$��0��	� -
    	�Name$ radioButtonBlue��$��Blue Accent��$
��Left=�$��1��$��21��$	��109��$��1��	� -   $ radioButtonRed��$��
Red Accent��$
��Left=�$��1��$��21��$	��104��$��2��	� -   $ radioButtonGreen��$��Green Accent��$
��Left=�$��1��$��20��$	��121��$��3��