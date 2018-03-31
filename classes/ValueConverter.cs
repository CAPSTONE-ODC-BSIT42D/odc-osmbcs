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


    public class EmployeeNameConcatenator : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
            {
                MainVM.SelectedEmployeeContractor = MainVM.Employees.Where(x => x.EmpID.ToString().Equals(value.ToString())).FirstOrDefault();
                if(MainVM.SelectedEmployeeContractor == null)
                    MainVM.SelectedEmployeeContractor = MainVM.Contractor.Where(x => x.EmpID.ToString().Equals(value.ToString())).FirstOrDefault();
                return MainVM.SelectedEmployeeContractor.EmpFname + " " + MainVM.SelectedEmployeeContractor.EmpMiddleInitial + " " + MainVM.SelectedEmployeeContractor.EmpLName;
            }
            return "";
            return null;
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

    public class PhaseItemConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
            {
                MainVM.SelectedPhase = MainVM.Phases.Where(x => x.PhaseID == int.Parse(value.ToString())).First();
                return MainVM.SelectedPhase.PhaseName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    //public class PhaseGroupConverter : IValueConverter
    //{
    //    MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
    //    public object Convert(object value, Type targetType, object parameter,
    //            System.Globalization.CultureInfo culture)
    //    {
    //        if (!value.Equals(0))
    //        {
    //            MainVM.SelectedPhase = MainVM.Phases.Where(x => x.PhaseID == int.Parse(value.ToString())).First();
    //            MainVM.SelectedPhaseGroup = MainVM.PhasesGroup.Where(x => x.PhaseGroupID == MainVM.SelectedPhase.PhaseGroupID).First();
    //            return MainVM.SelectedPhaseGroup.PhaseGroupName;
    //        }
    //        return null;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter,
    //            System.Globalization.CultureInfo culture)
    //    {
    //        return "no";
    //    }
    //}


    public class SaleQuoteDetailsConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
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

    public class InvoicePriceConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
            {
                MainVM.VatableSale = 0;
                MainVM.TotalSalesWithOutDp = 0;
                
                MainVM.SelectedSalesQuote = MainVM.SalesQuotes.Where(x => x.sqNoChar_.Equals(value.ToString())).FirstOrDefault();
                var invoiceprod = from ai in MainVM.AvailedItems
                                  where ai.SqNoChar.Equals(value.ToString())
                                  select ai;
                var invoiceserv = from aser in MainVM.AvailedServices
                                  where aser.SqNoChar.Equals(value.ToString())
                                  select aser;
                foreach (AvailedItem ai in invoiceprod)
                {
                    var markupPrice = from itm in MainVM.MarkupHist
                                      where itm.ItemID == ai.ItemID
                                      && itm.DateEffective <= MainVM.SelectedSalesQuote.dateOfIssue_
                                      select itm;
                    decimal totalPric = ai.ItemQty * (ai.UnitPrice + (ai.UnitPrice / 100 * markupPrice.Last().MarkupPerc));
                    MainVM.VatableSale += totalPric;
                }

                foreach (AvailedService aserv in invoiceserv)
                {
                    MainVM.SelectedProvince = (from prov in MainVM.Provinces
                                               where prov.ProvinceID == aserv.ProvinceID
                                               select prov).FirstOrDefault();
                    MainVM.SelectedRegion = (from rg in MainVM.Regions
                                             where rg.RegionID == MainVM.SelectedProvince.RegionID
                                             select rg).FirstOrDefault();

                    var service = from serv in MainVM.ServicesList
                                  where serv.ServiceID == aserv.ServiceID
                                  select serv;

                    decimal totalFee = (from af in aserv.AdditionalFees
                                        select af.FeePrice).Sum();
                    decimal totalAmount = aserv.TotalCost + totalFee;

                    MainVM.VatableSale += totalAmount;
                }

                MainVM.TotalSalesNoVat = Math.Round(MainVM.VatableSale, 2);

                MainVM.VatAmount = (MainVM.VatableSale * ((decimal)0.12));

                MainVM.TotalSales = MainVM.VatableSale + MainVM.VatAmount;
                return MainVM.TotalSales;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class ServiceSalesOrderIDConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
            {
                MainVM.SelectedAvailedServices = MainVM.AvailedServices.Where(x => x.AvailedServiceID == int.Parse(value.ToString())).FirstOrDefault();
                return MainVM.SelectedAvailedServices.SqNoChar;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }
    
    public class ServiceNameConverter2 : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
            {
                MainVM.SelectedAvailedServices = MainVM.AvailedServices.Where(x => x.AvailedServiceID == int.Parse(value.ToString())).FirstOrDefault();
                MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID == MainVM.SelectedAvailedServices.ServiceID).FirstOrDefault();
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

    public class ProductIDToNameConverter : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!value.Equals(0))
            {
                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID == int.Parse(value.ToString())).FirstOrDefault();
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

    public class CountServicesAssigned : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            int count = 0;
            MainVM.SelectedEmployeeContractor = MainVM.Employees.Where(x => x.EmpID.Equals(value)).FirstOrDefault();
            MainVM.SelectedEmployeeContractor = MainVM.Contractor.Where(x => x.EmpID.Equals(value)).FirstOrDefault();
            foreach (ServiceSchedule ss in MainVM.ServiceSchedules_)
            {
                if (ss.assignedEmployees_.Contains(MainVM.SelectedEmployeeContractor))
                {
                    count++;
                }
            }
            return count;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }


    public class DisableActionButtonPurchaseOrder : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!MainVM.isNewPurchaseOrder)
            {
                if (MainVM.isEditPurchaseOrder && !MainVM.isViewPurchaseOrder)
                    return true;
                else if (!MainVM.isEditPurchaseOrder && MainVM.isViewPurchaseOrder)
                    return false;
                else
                    return null;
            }
            else
                return true;
             
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }

    public class DisableActionButtonServiceSchedule : IValueConverter
    {
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (!MainVM.isNewSchedule)
            {
                if (MainVM.isEditSchedule && !MainVM.isViewSchedule)
                    return true;
                else if (!MainVM.isEditSchedule && MainVM.isViewSchedule)
                    return false;
                else
                    return null;
            }
            else
                return true;

        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }
}
