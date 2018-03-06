using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prototype2
{
    public class Commands
    {
        public Commands()
        {

        }

        public MainViewModel FillViewModels()
        {
            var dbCon = DBConnection.Instance();
            var mV = new MainViewModel();

            if (dbCon.IsConnect())
            {
                string query = "SELECT locProvinceID, locProvince FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                mV.Provinces.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.Provinces.Add(new Province() { ProvinceID = (int)dr["locProvinceID"], ProvinceName = dr["locProvince"].ToString() });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT locationProvinceID, locationPrice FROM location_details_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    foreach (Province prov in mV.Provinces)
                    {
                        if (prov.ProvinceID == (int)dr["locationProvinceID"])
                            prov.ProvincePrice = (decimal)dr["locationPrice"];
                    }
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM POSITION_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                mV.EmpPosition.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.EmpPosition.Add(new EmpPosition() { PositionID = dr["positionid"].ToString(), PositionName = dr["positionName"].ToString() });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM JOB_TITLE_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                mV.ContJobTitle.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.ContJobTitle.Add(new ContJobName() { JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString() });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM ITEM_TYPE_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                mV.ProductCategory.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.ProductCategory.Add(new ItemType() { TypeID = int.Parse(dr["typeID"].ToString()), TypeName = dr["typeName"].ToString() });
                }

            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM service_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                mV.ServicesList.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.ServicesList.Add(new Service() { ServiceID = dr["serviceID"].ToString(), ServiceName = dr["serviceName"].ToString(), ServiceDesc = dr["serviceDesc"].ToString(), ServicePrice = (decimal)dr["ServicePrice"] });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM ITEM_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                mV.ProductList.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.SelectedCustomerSupplier = mV.Suppliers.Where(x => x.CompanyID.Equals(dr["supplierID"].ToString())).FirstOrDefault();
                    mV.ProductList.Add(new Item() { ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(), Quantity = 1, SupplierID = dr["supplierID"].ToString(), SupplierName = mV.SelectedCustomerSupplier.CompanyName });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT locationProvinceID, locationPrice FROM location_details_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    foreach (Province prov in mV.Provinces)
                    {
                        if (prov.ProvinceID == (int)dr["locationProvinceID"])
                            prov.ProvincePrice = (decimal)dr["locationPrice"];
                    }
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID, cs.companyEmail, cs.companyTelephone, cs.companyMobile " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.Customers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString() });
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID, cs.companyEmail, cs.companyTelephone, cs.companyMobile " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    mV.Suppliers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString() });
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * representative_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                   
                }
            }
            //if (dbCon.IsConnect())
            //{
            //    string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID " +
            //        "FROM cust_supp_t cs  " +
            //        "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
            //        "WHERE isDeleted = 0 AND companyType = 1;";
            //    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            //    DataSet fromDb = new DataSet();
            //    DataTable fromDbTable = new DataTable();
            //    dataAdapter.Fill(fromDb, "t");
            //    fromDbTable = fromDb.Tables["t"];
            //    MainScreen.MainVM.Suppliers.Clear();
            //    foreach (DataRow dr in fromDbTable.Rows)
            //    {
            //        MainScreen.MainVM.Suppliers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString() });
            //    }
            //    dbCon.Close();
            //}
            //if (dbCon.IsConnect())
            //{
            //    string query = "SELECT a.empID, a.empFName,a.empLname, a.empMI, a.empAddinfo, a.empAddress, a.empCity, a.empProvinceID, a.empUserName, b.locprovince, a.positionID ,c.positionName, a.jobID, d.empPic, d.empSignature " +
            //        "FROM emp_cont_t a  " +
            //        "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
            //        "JOIN position_t c ON a.positionID = c.positionid " +
            //        "JOIN emp_pic_t d ON a.empID = d.empID " +
            //        //"JOIN job_title_t d ON a.jobID = d.jobID " +
            //        "WHERE isDeleted = 0 AND empType = 0;";
            //    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            //    DataSet fromDb = new DataSet();
            //    DataTable fromDbTable = new DataTable();
            //    dataAdapter.Fill(fromDb, "t");
            //    fromDbTable = fromDb.Tables["t"];
            //    MainScreen.MainVM.Employees.Clear();
            //    foreach (DataRow dr in fromDbTable.Rows)
            //    {
            //        if (dr["empPic"].Equals(DBNull.Value))
            //        {
            //            MainScreen.MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString() });
            //        }
            //        else
            //        {
            //            MainScreen.MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpPic = (byte[])dr["empPic"], EmpUserName = dr["empUserName"].ToString() });
            //        }

            //    }
            //    dbCon.Close();
            //}
            //if (dbCon.IsConnect())
            //{
            //    string query = "SELECT a.empID, a.empFName,a.empLname, a.empMI, a.empAddinfo, a.empAddress, a.empCity, a.empProvinceID, b.locprovince, a.jobID, d.jobName " +
            //        "FROM emp_cont_t a  " +
            //        "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
            //        //"JOIN position_t c ON a.positionID = c.positionid " +
            //        "JOIN job_title_t d ON a.jobID = d.jobID " +
            //        "WHERE a.isDeleted = 0 AND a.empType = 1;";
            //    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            //    DataSet fromDb = new DataSet();
            //    DataTable fromDbTable = new DataTable();
            //    dataAdapter.Fill(fromDb, "t");
            //    fromDbTable = fromDb.Tables["t"];
            //    MainScreen.MainVM.Contractor.Clear();
            //    foreach (DataRow dr in fromDbTable.Rows)
            //    {
            //        MainScreen.MainVM.Contractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString() });
            //    }
            //    dbCon.Close();
            //}


            return mV;
        }
    }

}
