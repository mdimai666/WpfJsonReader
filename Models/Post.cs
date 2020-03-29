using JsonReaderDima.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace JsonReaderDima
{
#pragma warning disable CA2235 // Mark all non-serializable fields
    [Serializable]
    public class CatItemStat : INotifyPropertyChanged
    {
        //int id;

        string title;

        int count;
        string value;

        ObservableCollection<string> cats = new ObservableCollection<string>();



        [JsonProperty("value")]
        public string Value
        {
            get => value;
            set { this.value = value; OnPropertyChanged(); }
        }

        [JsonProperty("count")]
        public int Count
        {
            get => count;
            set { this.count = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

    public class MockCatItemStat : CatItemStat
    {
        public MockCatItemStat()
        {
            Value = "Cat Title";
            Count = 2;


        }
    }

#pragma warning restore CA2235 // Mark all non-serializable fields
}
