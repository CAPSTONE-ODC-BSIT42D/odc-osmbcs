using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                            MainVM.SelectedProvince.ProvinceName = provinceNameTb.Text;
                            MessageBox.Show("Record is Saved");
                        }
                    }
                    else
                    {
                        string query = "INSERT INTO `odc_db`.`provinces_t` (`provinceName`,`regionID`) VALUES('" + provinceNameTb.Text + "','" + MainVM.SelectedRegion.RegionID + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record is Saved");
                            query = "SELECT LAST_INSERT_ID();";
                            string result = dbCon.selectScalar(query, dbCon.Connection).ToString();
                            MainVM.SelectedRegion.Provinces.Add(new Province() { ProvinceID = int.Parse(result), ProvinceName = provinceNameTb.Text });
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
                    MainVM.SelectedRegion.Provinces.Add(new Province() { ProvinceName = provinceNameTb.Text });
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
                    MainVM.SelectedRegion.Provinces.Remove(MainVM.SelectedProvince);
                }
            }
            else
                MainVM.SelectedRegion.Provinces.Remove(MainVM.SelectedProvince);

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
                    MainVM.Provinces.Clear();
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
                MainVM.Provinces.Clear();
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
                        "' WHERE id = " + MainVM.SelectedRegion.RegionID;
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
                        foreach (Province pr in MainVM.SelectedRegion.Provinces)
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
            MainVM.SelectedRegion.Provinces = new ObservableCollection<Province> (from prov in MainVM.Provinces
                                              where prov.RegionID == MainVM.SelectedRegion.RegionID
                                              select prov);
            Grid.SetZIndex(disableGrid, 1);
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
            }
            else
            {
                MainVM.SelectedRegion = new Region();
            }
            
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetZIndex(disableGrid, 0);
            saveCancelGrid2.Visibility = Visibility.Visible;
            editCloseGrid2.Visibility = Visibility.Collapsed;
        }
    }
}
