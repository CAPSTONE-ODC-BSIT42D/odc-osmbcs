using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucProductsCRUD.xaml
    /// </summary>
    public partial class ucProductsCRUD : UserControl
    {
        public ucProductsCRUD()
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

        private void generateProductCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(productNameTb.Text))
            {
                MessageBox.Show("For meaningful product code, enter product name first");
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string productCode = "";
                var random = new Random();

                for (int i = 0; i < 4; i++)
                {
                    productCode += chars[random.Next(chars.Length)];
                }
                productCode += "-";

                for (int i = 0; i < 4; i++)
                {
                    productCode += chars[random.Next(chars.Length)];
                }
                productCode += MainVM.ProductList.Count + 1;

                productCodeTb.Text = productCode;
            }
            else
            {
                if (productNameTb.Text.Length > 4)
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string productCode = "";
                    var random = new Random();

                    for (int i = 0; i < 4; i++)
                    {
                        productCode += chars[random.Next(chars.Length)];
                    }
                    productCode += "-";

                    productCode += productNameTb.Text.Trim().Substring(0, 4).ToUpper();
                    productCode += MainVM.ProductList.Count + 1;

                    productCodeTb.Text = productCode;
                }
                else
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string productCode = "";
                    var random = new Random();

                    for (int i = 0; i < 4; i++)
                    {
                        productCode += chars[random.Next(chars.Length)];
                    }
                    productCode += "-";

                    productCode += productNameTb.Text.Trim().ToUpper();
                    productCode += MainVM.ProductList.Count + 1;

                    productCodeTb.Text = productCode;
                }
            }
        }

        private void editProductBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in productDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    ((TextBox)element).IsEnabled = true;
                }
                if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                {
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled = true;
                }
                if (element is ComboBox)
                {
                    ((ComboBox)element).IsEnabled = true;
                }
            }
            saveCancelGrid2.Visibility = Visibility.Visible;
            editCloseGrid2.Visibility = Visibility.Collapsed;
            MainVM.isEdit = true;
        }

        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                if (productDetailsFormGrid.IsVisible)
                {
                    foreach (var element in productDetailsFormGrid1.Children)
                    {
                        if (element is TextBox)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);

                            if (expression != null)
                            {
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((TextBox)element);
                            }
                        }
                        if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                        {
                            BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.ValueProperty);
                            Validation.ClearInvalid(expression);
                            if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                            {
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element);
                            }
                        }
                        if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            if (expression != null)
                            {
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((ComboBox)element);
                            }

                        }
                    }
                }
                if (!validationError)
                {
                    saveDataToDb();
                    MainVM.isEdit = false;
                    OnSaveCloseButtonClicked(e);
                }
                    

                
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }

        private void cancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                resetFieldsValue();
                OnSaveCloseButtonClicked(e);
            }
            else if (result == MessageBoxResult.No)
            {
            }

        }

        private void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            if (productDetailsFormGrid.IsVisible)
            {
                using (MySqlConnection conn = dbCon.Connection)
                {

                    conn.Open();
                    MySqlCommand cmd = null;
                    if (!MainVM.isEdit)
                    {
                        cmd = new MySqlCommand("INSERT_ITEM", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        cmd = new MySqlCommand("UPDATE_ITEM", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@itemNo", MainVM.SelectedProduct.ItemCode);
                        cmd.Parameters["@itemNo"].Direction = ParameterDirection.Input;
                        MainVM.isEdit = false;
                    }

                    //INSERT NEW Product TO DB;

                    cmd.Parameters.AddWithValue("@itemCode", productCodeTb.Text);
                    cmd.Parameters["@itemCode"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@itemName", productNameTb.Text);
                    cmd.Parameters["@itemName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@itemDesc", productDescTb.Text);
                    cmd.Parameters["@itemDesc"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@costPrice", costPriceTb.Value);
                    cmd.Parameters["@costPrice"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@itemUnit", unitTb.Text);
                    cmd.Parameters["@itemUnit"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@typeID", categoryCb.SelectedValue);
                    cmd.Parameters["@typeID"].Direction = ParameterDirection.Input;

                    if (supplierCb.SelectedIndex == 0)
                    {
                        cmd.Parameters.AddWithValue("@supplierID", DBNull.Value);
                        cmd.Parameters["@supplierID"].Direction = ParameterDirection.Input;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@supplierID", supplierCb.SelectedValue);
                        cmd.Parameters["@supplierID"].Direction = ParameterDirection.Input;
                    }
                    cmd.ExecuteNonQuery();
                }
                resetFieldsValue();

            }
        }

        private void resetFieldsValue()
        {
            foreach (var element in productDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    Validation.ClearInvalid(expression);
                }
                else if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                {
                    BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.ValueProperty);
                    Validation.ClearInvalid(expression);
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).Value = 0;
                }
                else if (element is ComboBox)
                {
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
            }
        }

        private void loadDataToUi()
        {
            productNameTb.Text = MainVM.SelectedProduct.ItemName;
            productDescTb.Text = MainVM.SelectedProduct.ItemDesc;
            categoryCb.SelectedValue = MainVM.SelectedProduct.TypeID;
            costPriceTb.Value = MainVM.SelectedProduct.CostPrice;
            unitTb.Text = MainVM.SelectedProduct.Unit;
            supplierCb.SelectedValue = MainVM.SelectedProduct.SupplierID;
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainVM.isEdit && this.IsVisible)
            {
                loadDataToUi();
            }
        }
    }
}
