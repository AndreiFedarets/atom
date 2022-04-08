using System;
using System.Globalization;
using System.Windows.Data;

namespace Atom.Client
{
    public sealed class SearchTextToSearchPopupOpenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string searchText = (string)value;
            return !string.IsNullOrEmpty(searchText);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
