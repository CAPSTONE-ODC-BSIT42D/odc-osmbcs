using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var dbCon = DBConnection.Instance();
            if (String.IsNullOrWhiteSpace(usernameTb.Text) && String.IsNullOrWhiteSpace(passwordBox.Password.ToString()))
            {
                MessageBox.Show("Username and Password must be filled.");
            }
            else
            {
                if (usernameTb.Text.Equals("admin") && passwordBox.Password.Equals("admin"))
                {
                    toLogin();
                }
                else if (dbCon.IsConnect())
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        SecureString passwordsalt = passwordBox.SecurePassword;
                        foreach (Char c in "$w0rdf!$h")
                        {
                            passwordsalt.AppendChar(c);
                        }
                        passwordsalt.MakeReadOnly();

                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("LOGIN_PROC", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", usernameTb.Text);
                        cmd.Parameters["@username"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@upassword", SecureStringToString(passwordsalt));
                        cmd.Parameters["@upassword"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                        cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;


                        cmd.ExecuteNonQuery();
                        string empId = cmd.Parameters["@insertedid"].Value.ToString();


                        if (!String.IsNullOrWhiteSpace(empId))
                        {
                            toLogin();
                        }
                        else
                        {
                            MessageBox.Show("Username or Password is incorrect.");
                            usernameTb.Clear();
                            passwordBox.Clear();
                        }
                    }
                }
            }
            
            

        }

        public void toLogin()
        {
            MainMenu mainMenu = new MainMenu();
            this.Hide();
            mainMenu.ShowDialog();
            this.Show();
        }

        String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
