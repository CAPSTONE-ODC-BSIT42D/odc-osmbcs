using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ucLocation.xaml
    /// </summary>
    public partial class ucLocation : UserControl
    {
        public ucLocation()
        {
            InitializeComponent();
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        Region region = new Region();

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }
        
        private void addProvinceBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            if (MainVM.isEdit)
            {
                if (provinceNameTb.IsVisible)
                {
                    provinceNameTb.Visibility = Visibility.Collapsed;
                    provinceLbl.Visibility = Visibility.Collapsed;
                    if (MainVM.SelectedProvince != null)
                    {
                        string query = "UPDATE provinces_t SET " +
                            "provinceName = " + provinceNameTb.Text +
                            " WHERE id = " + MainVM.SelectedProvince.ProvinceID;
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record is Saved");
                            MainVM.Ldt.worker.RunWorkerAsync();
                            MainVM.Provinces = MainVM.SelectedRegion.Provinces;
                        }
                    }
                    else
                    {
                        string query = "INSERT INTO `odc_db`.`provinces_t` (`provinceName`,`regionID`) VALUES('" + provinceNameTb.Text + "','" + region.RegionID + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record is Saved");
                            MainVM.Ldt.worker.RunWorkerAsync();
                            MainVM.SelectedRegion = MainVM.Regions.Where(x => x.RegionID == region.RegionID).FirstOrDefault();
                            MainVM.Provinces = MainVM.SelectedRegion.Provinces;
                        }
                    }
                    addProvinceBtn.Content = "Add";
                }
                else
                {
                    provinceNameTb.Visibility = Visibility.Visible;
                    provinceLbl.Visibility = Visibility.Visible;
                    addProvinceBtn.Content = "Save";
                }
            }
            else
            {
                if (provinceNameTb.IsVisible)
                {
                    provinceNameTb.Visibility = Visibility.Collapsed;
                    provinceLbl.Visibility = Visibility.Collapsed;
                    region.Provinces.Add(new Province() { ProvinceName = provinceNameTb.Text });
                    addProvinceBtn.Content = "Add";
                }
                else
                {
                    provinceNameTb.Visibility = Visibility.Visible;
                    provinceLbl.Visibility = Visibility.Visible;
                    addProvinceBtn.Content = "Save";
                }
            }
        }

        private void editRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            addProvinceBtn.Content = "Save";
            provinceNameTb.Visibility = Visibility.Visible;
            provinceLbl.Visibility = Visibility.Visible;
            provinceNameTb.Text = MainVM.SelectedProvince.ProvinceName;
        }

        private void deleteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.isEdit)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = "odc_db";
                string query = "UPDATE provinces_t SET " +
                            "isDeleted = true" +
                            " WHERE id = " + MainVM.SelectedProvince.ProvinceID;
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    MessageBox.Show("Record is deleted");
                    MainVM.Ldt.worker.RunWorkerAsync();
                }
            }
            else
                region.Provinces.Remove(MainVM.SelectedProvince);

        }

        private bool validationError = false;
        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in form.Children)
                {
                    if (element is TextBox)
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
                    else if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
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
                    else if (element is ComboBox)
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
            if (dbCon.IsConnect())
            {
                if (MainVM.isEdit)
                {
                    string query = "UPDATE regions_t SET " +
                        "regionName = '" + regionNameTb.Text + "', " +
                        "ratePrice = '" + ratePriceTb.Value +
                        "' WHERE id = " + region.RegionID;
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Record is Saved");
                    }
                }
                else
                {
                    string query = "INSERT INTO regions_t (regionName,ratePrice) VALUES ('" +
                    regionNameTb.Text + "','" +
                    ratePriceTb.Value + "');";

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "SELECT LAST_INSERT_ID();";
                        MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                        DataSet fromDb = new DataSet();
                        DataTable fromDbTable = new DataTable();
                        dataAdapter.Fill(fromDb, "t");
                        fromDbTable = fromDb.Tables["t"];
                        string regionID = "";
                        foreach (DataRow dr in fromDbTable.Rows)
                            regionID = dr[0].ToString();
                        foreach (Province pr in region.Provinces)
                        {
                            query = "INSERT INTO `odc_db`.`provinces_t` (`provinceName`,`regionID`) VALUES('" + pr.ProvinceName + "','"+ regionID + "')";
                            if (dbCon.insertQuery(query, dbCon.Connection))
                            {
                                {

                                    MessageBox.Show("Record is Saved");
                                    dbCon.Close();
                                }
                            }
                        }

                    }
                }

            }
            resetFieldsValue();
        }

        private void resetFieldsValue()
        {
            foreach (var element in form.Children)
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
            regionNameTb.Text = MainVM.SelectedRegion.RegionName;
            ratePriceTb.Value = MainVM.SelectedRegion.RatePrice;
            MainVM.Provinces = MainVM.SelectedRegion.Provinces;
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void uControlLocation_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && MainVM.isEdit)
            {
                loadDataToUi();
                region = MainVM.SelectedRegion;
            }
            
        }

        
    }
}
