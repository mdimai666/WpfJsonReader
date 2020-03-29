using JsonReaderDima.ViewModels;
using System;
using System.Collections.Generic;
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
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;


namespace JsonReaderDima.Views
{
    /// <summary>
    /// Interaction logic for CategoryEdit1.xaml
    /// </summary>
    public partial class CategoryEdit1 : Page
    {


        public CategoryEdit1VM viewModel;


        public CategoryEdit1()
        {
            InitializeComponent();

            DataContext = viewModel = new CategoryEdit1VM();


            viewModel.Cats = new System.Collections.ObjectModel.ObservableCollection<CatItemStat>(GetCatItems());

            viewModel.SelectedCat = viewModel.Cats.FirstOrDefault();
        }

        IEnumerable<CatItemStat> GetCatItems()
        {
            DataStore e = DependencyService.GetInstance<DataStore>();
            List<CatItemStat> list = new List<CatItemStat>();

            foreach (var item in e.Posts)
            {
                List<CatItemStat> q = item.Cats.Select(s => new CatItemStat { Value = s }).ToList();
                list.AddRange(q);
            }

            return list.GroupBy(s => s.Value).Select(g => new CatItemStat { Count = g.Count(), Value = g.First().Value }).OrderByDescending(s => s.Count).ToList();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            Notifier notifier = DependencyService.GetInstance<Notifier>();

            notifier.ShowSuccess("saved");

        }



        private void ButtonListItemRemove_Click(object sender, RoutedEventArgs e)
        {
            CatItemStat item = (sender as Button).DataContext as CatItemStat;


            if (viewModel.RemoveCat(item.Value))
            {
                AddLog($"removed item = {item.Value} ({item.Count})");
            }
        }

        void AddLog(string text)
        {
            viewModel.Logs.Add(text);
            listViewLogs1.SelectedIndex = listViewLogs1.Items.Count - 1;
            listViewLogs1.ScrollIntoView(listViewLogs1.SelectedItem);
        }

        private void FnButtonRemoveLessBy_Click(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(input_removeLessBy1.Text);

            if (viewModel.RemoveItemsLessBy(count))
            {
                AddLog($"remove tags less by = {count}");
            }

            
        }
    }

}
