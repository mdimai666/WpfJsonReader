using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace JsonReaderDima.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public const string lorem = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";





        Post _post;

        public Post SelectedPost
        {
            get => _post; set
            {
                _post = value;
                OnPropertyChanged(nameof(SelectedPost));
                OnPropertyChanged(nameof(CurrentSelectIndexNum));
            }
        }

        ObservableCollection<Post> _posts { get; set; }

        public ObservableCollection<Post> Posts { get => GetAsSearch(); set { _posts = value; OnPropertyChanged(nameof(Posts)); } }

        string _searchText = "";


        public int CurrentSelectIndexNum => (SelectedPost == null || _post == null) ? -1 : Posts.IndexOf(SelectedPost)+1;

        public string SearchText
        {
            get => _searchText; set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(Posts));
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

        public MainWindowVM()
        {
            Posts = new ObservableCollection<Post>(){
                new Post(){ Id=1, Title="Title1", Text="1 "+lorem, Author="Dima" },
                new Post(){ Id=2, Title="Big poeme", Text="2 "+lorem, Author="Kolya" },
                new Post(){ Id=3, Title="Enuma elish", Text="3 "+lorem,Author="vauya" },
            };

            SelectedPost = Posts.First();
        }

        public ObservableCollection<Post> GetAsSearch()
        {
            string search = _searchText.ToLower();

            if (string.IsNullOrEmpty(_searchText.Trim()))
            {
                return _posts;
            }
            else
            {
                return _posts.Where(post =>
                {
                    return
                        (post.Title.ToLower().Contains(search))
                        || (post.Id.ToString().Contains(search))
                        || (post.Text.ToLower().Contains(search))
                        || (post.Author.ToLower().Contains(search))
                        ;
                }).AsObservableCollection();
            }
        }
    }

    public static partial class Ext
    {
        public static ObservableCollection<T> AsObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }
    }
}
