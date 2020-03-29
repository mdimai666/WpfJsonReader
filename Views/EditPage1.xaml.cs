using JsonReaderDima.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace JsonReaderDima.Views
{
    /// <summary>
    /// Interaction logic for EditPage1.xaml
    /// </summary>
    public partial class EditPage1 : Page
    {

        public MainWindowVM viewModel;


        public EditPage1()
        {
            InitializeComponent();

            DataContext = viewModel = new MainWindowVM();

            DataStore e = new DataStore();
            DependencyService.Register<DataStore>(e);

            OpenFile(Settings1.Default.LastFileName);

            int ID = Settings1.Default.SelectedItemId;

            if (ID > -1 && viewModel.Posts.Count(s => s != null && s.Id == ID) > 0)
            {
                viewModel.SelectedPost = viewModel.Posts.First(s => s.Id == ID);
            }
            else
            {
                if (e.Posts.Count > 0) viewModel.SelectedPost = viewModel.Posts.First();
            }

            ExecutionPlan.Delay(300, () =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OpenCategoriesPageButton_Click(null, null);
                });
                
            });
        }

        void OpenFile(string filename = "")
        {
            DataStore e = new DataStore(filename);

            DataStore de = DependencyService.GetInstance<DataStore>();
            de.Posts = e.Posts;


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
    }
}
