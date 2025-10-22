using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;

namespace WPFGameEngine.Editor.Controls.Validators
{
    public class DoubleValueRangeValidator : ValidationRule
    {
        public double Max { get; set; }
        public double Min { get; set; }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double v = 0;
            string str = value.ToString();
            if (str.Contains("."))
                str = str.Replace(".", ",");

            if (!double.TryParse(str, out v))
            {
                return new ValidationResult(false, "Not a number!");
            }
            else if (v < Min)
            {
                return new ValidationResult(false, "Not in Bounds of Min!");
            }
            else if (v > Max)
            {
                return new ValidationResult(false, "Not in Bounds of Max!");
            }
            else
            {
                return new ValidationResult(true, "");
            }

        }
    }
}
