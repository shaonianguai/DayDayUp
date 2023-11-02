using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileParallelQuery ftp = new FileParallelQuery();//并行查询类
        Stopwatch sw = new Stopwatch();//计时器

        public MainWindow()
        {
            InitializeComponent();
            SearchTextBox.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                sw.Start();
                //读取USN日志
                Task.Factory.StartNew((Action)delegate ()
                {
                    var completeNames = ftp.InitCollectionFromDisks();
                    foreach (string name in completeNames)
                    {

                    }
                    sw.Stop();
                }).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        SearchTextBox.IsEnabled = true;

                    }), DispatcherPriority.Normal);
                });
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileListBox.Items.Clear();
            List<string> extensions = new List<string>(5);
            extensions.Add("cs");

            DataSource.TemporaryList1 = ftp.SearchMatchFileMethod("", "Button", extensions).AsParallel().ToList<FileModel>();
            foreach(var item in DataSource.TemporaryList1)
            {
                FileListBox.Items.Add(item.FilePath);
            }
        }
    }
}
