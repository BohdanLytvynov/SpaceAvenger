using System;
using System.Globalization;
using System.Windows.Data;

namespace SpaceAvenger.Converters
{
    /// <summary>
    /// Converts Female - icon -> True, Male - icon -> False
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToMaleFemaleInvertedConverter : IValueConverter
    {
        /// <summary>
        /// Convert Bool to Male / Female
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool maleFemale = (bool)value;

            switch (maleFemale)
            {
                case true:
                    return "Female";
                default:
                    return "Male";
            }
        }
        /// <summary>
        /// Convert Male / Female to Bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return "Fail to Convert to male/female";
            }

            if (str.Equals("Female"))
            {
                return true;
            }
            else if (str.Equals("Male"))
            {
                return false;
            }

            return "Fail to convert!";
        }
    }

}
