﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Projekat.Validation
{
	
    public class EmptyStringValidationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text1 = values[0] as string;
            Console.Write(text1);
            if (string.IsNullOrEmpty(text1)) return false;

            string text2 = values[1] as string;
            if (string.IsNullOrEmpty(text2)) return false;

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
