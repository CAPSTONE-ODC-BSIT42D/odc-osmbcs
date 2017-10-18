using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace prototype2
{
    public class IdToNameConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!String.IsNullOrWhiteSpace(value.ToString()))
            {
                MainVM.SelectedCustomerSupplier = MainVM.AllCustomerSupplier.Where(x => x.CompanyID.Equals(value.ToString())).First();
                return MainVM.SelectedCustomerSupplier.CompanyName;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class EmployeeTypeOfConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!String.IsNullOrWhiteSpace(value.ToString()))
            {
                if (value.ToString().Equals("0"))
                {
                    return true;
                }
                else
                    return false;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }
}
