using System;
using System.Globalization;
using Xamarin.Forms;

namespace GithubRepoExample.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = value as DateTimeOffset?;
            var imageName = string.Empty;

            if (date != null)
            {
                return date.Value.Date.ToShortDateString();
            }

            return "Created Date not known";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}

