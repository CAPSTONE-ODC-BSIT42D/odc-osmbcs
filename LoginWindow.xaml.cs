using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            String uname = "";
            String pword = "";
            uname = usernameTb.Text;
            pword = passwordBox.Password.ToString();
            if (String.IsNullOrWhiteSpace(usernameTb.Text) && String.IsNullOrWhiteSpace(passwordBox.Password.ToString()))
            {
                MessageBox.Show("Username and Password must be filled.");
            }
            else
            {
                if (uname.Equals("admin") && pword.Equals("adminadmin"))
                {
                    MainMenu mainMenu = new MainMenu();
                    this.Hide();
                    mainMenu.ShowDialog();
                    this.Show();
                }
                else
                {
                    usernameTb.Text = "";
                    passwordBox.Password = "";
                    MessageBox.Show("Username and Password do not match.");

                }

            }
        }
    }
}
