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
    /// Interaction logic for ucAddPhase.xaml
    /// </summary>
    public partial class ucAddPhase : UserControl
    {
        public ucAddPhase()
        {
            InitializeComponent();
        }

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler AddPhaseItem;
        protected virtual void OnAddPhaseItem(RoutedEventArgs e)
        {
            var handler = AddPhaseItem;
            if (handler != null)
                handler(this, e);
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private bool validationError = false;

        private void addPhaseItemBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddPhaseItem(e);
        }

        private void editRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddPhaseItem(e);
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
                            MainVM.SelectedPhaseGroup.PhaseItems.Remove(MainVM.SelectedPhase);
                        }
                    }
                }
                else
                    MainVM.SelectedPhaseGroup.PhaseItems.Remove(MainVM.SelectedPhase);

            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            MessageBoxResult result = MessageBox.Show("Do you want to save this service type?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in phaseDetailsForm.Children)
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
                }
                if (!validationError)
                {
                    if (MainVM.SelectedPhaseGroup.PhaseGroupName != null)
                    {
                        MainVM.SelectedPhaseGroup.PhaseGroupName = phaseNameTb.Text;
                        MainVM.SelectedPhaseGroup.PhaseGroupDesc = phaseDescTb.Text;
                        MainVM.SelectedPhaseGroup.IsModified = true;
                    }
                    else
                    {
                        //MainVM.SelectedService.PhaseGroups.Add(new PhaseGroup() { PhaseGroupName = phaseNameTb.Text, PhaseGroupDesc = phaseDescTb.Text, PhaseItems = MainVM.SelectedPhaseGroup.PhaseItems });
                    }
                }
                else
                    MessageBox.Show("Resolve the error first");
                validationError = false;
                
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
            
            
            OnSaveCloseButtonClicked(e);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void moveUpSeqNoBtn_Click(object sender, RoutedEventArgs e)
        {

            if (MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) == 1)
            {
                MainVM.SelectedPhase.FirstItem = true;
                MainVM.SelectedPhase.LastItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems[0].FirstItem = false;
                if(MainVM.SelectedPhaseGroup.PhaseItems.Count == 2)
                    MainVM.SelectedPhaseGroup.PhaseItems[0].LastItem = true;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1);

            }
            else
            {
                MainVM.SelectedPhase.LastItem = false;
                if(MainVM.SelectedPhaseGroup.PhaseItems.Count - 1 == MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase))
                    MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1].LastItem = true;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1);
            }
        }

        private void moveDownSeqNoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) == MainVM.SelectedPhaseGroup.PhaseItems.Count - 2)
            {
                MainVM.SelectedPhase.FirstItem = false;
                MainVM.SelectedPhase.LastItem = true;
                if (MainVM.SelectedPhaseGroup.PhaseItems.Count == 2)
                    MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 1].LastItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 1].LastItem = false;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.Count + 1);

            }
            else
            {
                MainVM.SelectedPhase.FirstItem = false;
                if (MainVM.SelectedPhaseGroup.PhaseItems.Count - 1 == MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase))
                    MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1].LastItem = false;
                MainVM.SelectedPhase.SequenceNo = MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) + 1);
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && !String.IsNullOrWhiteSpace(MainVM.SelectedPhaseGroup.PhaseGroupName))
            {
                loadDataToUI();
            }
        }

        private void loadDataToUI()
        {
            phaseNameTb.Text = MainVM.SelectedPhaseGroup.PhaseGroupName;
            phaseDescTb.Text = MainVM.SelectedPhaseGroup.PhaseGroupDesc;
            MainVM.SelectedPhaseGroup.PhaseItems = new ObservableCollection<Phase>(from pi in MainVM.Phases where pi.PhaseGroupID == MainVM.SelectedPhaseGroup.PhaseGroupID select pi);
        }

       
    }
}
