   M S B A M L   `   `   `  ����   Video Player Remake    Media_Player_Remake.View   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $	��650��$	��900��$	��600��$	��350��  � Icon$  /Images/ico.ico�� � Title �� *Player.FileName+ � WindowState �� *
FullScreen+ �� Mode$ OneWay=� � WindowStyle �� *WindowBorders+$ OneWay=� � Cursor �� *Cursor+ J� InputBindings d�  d� MouseAction$ MiddleClicke� �� Command �� *	HideMouse+�� 
	 �� Key$	 F2�� �� *Help+�� $
	 Home��
 �� CommandParameter
 �� *Player+ �� *JumpToStart+�� $		 End��
 �� *Player+ �� *	JumpToEnd+�� $	 Space��
 �� *Player+ �� *Play+�� $	 M�� �� 	Modifiers$
 Ctrlf�
 �� *Player+ �� *Mute+�� $	 S��$
 Ctrlf�
 �� *Player+ �� *Stop+�� $	 O��$
 Ctrlf�
 �� *Player+ �� *
OpenFile+�� $	 R��$
 Ctrlf�
 �� *Player+ �� *Repeat+�� $	 F�� �� *ToggleFullScreen+�� $		 F11�� �� *ToggleFullScreen+�� $	 H��$
 Ctrlf� �� *HideControlPanel+�� $
	 Left��$
 -1�� �� *	ChangePos+�� $	 Right��$
 1�� �� *	ChangePos+�� $	 Up��$
 5�� �� *	ChangePos+�� $
	 Down��$
 -5�� �� *	ChangePos+�� $	 D��$
 Ctrlf� �� *
SwapDark+.��� 
���   � � RowDefinitions �� ���� *DefaultRowHeight+�� $��* ��� ���� *DefaultRowHeight+�� ���� *AdditionalRowHeight+.+�]� $��0�� � 	Resources  �� 
TargetType �� Property �� Value��%�  '��       ']� j     '��     'd� �    �� " ��.���  � ��$ 30����  � ��$ 15����  �@��BitmapScalingMode$ HighQuality=��� " ]�.���  � y� �� *DockPanelVisibility+��  � �� .  ��  � �� �� *PrimaryDockPanelColor+��  �@J�Opacity �� *DockPanelOpacity+�� " ��.���  � |� .  ��  � ��$ 3,3,0,3q���  �@^�ShowDuration$
 1500����  �@^�InitialShowDelay$
 1500����  � ��
 ���� ��  � ��$ 0q��� " d�.���  �@^�ShowDuration$
 1500����  �@^�InitialShowDelay$
 1500����  � |� .  ��  � ��
 ���� ��  � ��$ 0q���  � ��$ 3,3,0,3q�.+��� ���� *
OpenFile+���� *Player+.���� $��/Images/folder.png�� � ToolTip _� .��CTRL+Od�  d� 	IsChecked �� *Player.Repeat+.���� $��/Images/repeat.png�� _� .��CTRL+Rd�  �� *
DarkMode+.���� $��/Images/dark.png�� _� .��CTRL+D�� ���� *ToggleFullScreen+.���� $��/Images/fullscreen.png�� _� .��F�� ���� *HideControlPanel+.���� $��/Images/hidePanel.png�� _� .��CTRL+H�� ���� *Help+.���� $��/Images/question.png�� _� .��F2r� -    r�Name$ mediaElement�� ^� InitialShowDelay$
 1000�� ^� ShowDuration$
 1000��$
��Auto��$��1�� d� $ 
RightClicke� �� *Play+
 �� *Player+d� $ LeftDoubleClicke� �� *ToggleFullScreen+d� $ RightDoubleClicke�
 �� *Player+ �� *
OpenFile+ _� .��]� .+��� $��/Images/dblClick.png��$��90���� $��/Images/rmbClick.png��$��90���� $��/Images/middleClick.png��$��90��]�  ��%>  ']�       '�� �     '�� �    '�� 3    '�� �    �� " ]�.���  � y� �� *DockPanelVisibility+��  � �� �� *PrimaryDockPanelColor+��  � ��$ Stretch=���  � ��$ Bottom=���  � ��$ 2����  �@J�Opacity �� *DockPanelOpacity+�� " ��.���  �@^�ShowDuration$
 1500����  � |� .  ��  � ��$ 0,0,7,0q���  � �� �� *ButtonsColor+��  � ��$ 0q���  � ��$ 50����  � ��$ 25���� " ��.���  �@�Minimum$ 0Q���  �@�Cursor$ SizeAll����  � ��$ 0,0,10,0q���  � ��$ Center=��� " ��.���  � ��$ 20����  �@��BitmapScalingMode$ HighQuality=��� " ��.���  � �� �� *PrimaryForegroundColor+��  � ��$ 5q���  � ��$
 Auto����  � ��$ Center=�.+��� ���� *Play+���� *Player+.���� $��/Images/play.png�� _� .���� $��/Images/space.png��$	��100���� ���� *Stop+���� *Player+.���� $��/Images/stop.png�� _� .��
CTRL + S�� $��25������ *Player.Volume+ �� .+��� $��CTRL+M���� $��/Images/rmbClick.png�� d� $ 
RightClicke� �� *Mute+
 �� *Player+��  � Maximum$	 100Q�$	��100�� � Value �� *Player.Volume+$ TwoWay=��� $	��115������ *Player.PositionTimeSpan+��  �� *Player.MaxLen+ �� *Player.PositionDouble+�� |�.  $
��Left=�$��Bottom=�$	��5,0q�$��3�� J� Opacity �� *RestoreButtonOpacity+���� *HideControlPanel+.���� $��8��$��/Images/hidePanel.png�� _� .��CTRL+H