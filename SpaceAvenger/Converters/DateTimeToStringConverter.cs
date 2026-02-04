using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SpaceAvenger.Converters
{
    /// <summary>
    /// Converts Datetime to String and back
    /// </summary>
    [ValueConversion(typeof(DateTime), typeof(string))]
    internal class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts Datetime to ShortDateString
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;

            if (date.Equals(default))
                return (parameter as string)?.ToString() ?? "This is a default DateTime!";

            return date.ToShortDateString();
        }
        /// <summary>
        /// String to Datetime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date;

            if (!DateTime.TryParse(value.ToString(), out date))
                return DependencyProperty.UnsetValue;

            return date;
        }
    }
}
