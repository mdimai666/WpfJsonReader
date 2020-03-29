using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace JsonReaderDima.ViewModels
{
    public class CategoryEdit1VM : INotifyPropertyChanged
    {
        public const string lorem = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";





        CatItemStat _cat;

        public CatItemStat SelectedCat
        {
            get => _cat; set
            {
                _cat = value;
                OnPropertyChanged(nameof(SelectedCat));
                OnPropertyChanged(nameof(CurrentSelectIndexNum));
            }
        }

        ObservableCollection<CatItemStat> _cats { get; set; }

        public ObservableCollection<CatItemStat> Cats
        {
            get => GetAsSearch();
            set { _cats = value; OnPropertyChanged(nameof(Cats)); }
        }

        string _searchText = "";


        public int CurrentSelectIndexNum => (SelectedCat == null || _cat == null) ? -1 : Cats.IndexOf(SelectedCat) + 1;

        public string SearchText
        {
            get => _searchText; set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(Cats));
            }
        }

        bool _itemIsDirty = false;

        public bool ItemIsDirty
        {
            get => _itemIsDirty; set
            {
                _itemIsDirty = value;
                OnPropertyChanged();
            }
        }

        //public string FileName => Settings1.Default.LastFileName;



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //------------------------------------

        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string> { "Logs will here" };


        //------------------------------------

        public CategoryEdit1VM()
        {

            Cats = new ObservableCollection<CatItemStat>(){
                new CatItemStat(){ Value="Cat 1", Count=1},
                new CatItemStat(){ Value="Cat 2 view", Count=2},
                new CatItemStat(){ Value="Cat 3 long", Count=8},
            };

            SelectedCat = Cats.First();
        }

        

        public ObservableCollection<CatItemStat> GetAsSearch()
        {
            string search = _searchText.ToLower();

            if (string.IsNullOrEmpty(_searchText.Trim()))
            {
                return _cats;
            }
            else
            {
                return _cats.Where(post =>
                {
                    return
                        (post.Value.ToLower().Contains(search))
                        ;
                }).ToObservableCollection();
            }
        }

        public bool RemoveCat(string value)
        {
            CatItemStat _item = _cats.FirstOrDefault(s => s.Value == value);
            bool e = _cats.Remove(_item);
            OnPropertyChanged(nameof(Cats));
            ItemIsDirty = true;
            return e;
        }


        public bool RemoveItemsLessBy(int count)
        {
            var items = _cats.Where(s => s.Count >= count);
            bool e = items.Count() > 0;
            _cats = new ObservableCollection<CatItemStat>(items);
            OnPropertyChanged(nameof(Cats));
            ItemIsDirty = true;
            return e;
        }
    }

    public class LogsMockDesinger : ObservableCollection<string>
    {
        public LogsMockDesinger() : base(new string[] { "Logs will here", "row2" })
        {

        }
    }

}
