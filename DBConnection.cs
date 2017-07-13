using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace prototype2

{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            bool result = true;
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    result = false;
                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=password", databaseName);
                connection = new MySqlConnection(connstring);
                //connection.Open();
                result = true;
            }

            return result;
        }
        private MySqlCommand sqlCommand;
        
        public bool insertQuery(string query, MySqlConnection con)
        {
            if (query.Length > 0)
            {
                con.Open();
                
                try
                {
                    sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;
                    sqlCommand.CommandText = query;
                    sqlCommand.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    int errorcode = ex.Number;
                    if (errorcode == 1062)
                    {
                        MessageBox.Show("Duplicate Entry");
                    }
                    else
                    {
                        MessageBox.Show(""+ex);
                    }
                }
               
                con.Close();
                return true;
            }
            else
                return false;
            
        }
        public MySqlDataAdapter selectQuery(string query, MySqlConnection con)
        {
            if (query.Length > 0)
            {
                MySqlDataAdapter dataAdapater = new MySqlDataAdapter(query, con);
                Close();
                return dataAdapater;
            }
            else
                return null;
        }
        public bool deleteQuery(string query, MySqlConnection con)
        {
            if (query.Length > 0)
            {
                con.Open();
                sqlCommand = new MySqlCommand(query, con);
                sqlCommand.ExecuteNonQuery();
                Close();
                return true;
            }
            else
                return false;
        }

        public MySqlCommand storedProc(string query, MySqlConnection con)
        {
            if (query.Length > 0)
            {
                sqlCommand = new MySqlCommand();
                sqlCommand.Connection = con;
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                return sqlCommand;
            }
            else
                return null;

        }


        public void Close()
        {
            connection.Close();
        }
    }
}