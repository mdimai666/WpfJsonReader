using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JsonReaderDima
{
    public static partial class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }
    }
}
