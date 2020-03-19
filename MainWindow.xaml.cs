using JsonReaderDima.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonReaderDima
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataStore e;

        public MainWindowVM viewModel;



        public MainWindow()
        {
            InitializeComponent();

            DataContext = viewModel = new MainWindowVM();



            OpenFile(Settings1.Default.LastFileName);

            int ID = Settings1.Default.SelectedItemId;

            if (ID > -1 && viewModel.Posts.Count(s => s!= null && s.Id == ID) > 0)
            {
                viewModel.SelectedPost = viewModel.Posts.First(s => s.Id == ID);
            }
            else
            {
                if (e.Posts.Count > 0) viewModel.SelectedPost = viewModel.Posts.First();
            }


        }

        void OpenFile(string filename = "")
        {
            e = new DataStore(filename);

            viewModel.Posts = new ObservableCollection<Post>(e.Posts);

            Title = $"JSON Editor (mdimai666) - {filename}";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = searchBox1.Text.ToLower();


        }
        
        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            


        }

        private void Button_clearSearch(object sender, RoutedEventArgs e)
        {
            searchBox1.Text = "";
        }

        private void MainWindowRoot_SourceInitialized(object sender, EventArgs e)
        {


            this.Top = Settings1.Default.Top;
            this.Left = Settings1.Default.Left;
            this.Height = Settings1.Default.Height;
            this.Width = Settings1.Default.Width;
            // Very quick and dirty - but it does the job
            if (Settings1.Default.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void MainWindowRoot_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Settings1.Default.Top = RestoreBounds.Top;
                Settings1.Default.Left = RestoreBounds.Left;
                Settings1.Default.Height = RestoreBounds.Height;
                Settings1.Default.Width = RestoreBounds.Width;
                Settings1.Default.Maximized = true;
            }
            else
            {
                Settings1.Default.Top = this.Top;
                Settings1.Default.Left = this.Left;
                Settings1.Default.Height = this.Height;
                Settings1.Default.Width = this.Width;
                Settings1.Default.Maximized = false;
            }

            if (viewModel.SelectedPost != null)
            {
                Settings1.Default.SelectedItemId = viewModel.SelectedPost.Id;
            }

            Settings1.Default.Save();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.SelectedPost != null)
            {
                int ID = viewModel.SelectedPost.Id;
                string link = $"https://rustih.ru/?p={ID}";
                var psi = new ProcessStartInfo
                {
                    FileName = link,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;

                Settings1.Default.LastFileName = filename;
                OpenFile(filename);
            }
        }

        private void ButtonDetailRemove_Click(object sender, RoutedEventArgs e)
        {
            string data = (sender as Button).DataContext as string;
            Debug.WriteLine(data);
            viewModel.SelectedPost.Cats.Remove(data);
            viewModel.OnPropertyChanged(nameof(viewModel.Posts));
        }
    }
}
