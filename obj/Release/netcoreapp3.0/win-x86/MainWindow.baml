   M S B A M L   `   `   `  ���� U  Qneteas-cloud-music_analyze, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:neteas_cloud_music_analyzeneteas_cloud_music_analyze  a5clr-namespace:neteas_cloud_music_analyze.SmoothScroll'neteas_cloud_music_analyze.SmoothScroll  +    %neteas_cloud_music_analyze.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     :local(clr-namespace:neteas_cloud_music_analyze     Hlocall5clr-namespace:neteas_cloud_music_analyze.SmoothScroll       � Title$!  Netease-cloud-music Analyze��$	��710��$
��1000��   MaterialDesignBody#���   ���#efe5fd$��Medium"�$��14(�$X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.����  �� Padding$ 8q�._��  � ColumnDefinitions �� $��* ��� $��3* �.+���  �� Orientation$ Vertical=�.+��� $��0��$��0 0 10 0q�.+��  �� $��* ��� $��* ��� $��* �.+��� -    ��Name$ ApplyButton��$��0��$��Apply��$��5 0 5 0q��� -   $ ResetButton��$��1��$��Reset��$��5 0 5 0q��� -   $ 
HelpButton��$��2��$
��Help��$��5 0 5 0q���    MaterialDesignTitleTextBlock#���  $��Basic��$
��Bold"�$�� 0 10q��� $	��URL���� -    ��Name$ URLText��$��2q��  �� $��2* ��� $��* �.+��� $��1��.+��� $��List Id���� -   $ 
ListIdText�� �� 
IsReadOnly$
 True�����#eee�� $��0��$��0 0 10 0q�.+��� $��Status���� -   $ 
StatusText��$
 True�����#eee�� -   $ CaculateButton��$��统计歌词信息��$��0 15 0 0q��� $��
Word Cloud��$
��Bold"�$
��0 10q��� -   $��UseOtherPictureCheck��$��使用自定义背景���� $��预置背景��$��0 5 0 0 q��� -	    ��Name$ ImageSelect�� �� SelectedIndex$ 0��$��2q�.@��� -
   $��HeartShapeCombo��.��
HeartShape�� -   $��PikaqiuCombo��.��Go�� $��自定义背景路径��$��0 5 0 0 q��� -   $��PicturePathText��� $��0 5 0 0q� �� $��* ��� $��* ��� $��* �.+��� $��0��$��0 0 5 0q�.+��� $��显示歌词数���� -   $��WordNumText���� $��1��$��0 0 5 0q�.+��� $��字体最大值���� -   $��WordMaxSizeTsxt���� $��2��$��0 0 5 0q�.+��� $��图片清晰度���� -   $��PictureSacleText���� -   $ PrintWCButton��$��显示词云��$��0 10 0 0q��� $��Look up��$
��Bold"�$
��0 10q��� $��查找某词词频���  �� $��2* ��� $��* �.+��� $��0��$��0 0 10 0q�.+��� $
��Word���� -   $ TargetWordText���� $��1��.+��� -   $��查找��$
��0 10q��� $��丢弃词汇���� $
��Word���� -   $��TrushWordText��� $��0 5 0 0 q� �� $��* ��� $��* �.+��� -   $��0��$��丢弃��$��0 0 5 0q��� -   $��1��$��移除丢弃��$��5 0 0 0 q��� $��1��.@��� -   $��tebItem1��$��歌词统计信息��.��� .+��� $��统计信息��$
��Bold"�$�� 0 10q��  �� $��2* ��� $��3* �.+�� $��0��$��5 0 5 0q� �� $
��158* ��� $
��125* �	 � RowDefinitions	 �� $��2* ��� $��3* �.+��� $��0��$��2��$��	0,0,0,391q�$��2��.+��� -   
 ��Name$
 	TotalInfo��$��30q�
������ $
 True��$|�False�� � Cursor$ Arrow���� -    ��Name$ WordListView��$��1��$��10q�$��2�� �� View �� .P��� -   WordItem��$��单词�� �� Width$	 120�� �� DisplayMemberBinding �� *word+�� -   NumItem��$��词频��$	 120�� �� *	frequency+� $��1��$��5 0 5 5q�.+��� -   $ MusicListView��$��1��$��10q� �� .P��� $��曲目��$	 300���� -   $��tabItem2��$��词云显示��.��� .+��� -   $��	wordcloud���� -   $��tabItem3��$��查找结果��.��� .+��  �� $��2* ��� $��3* �.+�� $��0��$��5 0 5 0q�	 �� $��2* ��� $��3* �.+��� $��0��.+��� $��查找结果：��$
��Bold"��� -   $
 
SearchInfo��$��30q�
������ $
 True��$|�False��$ Arrow���� -    $��TrashListView��$��1��$��10q� �� .P��� -!   	TrashItem��$��移去的词��$	 120��� $��1��$��5 0 5 5q�.+��� -"   $ SearchListView��$��1��$��10q� �� .P��� -#   InSongs��$��出现在以下曲目中��$	 300�� �� *song+�� -$   Num��$��词频��$ 80�� �� *num+�� -%   $��tebItem4��$��歌词��.��� .+�@   :neteas_cloud_music_analyze.SmoothScroll.SmoothScrollViewer  -&     Name$ scrollviewer��@   :neteas_cloud_music_analyze.SmoothScroll.SmoothScrollViewer   ScrollChanged$  scrollviewer_ScrollChanged��$
��Auto=�$��Disabled=�.���� -'   $
 showInfo��$��30q�
������ $
 True��$|�False��$ Arrow��