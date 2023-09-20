using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Projekat.Validation
{
	public class StringToDoubleValidationRule : ValidationRule
	{

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
            try
            {
                var s = value as string;
                if (s.Contains(',')) {
                    return new ValidationResult(false, "Cifre treba da budu odvojene tačkom, a ne zapetom.");
                }
                double r;
                if (double.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Unesite validan oblik cene (cifre odvojene jednom tačkom).");
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška se desila.");
            }
        }
	}
}
