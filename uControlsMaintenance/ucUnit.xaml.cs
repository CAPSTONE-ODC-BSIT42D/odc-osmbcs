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
    /// Interaction logic for ucUnit.xaml
    /// </summary>
    public partial class ucUnit : UserControl
    {
        public ucUnit()
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

        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in unitForm.Children)
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
                    MainVM.Ldt.worker.RunWorkerAsync();
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
                if (!MainVM.isEdit)
                {
                    string query = "INSERT INTO unit_t (unitName, unitShorthand) VALUES ('" +
                    unitNameTb.Text + "','" +
                    unitShorthandTb.Text +"');";

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Record is Saved");

                    }
                }
                else
                {
                    string query = "UPDATE unit_t SET " +
                        "unitName = " + unitNameTb.Text +
                        ", unitShorthand = " + unitShorthandTb.Text +
                        " WHERE id = "+MainVM.SelectedUnit.ID+";";

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
            foreach (var element in unitForm.Children)
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
            unitNameTb.Text = MainVM.SelectedUnit.UnitName;
            unitShorthandTb.Text = MainVM.SelectedUnit.UnitShorthand;
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
