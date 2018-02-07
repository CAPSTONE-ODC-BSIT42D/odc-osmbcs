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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prototype2.uControlsMaintenance
{
    /// <summary>
    /// Interaction logic for ucServices.xaml
    /// </summary>
    public partial class ucServices : UserControl
    {
        public ucServices()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ucAddPhase.SaveCloseButtonClicked += addPhaseSaveCloseBtn_SaveCloseButtonClicked;
            this.ucAddPhase.AddPhaseItem += addPhaseItem_Clicked;
            this.ucAddPhaseItem.SaveCloseButtonClicked += addPhaseItemSaveCloseBtn_SaveCloseButtonClicked;
        }


        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private bool validationError;

        #region Custom EventArgs

        private void addPhaseSaveCloseBtn_SaveCloseButtonClicked(object sender, EventArgs e)
        {
            //Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            modalsGrid.Visibility = Visibility.Collapsed;
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (((UserControl)obj).Equals(ucAddPhase))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                }

            }
        }

        private void addPhaseItemSaveCloseBtn_SaveCloseButtonClicked(object sender, EventArgs e)
        {
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (((UserControl)obj).Equals(ucAddPhaseItem))
                    {
                        Grid.SetZIndex(((UserControl)obj), 0);
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                }

            }
        }

        private void addPhaseItem_Clicked(object sender, EventArgs e)
        {
            //Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            modalsGrid.Visibility = Visibility.Visible;
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Equals(ucAddPhaseItem))
                    {
                        Grid.SetZIndex(((UserControl)obj), 0);
                    }
                    else
                    {
                        Grid.SetZIndex(((UserControl)obj), 1);
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        //sb.Begin(((UserControl)obj));
                    }
                }

            }
        }



        #endregion

        private void addPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.SelectedPhaseGroup = new PhaseGroup();
            modalsGrid.Visibility = Visibility.Visible;
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (((UserControl)obj).Equals(ucAddPhase))
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                    }
                    else
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void saveServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            MessageBoxResult result = MessageBox.Show("Do you want to save this service type?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in serviceDetailsGrid.Children)
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
                        if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                        {
                            expression.UpdateSource();
                            if (Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element))
                                validationError = true;
                        }
                    }
                }
                if (!validationError)
                {
                    if (!MainVM.isEdit)
                    {
                        string query = "INSERT INTO services_t (serviceName,serviceDesc,servicePrice) VALUES ('" + serviceName.Text + "','" + serviceDesc.Text + "', '" + servicePrice.Value + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Service type successfully added!");

                            //clearing textboxes
                            serviceName.Clear();
                            serviceDesc.Clear();
                            servicePrice.Value = 0;
                            MainVM.Ldt.worker.RunWorkerAsync();
                        }
                        MainVM.isEdit = false;
                    }
                    else
                    {
                        string query = "UPDATE `services_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + MainVM.SelectedService.ServiceID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            //MessageBox.Show("Sevice type sucessfully updated");
                            MainVM.Ldt.worker.RunWorkerAsync();
                            OnSaveCloseButtonClicked(e);
                        }
                        MainVM.isEdit = false;
                    }
                }
                else
                    MessageBox.Show("Resolve the error first");
                validationError = false;
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }
        private void cancelServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        void loadDataToUi()
        {
            serviceName.Text = MainVM.SelectedService.ServiceName;
            serviceDesc.Text = MainVM.SelectedService.ServiceDesc;
            servicePrice.Value = MainVM.SelectedService.ServicePrice;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainVM.SelectedService = new Service();
            if(this.IsVisible && MainVM.isEdit)
            {
                loadDataToUi();
            }
        }

        



    }
}
