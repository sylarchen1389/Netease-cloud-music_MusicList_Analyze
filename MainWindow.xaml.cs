using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using neteas_cloud_music_analyze.SmoothScroll;
using System.Data;

namespace neteas_cloud_music_analyze
{

    /// <summary>
    /// 词汇结构体
    /// </summary>
    public partial class HotWord
    {
        public string word;
        public Int32 frequency;
        public List<string> music = new List<string>();

        public HotWord() { }
        public HotWord(string word,Int32 frequency) {
            
            this.word = word;
            this.frequency = frequency;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 滚动条滑动补充
        /// </summary>
        double m_ScrollStepRatio = 0.0;     //滚动条的归一化步进长度
        double m_ScrollPositionRatio = 0.0; //滚动条的归一化位置


        /// <summary>
        /// 资源路径Lyrics
        /// </summary>
        static string MUSIC_INPUT_PATH = @".\assert\output\music_output.txt";
        static string WORD_INPUT_PATH = @".\assert\output\word_output.txt";
        static string INFO_INPUT_PATH = @".\assert\output\info_output.txt";
        static string SEARCH_INPUT_PATH = @".\assert\output\search_output.txt";

        static string IGNORE_PATH = @".\assert\lyrics\Trash.txt";
        static string dir_path = @".\assert\lyrics\";

        //static string input_img_dir_path = @".\assert\source_img\";
        static string ouput_img_dir_path = @".\assert\output_img\";


        /// <summary>
        /// exe 路径
        /// </summary>
        static string EXE_PATH = @".\LyricsPoccess.exe";
        static string REQUSET_EXE_PATH = @".\request.exe";


        /// <summary>
        /// 词汇及忽略词公共变量
        /// </summary>
        string targetWord;
        //Int32 wordSize;
        //Int32 targetFrequency;
        List<HotWord> myList = new List<HotWord>();
        List<string> ignoreList = new List<string>();


        /// <summary>
        /// 主界面初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            LoadDocument();
            ListIdText.Text = StatusText.Text = null;
            UseOtherPictureCheck.IsChecked = false;
            WordNumText.Text = "100";
            WordMaxSizeTsxt.Text = "125";
            PictureSacleText.Text = "32";

            System.IO.File.Delete(dir_path+@"param.txt");
            FileStream fs = new FileStream(dir_path+@"param.txt", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.UTF8.GetBytes("HeartShape.jpg" + Environment.NewLine);
            fs.Write(data, 0, data.Length);
            //开始写入
            fs.Flush();
            fs.Close();

            //LoadWordList();
            
        }

        /// <summary>
        /// apply确认URL，进行歌词爬取
        /// </summary>
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {

            this.StatusText.Text = new string("爬取歌词中");
            FileStream fs = new FileStream(dir_path+@"url.txt", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.UTF8.GetBytes(this.URLText.Text);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();

            System.Diagnostics.Process myexe = new System.Diagnostics.Process();
            myexe.StartInfo.FileName = REQUSET_EXE_PATH;
            myexe.StartInfo.UseShellExecute = true;
            myexe.StartInfo.CreateNoWindow = false;
            myexe.Start();
            //等待外部程序退出后才能往下执行
            //MessageBox.Show("统计中");
            myexe.WaitForExit();
            MessageBox.Show("歌曲爬取完成");
            //System.Diagnostics.Process p = new System.Diagnostics.Process();
            //p.StartInfo.FileName = @"request.exe";
            //p.Start();
            //while (!p.HasExited) { } //是否正在运行

            // 从文件中读入内容并显示

            //txtBlockOutpuMessage = new TextBlock();
            //txtBlockOutpuMessage.Text = null;
            //System.IO.StreamReader sr = new System.IO.StreamReader(@"lyrics.txt");
            //string str;
            //while ((str = sr.ReadLine()) != null){
            //    txtBlockOutpuMessage.Inlines.Add(str);
            //    txtBlockOutpuMessage.Inlines.Add(new LineBreak());
            //}

            //txtBlockOutpuMessage.Inlines.Add(new LineBreak());
            LoadDocument();
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusText.Text = new string("重载中");
            ListIdText.Text =StatusText.Text = null;
            UseOtherPictureCheck.IsChecked = false;
            WordNumText.Text = "100";
            WordMaxSizeTsxt.Text = "125";
            PictureSacleText.Text = "32";
            this.StatusText.Text = new string("重载完成");
        }

        /// <summary>
        /// 使用信息打印
        /// </summary>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 统计信息按钮press
        /// </summary>
        private void CaculateButton_Click(object sender, RoutedEventArgs e)
        {
            //this.StatusText.Text = new string("统计歌词信息中");
            LoadWordList();
            IgnoreInit();
            LoadInfo();
            this.StatusText.Text = new string("歌词信息统计完成");
        }

        /// <summary>
        /// 打印词云
        /// </summary>
        private void PrintWCButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusText.Text = new string("绘制词云中");
            wordcloud.Source = null;
            if (this.UseOtherPictureCheck.IsChecked != false)
            {
                FileStream fs = new FileStream(dir_path+@"param.txt", FileMode.Create);

                byte[] data = System.Text.Encoding.UTF8.GetBytes(this.PicturePathText.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length);


                data = System.Text.Encoding.UTF8.GetBytes(this.WordNumText.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length);
                data = System.Text.Encoding.UTF8.GetBytes(this.WordMaxSizeTsxt.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length);
                data = System.Text.Encoding.UTF8.GetBytes(this.PictureSacleText.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();

            }
            else {

                StreamReader fi = new StreamReader(dir_path+@"param.txt", Encoding.UTF8);
                string imgName = fi.ReadLine();
                fi.Close();

                FileStream fs = new FileStream(dir_path+@"param.txt", FileMode.Create);

                byte[] data = System.Text.Encoding.UTF8.GetBytes(imgName + Environment.NewLine);
                fs.Write(data, 0, data.Length);

                data = System.Text.Encoding.UTF8.GetBytes(this.WordNumText.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length);
                data = System.Text.Encoding.UTF8.GetBytes(this.WordMaxSizeTsxt.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length);
                data = System.Text.Encoding.UTF8.GetBytes(this.PictureSacleText.Text + Environment.NewLine);
                fs.Write(data, 0, data.Length); 
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();

            }
           
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"printWordCloud.exe";
            p.Start();
            while (!p.HasExited) { }

            string fileName = DateTime.Now.ToString("yyyy-MM-dd");

            fileName = fileName + DateTime.Now.ToString("_hh-mm-ss");
            fileName += ".jpg";


            wordcloud.Source = null;
            System.IO.File.Copy(ouput_img_dir_path+"wordcloud.png", ouput_img_dir_path+ fileName, true);
            string str = System.Environment.CurrentDirectory;
            wordcloud.Source = new BitmapImage(new Uri(str+"\\assert\\output_img\\" +  fileName));
            //BitmapImage image = new BitmapImage(new Uri(@".\woedcloud", UriKind.Absolute));
            //wordcloud.Source = image;

            this.StatusText.Text = new string("绘制词云完成");
        }

        /// <summary>
        /// 加载歌词
        /// </summary>
        private void LoadDocument()
        {
            this.showInfo.Document.Blocks.Clear();
            int flag = 0;
            string filePath = dir_path + @"lyrics.txt";//文件路径
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);//以只读方式打开源文件
                try
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        FlowDocument document = new FlowDocument();//承载文件内容
                        string content = sr.ReadToEnd();
                        string[] split = content.Split(new char[2] { '\r', '\n' });
                        foreach (string para in split)
                        {
                            if (para != "")
                            {
                                Paragraph paragraph = new Paragraph(new Run(para));
                                paragraph.FontFamily = new FontFamily("微软雅黑");//修改样式
                                paragraph.Foreground = new SolidColorBrush(Colors.Black);
                                if (flag == 1)
                                {
                                    paragraph.FontSize = 26;
                                    paragraph.TextAlignment = TextAlignment.Center;
                                    flag = 0;
                                }
                                else {
                                    paragraph.FontSize = 20;    
                                    if (para[0] == '*')
                                        flag = 1;
                                }
                                    
                                document.Blocks.Add(paragraph);
                            }
                        }
                        this.showInfo.Document = document;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 滑动补偿
        /// </summary>
        private void scrollviewer_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            m_ScrollStepRatio = scrollviewer.ViewportHeight / (scrollviewer.ExtentHeight - scrollviewer.ViewportHeight);
            m_ScrollPositionRatio = scrollviewer.ContentVerticalOffset / scrollviewer.ScrollableHeight;
            
            if (m_ScrollStepRatio< 0.5 || m_ScrollPositionRatio< 0.5 ) {
                m_ScrollStepRatio = 0;
                m_ScrollPositionRatio = 0;
                return;
            }
            if (e.VerticalChange < -1) 
                scrollviewer.SmoothScroll(m_ScrollStepRatio, m_ScrollPositionRatio, ScrollDirection.Up);
            else if (e.VerticalChange > 1)
                scrollviewer.SmoothScroll(m_ScrollStepRatio, m_ScrollPositionRatio, ScrollDirection.Down);

            
        }

        /// <summary>
        /// 预置图片选择控制
        /// </summary>
        private void ComboxBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 此时的 myComboxBox.SelectedValue = System.Windows.Controls.ComboBoxItem: 选项的内容
            // 所以如果用SelectedValue这种方法获取选中的值，还需要切割字符串

            if (this.UseOtherPictureCheck.IsChecked != false)
                return;

            ComboBoxItem item = ImageSelect.SelectedItem as ComboBoxItem;
            string content = item.Content.ToString();

            FileStream fs = new FileStream(dir_path+@"param.txt", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.UTF8.GetBytes(content + ".jpg" + Environment.NewLine);
            //开始写入
            fs.Write(data, 0, data.Length);

            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 忽略列表初始化
        /// </summary>
        /// TODO：重复？
        private void IgnoreInit()
        {
            this.TrashListView.Items.Clear();
            StreamReader inFile = new StreamReader(IGNORE_PATH, Encoding.Default);
            string trashword = inFile.ReadToEnd();

            string[] ts = trashword.Split(new char[3] { ' ','\n','\r'});
            ignoreList = new List<string>();

            foreach (string word in ts) {
                if (word == " " ||word == "  "||word == null|| word == "")
                    continue;
                this.TrashListView.Items.Add(word);
                ignoreList.Add(word);
            }

            inFile.Close();

        }

        /// <summary>
        /// 加载统计信息
        /// </summary>
        private void LoadInfo()
        {
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + @".\assert\output\info_output.txt";//文件路径
            //if (File.Exists(filePath))
            //{
                //FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read );//以只读方式打开源文件
                
            StreamReader sr = new StreamReader(INFO_INPUT_PATH, Encoding.Default);
                    
                    
            FlowDocument document = new FlowDocument();//承载文件内容
                                                       //string content = sr.ReadToEnd();
            string content;
            List<string> infoList = new List<string>();
            while ((content = sr.ReadLine()) != null) {
                infoList.Add(content);
            }
            List<string> CnList = new List<string>();
            CnList.Add("总词数：");
            CnList.Add("不重复的单词数：");
            CnList.Add("曲目数量：");
            CnList.Add("忽略的单词数：");
            //string[] split = content.Split(new char[2] { '\r', '\n' });

            //document.Blocks.Add(new Paragraph(new Run(" ")));
            for (int i=0; i < infoList.Count; ++i)
            {
                string para = CnList[i] + infoList[i];
                if (para != "")
                {
                    
                    Paragraph paragraph = new Paragraph(new Run(para));
                    paragraph.FontFamily = new FontFamily("宋体");//修改样式
                    paragraph.Foreground = new SolidColorBrush(Colors.Purple);
                    paragraph.FontSize = 18;
                                
                    document.Blocks.Add(paragraph);
                }
            }
            this.TotalInfo.Document = document;
                    
            //}
            //catch (Exception ex)
                //{

                //}
                //finally
                //{
                //    sr.Close();
                //}
            //}
        }

        /// <summary>
        /// 加载词汇列表
        /// </summary>
        private void LoadWordList()
        {
            this.MusicListView.Items.Clear();
            this.WordListView.Items.Clear();
            targetWord = this.TargetWordText.Text;
            System.Diagnostics.Process myexe = new System.Diagnostics.Process();
            myexe.StartInfo.FileName = EXE_PATH;
            myexe.StartInfo.Arguments = targetWord;
            myexe.StartInfo.UseShellExecute = false;
            myexe.StartInfo.CreateNoWindow = true;
            myexe.Start();
            //等待外部程序退出后才能往下执行
            //MessageBox.Show("统计中");
            myexe.WaitForExit();
            MessageBox.Show("统计完毕");

            string songName;
            //读取曲目内容
            StreamReader inFile = new StreamReader(MUSIC_INPUT_PATH, Encoding.Default);
            
            while ((songName = inFile.ReadLine()) != null)
            {
                if (songName.Length <= 1)
                    continue;
               
                MusicListView.Items.Add(songName);
            }
            inFile.Close();

            //读取单词信息
            inFile = new StreamReader(WORD_INPUT_PATH, Encoding.Default);
            ////指定单词词频
            //line = inFile.ReadLine();
            //targetFrequency = Convert.ToInt32(line);
            //label1.Text = line;
            //单词总数
            string line;
            while ((line = inFile.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, "^[0-9]*$"))
                {
                    //添加频率
                    HotWord temp = new HotWord();
                    temp.frequency = Convert.ToInt32(line);
                   
                    
                    //读取单词
                    line = inFile.ReadLine();
                    temp.word = line;
                    myList.Add(temp);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("word");
                    dt.Columns.Add("frequency");
                    dt.Rows.Add(temp.word, temp.frequency);

                    this.WordListView.Items.Add(dt);
                    
                }
                else
                {
                    // add item to list
                    myList[myList.Count - 1].music.Add(line);
                }
            }
            inFile.Close();
            
        }

        /// <summary>
        /// 增加忽略词汇
        /// </summary>
        private void addIgnoreButton_Click(object sender, EventArgs e)
        {

            string trash = this.TrushWordText.Text.ToString();
            foreach (string word in ignoreList) {
                if (string.Compare(trash, word) == 0) {
                    return;
                }

            }
            ignoreList.Add(this.TrushWordText.Text.ToString());
            this.TrashListView.Items.Add(this.TrushWordText.Text.ToString());

            FileStream fs = new FileStream(IGNORE_PATH, FileMode.Create);
            string all_trash = "" ;
            foreach (string word in ignoreList) {
                if (word == "")
                    continue;
                all_trash = all_trash + word + ' ';
            }

            byte[] data = System.Text.Encoding.UTF8.GetBytes(all_trash);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            LoadWordList();
        }


        /// <summary>
        /// 移除忽略词汇
        /// </summary>
        private void deleteIgnoreButton_Click(object sender, EventArgs e)
        {

            int flag = 0;
            string selectedword = this.TrushWordText.Text.ToString();
            

            foreach (string word in ignoreList) {
                if (string.Compare(word, selectedword) == 0) {
                    flag = 1;

                }
            }

            if (flag == 0)
                return;

            ignoreList.Remove(selectedword);

            string all_trash = "";
            foreach (string word in ignoreList)
            {
                if (word == "")
                    continue;
                all_trash = all_trash + word + ' ';
            }

            FileStream fs = new FileStream(IGNORE_PATH, FileMode.Create);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(all_trash);
            fs.Write(data, 0, data.Length-1);
            fs.Flush();
            fs.Close();

            LoadWordList();
            IgnoreInit();
            LoadInfo();
        }

        
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // 加载查找结果的list
            this.StatusText.Text = new string("查找中");
            this.SearchListView.Items.Clear();

            string selectedword = this.TargetWordText.Text.ToString();
            MessageBox.Show("正在查找："+selectedword);

           
            System.Diagnostics.Process myexe = new System.Diagnostics.Process();
            myexe.StartInfo.FileName = EXE_PATH;
            myexe.StartInfo.Arguments = selectedword;
            myexe.StartInfo.UseShellExecute = false;
            myexe.StartInfo.CreateNoWindow = true;
            myexe.Start();
            myexe.WaitForExit();
            //StreamReader sr = new StreamReader(SEARCH_INPUT_PATH, Encoding.Default);

            //bool isWordExisit = true;
           
            string totalShowNum;
            string songNum;
            //string songName;
            string line;
            //读取曲目内容
            StreamReader inFile = new StreamReader(SEARCH_INPUT_PATH, Encoding.Default);

            line = inFile.ReadLine();


            FlowDocument document = new FlowDocument();
            if (line[0] == '"' || line[0] == '“') {
                //isWordExisit = false;
                MessageBox.Show(line);
                inFile.Close();

                
                string para = selectedword + "没有在歌单中出现过";
                if (para != "")
                {

                    Paragraph paragraph = new Paragraph(new Run(para));
                    paragraph.FontFamily = new FontFamily("微软雅黑");//修改样式
                    paragraph.Foreground = new SolidColorBrush(Colors.MediumPurple);
                    paragraph.FontSize = 18;

                    document.Blocks.Add(paragraph);
                }
            
                this.SearchInfo.Document = document;

            return;
            }
            totalShowNum = line;
            songNum = inFile.ReadLine();
            
            while ((line = inFile.ReadLine()) != null)
            {
                string[] split = line.Split(new char[2] { '\n','\\' });

                //MessageBox.Show(split[0]);
                DataTable dt = new DataTable();
                dt.Columns.Add("song");
                dt.Columns.Add("num");
                dt.Rows.Add(split[1], split[0]);

                this.SearchListView.Items.Add(dt);
                
            }
            inFile.Close();


            // 加载查找的Info

            //FlowDocument document = new FlowDocument();//承载文件内容
                                                       //string content = sr.ReadToEnd();
            List<string> infoList = new List<string>();
            infoList.Add(selectedword);
            infoList.Add(totalShowNum);
            infoList.Add(songNum);

            List<string> CnList = new List<string>();
            CnList.Add("查找的单词是：");
            CnList.Add("总出现过数：");
            CnList.Add("一共出现过的歌曲数：");
            
           
           

            for (int i = 0; i < infoList.Count; ++i)
            {
                string para = CnList[i] + infoList[i];
                if (para != "")
                {

                    Paragraph paragraph = new Paragraph(new Run(para));
                    paragraph.FontFamily = new FontFamily("微软雅黑");//修改样式
                    paragraph.Foreground = new SolidColorBrush(Colors.MediumPurple);
                    paragraph.FontSize = 18;

                    document.Blocks.Add(paragraph);
                }
            }
            this.SearchInfo.Document = document;

            this.StatusText.Text = new string("查找完成");
        }
    }
}
