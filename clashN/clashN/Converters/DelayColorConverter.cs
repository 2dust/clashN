using System.Windows.Data;
using System.Windows.Media;

namespace ClashN.Converters
{
    public class DelayColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var temp = (int)value;

            if (temp <= 200)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.IndianRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}