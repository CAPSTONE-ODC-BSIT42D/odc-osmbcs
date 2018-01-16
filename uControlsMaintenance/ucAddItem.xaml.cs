using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prototype2.uControlsMaintenance
{
    /// <summary>
    /// Interaction logic for ucAddItem.xaml
    /// </summary>
    public partial class ucAddItem : UserControl
    {
        public ucAddItem()
        {
            InitializeComponent();
        }

        private bool validationError = false;
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        private void productRbtn_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                for (int x = 1; x < forms.Children.Count; x++)
                {
                    forms.Children[x].Visibility = Visibility.Hidden;
                }
                product.Visibility = Visibility.Visible;
                addNewServiceForm.Visibility = Visibility.Hidden;
            }

        }

        private void productRbtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void serviceRbtn_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                for (int x = 1; x < forms.Children.Count; x++)
                {
                    forms.Children[x].Visibility = Visibility.Hidden;
                }
                product.Visibility = Visibility.Hidden;
                addNewServiceForm.Visibility = Visibility.Visible;
            }

        }

        private void serviceRbtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void addressOfCustomerCb_Checked(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedCustomerSupplier != null)
            {
                foreach (var element in addNewServiceForm.Children)
                {
                    if (element is TextBox)
                    {
                        if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                        {
                            serviceAddressTb.Text = MainVM.SelectedCustomerSupplier.CompanyAddress;
                            serviceAddressTb.IsEnabled = false;
                        }
                        if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                        {
                            serviceCityTb.Text = MainVM.SelectedCustomerSupplier.CompanyCity;
                            serviceCityTb.IsEnabled = false;
                        }

                    }
                    if (element is ComboBox)
                    {
                        if (((ComboBox)element).Name.Equals(serviceProvinceCb.Name))
                        {
                            serviceProvinceCb.SelectedIndex = int.Parse(MainVM.SelectedCustomerSupplier.CompanyProvinceID) - 1;
                            serviceProvinceCb.IsEnabled = false;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Custoemr selected");
            }

        }

        private void addressOfCustomerCb_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var element in addNewServiceForm.Children)
            {
                if (element is TextBox)
                {
                    if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                    {
                        serviceAddressTb.Text = "";
                        serviceAddressTb.IsEnabled = true;
                    }
                    if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                    {
                        serviceCityTb.Text = "";
                        serviceCityTb.IsEnabled = true;
                    }

                }
                if (element is ComboBox)
                {
                    if (((ComboBox)element).Name.Equals(serviceProvinceCb.Name))
                    {
                        serviceProvinceCb.SelectedIndex = -1;
                        serviceProvinceCb.IsEnabled = true;
                    }
                }
            }
        }

        private void selectProductBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Item prd in MainVM.ProductList)
            {
                var linqResults = MainVM.RequestedItems.Where(x => x.itemID.Equals(prd.ID)).FirstOrDefault();
                if (linqResults == null)
                {
                    MainVM.RequestedItems.Add(new RequestedItem() { lineNo = MainVM.RequestedItems.Count + 1, itemID = prd.ID, itemType = 0, unitPrice = 0, qtyEditable = true });
                }
                else
                {
                    MessageBox.Show("Already added in the list.");
                }
            }
        }

        private void addProductBtn_Click(object sender, RoutedEventArgs e)
        {

            //Add Item In to List
            if ((bool)productRbtn.IsChecked)
            {
                
            }
            else if ((bool)serviceRbtn.IsChecked)
            {
                foreach (var element in addNewServiceForm.Children)
                {
                    if (element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        if (expression != null)
                        {
                            Validation.ClearInvalid(expression);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((TextBox)element);
                        }


                    }
                    if (element is ComboBox)
                    {
                        BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                        Validation.ClearInvalid(expression);
                        expression.UpdateSource();
                        validationError = Validation.GetHasError((ComboBox)element);
                    }
                }
                if (!validationError)
                {
                    MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(serviceTypeCb.SelectedValue.ToString())).First();
                    MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == int.Parse(serviceProvinceCb.SelectedValue.ToString())).First();

                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    var random = new Random();
                    string serviceNoChar = "";
                    for (int i = 0; i < 4; i++)
                    {
                        if (!(i > 3))
                        {
                            serviceNoChar += chars[random.Next(chars.Length)];
                        }
                    }
                    serviceNoChar += MainVM.SalesQuotes.Count + 1;
                    serviceNoChar += "-";
                    if (MainVM.SelectedService.ServiceName.Length > 5)
                    {
                        serviceNoChar += MainVM.SelectedService.ServiceName.Trim().Substring(0, 5).ToUpper();
                    }
                    else
                        serviceNoChar += MainVM.SelectedService.ServiceName.Trim().ToUpper();
                    serviceNoChar += "-";
                    serviceNoChar += DateTime.Now.ToString("yyyy-MM-dd");


                    //MainVM.AddedServices.Add(new AddedService() { TableNoChar = serviceNoChar, ServiceID = MainVM.SelectedService.ServiceID, ProvinceID = MainVM.SelectedProvince.ProvinceID, Address = serviceAddressTb.Text, City = serviceCityTb.Text, TotalCost = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice });

                    //MainVM.RequestedItems.Add(new RequestedItem() { lineNo = (MainVM.RequestedItems.Count + 1).ToString(), itemCode = serviceNoChar, itemName = MainVM.SelectedService.ServiceName, desc = serviceDescTb.Text, itemTypeName = "Service", itemType = 1, qty = 1, unitPrice = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, totalAmount = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, totalAmountMarkUp = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, qtyEditable = false });
                }

            }
        }

        
    }
}
