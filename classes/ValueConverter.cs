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
            if (!value.Equals(0))
            {
                MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.ToString().Equals(value.ToString())).FirstOrDefault();
                if (MainVM.SelectedCustomerSupplier == null)
                    MainVM.SelectedCustomerSupplier = MainVM.Suppliers.Where(x => x.CompanyID.ToString().Equals(value.ToString())).FirstOrDefault();
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

    public class EmployeePositionConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.EmpPosition.Count != 0)
            {
                MainVM.SelectedEmpPosition = MainVM.EmpPosition.Where(x => x.PositionID.Equals(value)).First();
                return MainVM.SelectedEmpPosition.PositionName;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class ContractorJobConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.ContJobTitle.Count!=0)
            {
                MainVM.SelectedJobTitle = MainVM.ContJobTitle.Where(x => x.JobID.Equals(value)).First();
                return MainVM.SelectedJobTitle.JobName;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class ServiceTypeConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!String.IsNullOrWhiteSpace(value.ToString()))
            {
                MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(value.ToString())).First();
                return MainVM.SelectedService.ServiceName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class ProductNameConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.ProductList.Count != 0)
            {
                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID.Equals(value)).First();
                return MainVM.SelectedProduct.ItemName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class ProductMarkUpPercent : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.MarkupHist.Count != 0)
            {
                var obj = MainVM.MarkupHist.Where(x => x.ItemID.Equals(value)).LastOrDefault();
                return obj.MarkupPerc;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class CategoryNameConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.ProductCategory.Count != 0)
            {
                var obj = MainVM.ProductCategory.Where(x => x.TypeID.Equals(value)).First();
                return obj.TypeName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class ServiceNameConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.ServicesList.Count != 0)
            {
                MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(value)).First();
                return MainVM.SelectedService.ServiceName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class SaleQuoteDetailsConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0) && MainVM.ServicesList.Count != 0)
            {
                MainVM.SelectedSalesQuote = MainVM.SalesQuotes.Where(x => x.sqNoChar_.Equals(value)).First();
                MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(MainVM.SelectedSalesQuote.custID_)).First();
                return MainVM.SelectedCustomerSupplier.CompanyName;
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
