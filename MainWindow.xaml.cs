using JsonReaderDima.ViewModels;
using JsonReaderDima.Views;
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
using System.Windows.Media.Animation;
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

        DataStore e;

        public MainWindow()
        {
            

            e = DependencyService.GetInstance<DataStore>();

            Loaded += MainWindow_Loaded;

            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFile(Settings1.Default.LastFileName);
        }


        public void OpenFile(string filename = "")
        {

            DataStore _e = new DataStore(filename);

            e.Posts = _e.Posts;
            e.FileName = _e.FileName;

            {
                //frame1.Content = new EditPage1();
                //if(frame1.CanGoBack) frame1.RemoveBackEntry();
                //var f = (frame1.Content as EditPage1);
                var f = (frame1.Content as EditPage1);
                f.UpdateFromStore();
            }


            Title = $"JSON Editor (mdimai666) - {filename}";
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

            //if (viewModel.SelectedPost != null)
            //{
            //    Settings1.Default.SelectedItemId = viewModel.SelectedPost.Id;
            //}

            Settings1.Default.Save();
        }

        public void OpenPage(object sender, EventArgs e)
        {
            frame1.Navigate(new EditPage1());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (frame1.CanGoBack) frame1.GoBack();
        }

        private void frame1_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var ta = new ThicknessAnimation();
            ta.Duration = TimeSpan.FromSeconds(0.3);
            ta.DecelerationRatio = 0.7;
            ta.To = new Thickness(0, 0, 0, 0);
            if (e.NavigationMode == NavigationMode.New)
            {
                ta.From = new Thickness(500, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ta.From = new Thickness(0, 0, 500, 0);
            }
            (e.Content as Page).BeginAnimation(MarginProperty, ta);
        }
    }
}
