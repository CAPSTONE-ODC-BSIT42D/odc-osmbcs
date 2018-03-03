using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void productRbtn_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {

                saveCancelGrid3.Visibility = Visibility.Collapsed;
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
                saveCancelGrid3.Visibility = Visibility.Visible;
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
                            serviceProvinceCb.SelectedValue = MainVM.SelectedCustomerSupplier.CompanyProvinceID;
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

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addNewItemFormGrid.IsVisible)
            {
                var linqResults = MainVM.ProductList.Where(x => x.ItemName.ToLower().Contains(searchTb.Text.ToLower()));
                var observable = new ObservableCollection<Item>(linqResults);
                addGridProductListDg.ItemsSource = observable;
            }

        }


        private void serviceProvinceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceProvinceCb.SelectedIndex != -1)
            {
                //MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == int.Parse(serviceProvinceCb.SelectedValue.ToString())).First();
                //if (MainVM.SelectedProvince.ProvincePrice == 0)
                //{
                //    MessageBox.Show("This location has no price set. Please set it in Settings.");
                //}
            }

        }   

        private void selectProductBtn_Click(object sender, RoutedEventArgs e)
        {
            object linqResults = null;
            foreach (Item prd in MainVM.ProductList)
            {
                linqResults = MainVM.RequestedItems.Where(x => x.itemID.Equals(prd.ID)).FirstOrDefault();
               
            }
            if (linqResults == null)
            {
                MainVM.RequestedItems.Add(new RequestedItem() { lineNo = MainVM.RequestedItems.Count + 1, itemID = MainVM.SelectedProduct.ID, itemType = 0, qty = 1,unitPrice = 0, qtyEditable = true });
                OnSaveCloseButtonClicked(e);
            }
            else
            {
                MessageBox.Show("Already added in the list.");
            }
        }
        //---- Service Part
        private void addProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)serviceRbtn.IsChecked)
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
                    MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID == int.Parse(serviceTypeCb.SelectedValue.ToString())).First();
                    

                    MainVM.SelectedRegion = MainVM.Regions.Where(x => x.Provinces.Contains(MainVM.SelectedProvince)).FirstOrDefault();

                    var newService = (new AvailedService() {AvailedServiceID = MainVM.AvailedServices.Count+1, ServiceID = MainVM.SelectedService.ServiceID, ProvinceID = int.Parse(serviceProvinceCb.SelectedValue.ToString()), Address = serviceAddressTb.Text, City = serviceCityTb.Text, TotalCost = MainVM.SelectedService.ServicePrice +  MainVM.SelectedRegion.RatePrice});
                    MainVM.AvailedServices.Add(newService);
                    MainVM.RequestedItems.Add(new RequestedItem() { lineNo = newService.AvailedServiceID, itemID = MainVM.SelectedService.ServiceID, itemType = 1, qty = 1, unitPrice = newService.TotalCost, qtyEditable = false });
                    OnSaveCloseButtonClicked(e);
                }

            }
        }

        private void cancelAddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        void resetFields()
        {
            foreach (var element in addNewServiceForm.Children)
            {
                if (element is TextBox)
                {
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is ComboBox)
                {
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
                else if (element is CheckBox)
                {
                    ((CheckBox)element).IsChecked = false;
                }
            }
        }
        
    }
}
