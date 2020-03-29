using JsonReaderDima.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Messages;

namespace JsonReaderDima.Views
{
    /// <summary>
    /// Interaction logic for EditPage1.xaml
    /// </summary>
    public partial class EditPage1 : Page
    {

        public MainWindowVM viewModel;

        DataStore e;


        MainWindow main => (App.Current as App).MainWindow as MainWindow;

        public EditPage1()
        {
            InitializeComponent();

            DataContext = viewModel = new MainWindowVM();

            UpdateFromStore();

#if DEBUG1
            ExecutionPlan.Delay(300, () =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        OpenCategoriesPageButton_Click(null, null);
                    });

                }); 
#endif

            e.OnPostsUpdate += E_OnPostsUpdate;
        }

        ~EditPage1()
        {
            e.OnPostsUpdate -= E_OnPostsUpdate;
        }

        private void UpdateFromStore()
        {
            e = DependencyService.GetInstance<DataStore>();

            viewModel.Posts = e.Posts.ToObservableCollection();


            int ID = Settings1.Default.SelectedItemId;

            if (ID > -1 && viewModel.Posts.Count(s => s != null && s.Id == ID) > 0)
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
            main.OpenFile(filename);
            
        }

        private void E_OnPostsUpdate()
        {
            viewModel.Posts = e.Posts.ToObservableCollection();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs _e)
        {
            string search = searchBox1.Text.ToLower();

        }

        private void OnSaveClick(object sender, RoutedEventArgs _e)
        {
            DataStore e = DependencyService.GetInstance<DataStore>();

            if (viewModel.SelectedPost != null)
            {
                e.UpdatePost(viewModel.SelectedPost);
            }


            e.SaveFile();


            Notifier notifier = DependencyService.GetInstance<Notifier>();
            notifier.ShowSuccess($"saved\n {e.FileName} ");
        }
        

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedPost != null)
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

        private void OpenCategoriesPageButton_Click(object sender, RoutedEventArgs e)
        {
            ((App.Current as App).MainWindow as MainWindow).frame1.Navigate(new CategoryEdit1());
        }

        private void ButtonListItemRemove_Click(object sender, RoutedEventArgs _e)
        {
            Post item = (sender as Button).DataContext as Post;

            //viewModel.Posts = viewModel.Posts.Where(s => s.Id != item.Id).ToObservableCollection();

            e.RemovePost(item.Id);
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs _e)
        {
            string dir = System.IO.Path.GetDirectoryName(e.FileName);
            Process.Start("explorer.exe", dir);
        }
    }
}
