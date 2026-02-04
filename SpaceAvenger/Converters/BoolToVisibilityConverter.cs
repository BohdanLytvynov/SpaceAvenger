using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SpaceAvenger.Converters
{
    /// <summary>
    /// Converts Boolean to Visibility. True -> Visible, False -> Hidden / Collapsed, depends on parameter. 
    /// If parameter = true -> Visibility.Hidden, if false -> Visibility.Collapsed
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Bool to Visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hiddenCollapsed = (bool)parameter;
            bool v = (bool)value;
            if (v) 
                return Visibility.Visible;
            else
            { 
                if(hiddenCollapsed)
                    return Visibility.Hidden;
                else 
                    return Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Visibility to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            switch (visibility)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                    return false;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
