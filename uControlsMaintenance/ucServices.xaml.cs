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
            //this.ucAddPhase.SaveCloseButtonClicked += addPhaseSaveCloseBtn_SaveCloseButtonClicked;
            //this.ucAddPhase.AddPhaseItem += addPhaseItem_Clicked;
            this.ucAddPhaseItem.SaveCloseButtonClicked += addPhaseItemSaveCloseBtn_SaveCloseButtonClicked;
        }


        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
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
           
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (((UserControl)obj).Equals(ucAddPhaseItem))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                }

            }
        }

        private void addPhaseItemSaveCloseBtn_SaveCloseButtonClicked(object sender, EventArgs e)
        {
            modalsGrid.Visibility = Visibility.Collapsed;
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
            modalsGrid.Visibility = Visibility.Visible;
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (((UserControl)obj).Equals(ucAddPhaseItem))
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                    }
                    else
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void editRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            modalsGrid.Visibility = Visibility.Visible;
            foreach (var obj in modalsGrid.Children)
            {
                if (obj is UserControl)
                {
                    if (((UserControl)obj).Equals(ucAddPhaseItem))
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                    }
                    else
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void deleteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                if (MainVM.isEdit)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `phase_t` SET `isDeleted`= 1 WHERE phaseID = '" + MainVM.SelectedPhase.PhaseID;
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            MainVM.SelectedService.Phases.Remove(MainVM.SelectedPhase);
                        }
                    }
                }
                else
                    MainVM.SelectedService.Phases.Remove(MainVM.SelectedPhase);

            }
            else if (result == MessageBoxResult.Cancel)
            {
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
                            query = "SELECT LAST_INSERT_ID();";
                            string serviceId = dbCon.selectScalar(query, dbCon.Connection).ToString();
                            foreach (Phase pi in MainVM.SelectedService.Phases)
                            {
                                query = "INSERT INTO phase_t (phaseName, phaseDesc, sequenceNo, serviceID) VALUE ('" + pi.PhaseName + "', '" + pi.PhaseDesc + "','" + pi.SequenceNo + "','" + serviceId + "')";
                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {
                                    
                                }
                            }
                            //clearing textboxes
                            serviceName.Clear();
                            serviceDesc.Clear();
                            servicePrice.Value = 0;
                            MainVM.Ldt.worker.RunWorkerAsync();
                        }
                        MessageBox.Show("Service type successfully added!");
                    }
                    else
                    {
                        string query = "UPDATE `services_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + MainVM.SelectedService.ServiceID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            var pgs = from pi in MainVM.SelectedService.Phases
                                      where pi.IsModified = true
                                      select pi;
                            if(pgs != null)
                            {
                                foreach (Phase pi in pgs)
                                {
                                    query = "UPDATE `phase_t` SET phaseName = '" + pi.PhaseName + "', phaseDesc = '" + pi.PhaseDesc + "', sequenceNo = '" + pi.SequenceNo + "' WHERE phaseID = '" + pi.PhaseID + "'";
                                    dbCon.insertQuery(query, dbCon.Connection);
                                }

                            }

                        }
                        MessageBox.Show("Sevice type sucessfully updated");
                    }
                    OnSaveCloseButtonClicked(e);
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
            MainVM.SelectedService.Phases = new ObservableCollection<Phase>(from pg in MainVM.Phases
                                                                                      where pg.ServiceID == MainVM.SelectedService.ServiceID
                                                                                      select pg);
        }


        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            if(this.IsVisible && MainVM.isEdit)
            {
                loadDataToUi();
            }
            else
            {
                MainVM.SelectedService = new Service();
                saveCloseButtonGrid.Visibility = Visibility.Visible;
                editCloseButtonGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void editServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement obj in serviceForm.Children)
            {
                obj.IsEnabled = true;
            }
            saveCloseButtonGrid.Visibility = Visibility.Visible;
            editCloseButtonGrid.Visibility = Visibility.Collapsed;
        }

        private void moveUpSeqNoBtn_Click(object sender, RoutedEventArgs e)
        {

            if (MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) == 1)
            {
                MainVM.SelectedPhase.FirstItem = true;
                MainVM.SelectedPhase.LastItem = false;
                MainVM.SelectedService.Phases[0].FirstItem = false;
                if (MainVM.SelectedService.Phases.Count == 2)
                    MainVM.SelectedService.Phases[0].LastItem = true;
                MainVM.SelectedPhase.SequenceNo =MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedService.Phases.Move(MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase), MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1);

            }
            else
            {
                MainVM.SelectedPhase.LastItem = false;
                if (MainVM.SelectedService.Phases.Count - 1 == MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase))
                    MainVM.SelectedService.Phases[MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1].LastItem = true;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedService.Phases.Move(MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase), MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1);
            }
        }

        private void moveDownSeqNoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) == MainVM.SelectedService.Phases.Count - 2)
            {
                MainVM.SelectedPhase.FirstItem = false;
                MainVM.SelectedPhase.LastItem = true;
                if (MainVM.SelectedService.Phases.Count == 2)
                    MainVM.SelectedService.Phases[MainVM.SelectedService.Phases.Count - 1].LastItem = false;
                MainVM.SelectedService.Phases[MainVM.SelectedService.Phases.Count - 1].LastItem = false;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedService.Phases.Move(MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase), MainVM.SelectedService.Phases.Count + 1);

            }
            else
            {
                MainVM.SelectedPhase.FirstItem = false;
                if (MainVM.SelectedService.Phases.Count - 1 == MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase))
                    MainVM.SelectedService.Phases[MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1].LastItem = false;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedService.Phases.Move(MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase), MainVM.SelectedService.Phases.IndexOf(MainVM.SelectedPhase) + 1);
            }
        }

    }
}
