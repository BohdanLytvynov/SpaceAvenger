using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            int length = str.Length;
            if (str[length - 1].Equals(','))
                str += "0";

            if (!double.TryParse(str, out v))
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
