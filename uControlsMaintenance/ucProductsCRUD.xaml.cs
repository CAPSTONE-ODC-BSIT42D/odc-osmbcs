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
                foreach (var element in productDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        if (((TextBox)element).IsVisible)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                            if (expression != null)
                            {
                                if (((TextBox)element).IsEnabled)
                                {
                                    expression.UpdateSource();
                                    if (Validation.GetHasError((TextBox)element))
                                        validationError = true;
                                }
                            }
                        }
                    }
                    if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                    {
                        BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.TextProperty);
                        Validation.ClearInvalid(expression);
                        if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                        {
                            expression.UpdateSource();
                            if (Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element))
                                validationError = true;
                        }
                    }
                    if (element is ComboBox)
                    {
                        BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                        if (expression != null)
                        {
                            expression.UpdateSource();
                            if (Validation.GetHasError((ComboBox)element))
                                validationError = true;
                        }

                    }
                }
                if (!validationError)
                {
                    saveDataToDb();
                    MainVM.isEdit = false;
                    OnSaveCloseButtonClicked(e);
                }
                else
                {
                    MessageBox.Show("Resolve the error first");
                    validationError = false;
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
            string unitID = "null";
            if (unitCb.SelectedValue != null)
                unitID = unitCb.SelectedValue.ToString();

            string typeID = "null";
            if(categoryCb.SelectedValue != null)
                typeID = categoryCb.SelectedValue.ToString();
            
            string supplierID = "null";
            if(supplierCb.SelectedValue != null)
                supplierID = supplierCb.SelectedValue.ToString();
            if (dbCon.IsConnect())
            {
                if (MainVM.isEdit)
                {
                    string query = "UPDATE item_t SET " +
                        "itemName = " + productNameTb.Text+", "+
                        "itemDescr= " + productDescTb.Text + ", " +
                        "markUpPerc = " + markUpPriceTb.Text + ", " +
                        "unitID = " + unitID+ ", " +
                        "typeID = " + typeID + ", " +
                        "supplierID = " + supplierID + ", " +
                        " dateEffective = '" + dateEffective.SelectedDate.Value.ToString("yyyy-MM-dd") + "', " +
                        "WHERE id = " +MainVM.SelectedProduct.ID;

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Record is Saved");

                    }
                }
                else
                {
                    string query = "INSERT INTO item_t (itemName,itemDescr,markUpPerc,unitID,typeID,supplierID, dateEffective) VALUES ('" +
                    productNameTb.Text + "','" +
                    productDescTb.Text + "'," +
                    markUpPriceTb.Value + "," +
                    unitID + "," +
                    typeID + "," +
                    supplierID+ ",'" +
                    dateEffective.SelectedDate.Value.ToString("yyyy-MM-dd") +
                    "');";

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Record is Saved");

                    }
                }
                
            }
            resetFieldsValue();
        }

        private void resetFieldsValue()
        {
            foreach (var element in productDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                {
                    BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.ValueProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).Value = 0;
                }
                else if (element is ComboBox)
                {
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
            }
            MainVM.isEdit = false;
        }

        private void loadDataToUi()
        {
            productNameTb.Text = MainVM.SelectedProduct.ItemName;
            productDescTb.Text = MainVM.SelectedProduct.ItemDesc;
            categoryCb.SelectedValue = MainVM.SelectedProduct.TypeID;
            markUpPriceTb.Value = MainVM.SelectedProduct.MarkUpPerc;
            unitCb.SelectedValue = MainVM.SelectedProduct.UnitID;
            supplierCb.SelectedValue = MainVM.SelectedProduct.SupplierID;
            dateEffective.SelectedDate = MainVM.SelectedProduct.DateEffective;
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

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            if (categoryNameTb.IsVisible)
            {
                categoryNameTb.Visibility = Visibility.Collapsed;
                addCategoryBtn.Content = "+";
                string query = "INSERT INTO `odc_db`.`item_type_t` (`typeName`) VALUES('" + categoryNameTb.Text + "')";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    {
                        MessageBox.Show("Item Category successfully added");
                        categoryNameTb.Clear();
                        MainVM.Ldt.worker.RunWorkerAsync();
                        dbCon.Close();
                    }
                }
            }
            else
            {
                categoryNameTb.Visibility = Visibility.Visible;
                addCategoryBtn.Content = "Save";
            }

        }
        private void addUnitBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            if (unitNameTb.IsVisible)
            {
                unitNameTb.Visibility = Visibility.Collapsed;
                addUnitBtn.Content = "+";
                string query = "INSERT INTO `odc_db`.`unit_t` (`unitName`) VALUES('" + unitNameTb.Text + "')";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    {
                        MessageBox.Show("Unit successfully added");
                        unitNameTb.Clear();
                        MainVM.Ldt.worker.RunWorkerAsync();
                    }
                }
            }
            else
            {
                unitNameTb.Visibility = Visibility.Visible;
                addUnitBtn.Content = "Save";
            }

        }
        
    }
}
