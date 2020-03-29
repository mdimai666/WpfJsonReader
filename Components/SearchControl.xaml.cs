using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

namespace JsonReaderDima.Components
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl, INotifyPropertyChanged
    {

        //string _searchText = "";

        private static readonly DependencyProperty TextProperty
            = DependencyProperty.Register(nameof(Text), typeof(string), typeof(SearchControl), 
                new PropertyMetadata("") );

        public string Text
        {
            //get => _searchText; 
            //set {
            //    _searchText = value;
            //    OnPropertyChanged(nameof(Text));
            //}

            get { return (string)GetValue(TextProperty); }
            set { 
                SetValue(TextProperty, value);
                //OnPropertyChanged(nameof(Text));
            }
        }

        public event TextChangedEventHandler OnTextChanged;


        public SearchControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private void Button_clearSearch(object sender, RoutedEventArgs e)
        {
            searchBox1.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged?.Invoke(sender, e);


        }
    }
}
