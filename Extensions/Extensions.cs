using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JsonReaderDima.ViewModels
{
    public static partial class Extensions
    {
        public static ObservableCollection<T> AsObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }
    }
}
