using System.Globalization;
using System.Windows.Controls;

namespace WPFGameEngine.Editor.Controls.Validators
{
    internal class DoubleValueValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double v;

            string str = value.ToString();
            if (str.Contains("."))
                str = str.Replace(".", ",");

            int length = str?.Length ?? 0;
            if (length >0 && str[length - 1].Equals(','))
                str += "0";

            if (!double.TryParse(str, new CultureInfo("en-US"), out v))
            {
                return new ValidationResult(false, "Not a number!");
            }
            else
            {
                return new ValidationResult(true, "");
            }
        }
    }
}
