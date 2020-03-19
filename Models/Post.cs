using JsonReaderDima.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JsonReaderDima
{
#pragma warning disable CA2235 // Mark all non-serializable fields
    [Serializable]
    public class Post : INotifyPropertyChanged
    {
        int row_id;
        int page;
        int id;

        string title;

        string author;
        string text;

        List<string> cats;


        [JsonProperty("id")]
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        [JsonProperty("title")]
        public string Title
        {
            get => title;
            set { title = value; OnPropertyChanged(); }
        }

        [JsonProperty("text")]
        public string Text
        {
            get => text;
            set { text = value; OnPropertyChanged(); }
        }

        [JsonProperty("author")]
        public string Author
        {
            get => author;
            set { author = value; OnPropertyChanged(); }
        }

        [JsonProperty("page")]
        public int Page
        {
            get => page;
            set { page = value; OnPropertyChanged(); }
        }


        [JsonProperty("row_id")]
        public int Row_id
        {
            get => row_id;
            set { row_id = value; OnPropertyChanged(); }
        }

        [JsonProperty("cats")]
        public List<string> Cats
        {
            get => cats;
            set { cats = value; OnPropertyChanged(); }
        }





        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class MockPost : Post
    {
        public MockPost()
        {
            Title = "Poem Title";
            Id = 101;
            Text = MainWindowVM.lorem;
            Author = "Макаров Дмитрий";
            Page = 2;
            Row_id = 50214;
            Cats = new List<string> { "cat1", "mycategory", "Zero-row" };


        }
    }

#pragma warning restore CA2235 // Mark all non-serializable fields
}
