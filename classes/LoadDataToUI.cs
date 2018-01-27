using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace prototype2
{
    public class LoadDataToUI
    {
        public LoadDataToUI()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

        }
        public static readonly BackgroundWorker worker_ = new BackgroundWorker();
        public BackgroundWorker worker
        {
            get { return worker_; }
        }
        

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { loadDataToUi(); }));
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {

        }

        private void loadDataToUi()
        {
            MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
            var dbCon = DBConnection.Instance();
            MainVM.Provinces.Clear();
            MainVM.Regions.Clear();
            MainVM.EmpPosition.Clear();
            MainVM.ContJobTitle.Clear();
            MainVM.ProductCategory.Clear();
            MainVM.ServicesList.Clear();
            MainVM.ProductList.Clear();



            MainVM.AllCustomerSupplier.Clear();
            MainVM.Customers.Clear();
            MainVM.Suppliers.Clear();

            MainVM.AllEmployeesContractor.Clear();
            MainVM.Employees.Clear();
            MainVM.Contractor.Clear();

            MainVM.RequestedItems.Clear();
            MainVM.AvailedItems.Clear();
            MainVM.AvailedServices.Clear();
            MainVM.AdditionalFees.Clear();

            MainVM.Units.Clear();

            MainVM.SalesQuotes.Clear();
            MainVM.SalesInvoice.Clear();
            MainVM.PaymentList_.Clear();

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM si_payment_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime paymentDate = new DateTime();
                    DateTime.TryParse(dr["SIpaymentDate"].ToString(), out paymentDate);
                    MainVM.PaymentList_.Add(new PaymentT() { SIpaymentID_ = int.Parse(dr["SIpaymentID"].ToString()), SIpaymentDate_ = paymentDate, SIpaymentAmount_ = decimal.Parse(dr["SIpaymentAmount"].ToString()), invoiceNo_ = int.Parse(dr["invoiceNo"].ToString()), SIpaymentMethod_ = dr["SIpaymentMethod"].ToString(), SIcheckNo_ = dr["SIcheckNo"].ToString() });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM regions_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var obj = new Region()
                    {
                        RegionID = int.Parse(dr[0].ToString()),
                        RegionName = dr[1].ToString(),
                        RatePrice = decimal.Parse(dr[2].ToString())
                    };
                    query = "SELECT * FROM provinces_t WHERE regionID = " + dr[0].ToString() + " AND isDeleted = 0;";
                    dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb2 = new DataSet();
                    DataTable fromDbTable2 = new DataTable();
                    dataAdapter.Fill(fromDb2, "t");
                    fromDbTable2 = fromDb2.Tables["t"];
                    foreach (DataRow dr2 in fromDbTable2.Rows)
                    {
                        var province = new Province() { ProvinceID = int.Parse(dr2[0].ToString()), ProvinceName = dr2[1].ToString(), RegionID = obj.RegionID };
                        obj.Provinces.Add(province);
                        MainVM.Provinces.Add(province);
                    }
                    MainVM.Regions.Add(obj);
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

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.EmpPosition.Add(new EmpPosition() { PositionID = int.Parse(dr["positionid"].ToString()), PositionName = dr["positionName"].ToString() });
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

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ContJobTitle.Add(new ContJobName() { JobID = int.Parse(dr["jobID"].ToString()), JobName = dr["jobName"].ToString() });
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

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ProductCategory.Add(new ItemType() { TypeID = int.Parse(dr["typeID"].ToString()), TypeName = dr["typeName"].ToString() });
                }

            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM unit_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Units.Add(new Unit() { ID = int.Parse(dr["id"].ToString()), UnitName = dr["unitName"].ToString(), UnitShorthand = dr["unitShorthand"].ToString() });
                }

            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM services_t where isDeleted = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ServicesList.Add(new Service() { ServiceID = int.Parse(dr["serviceID"].ToString()), ServiceName = dr["serviceName"].ToString(), ServiceDesc = dr["serviceDesc"].ToString(), ServicePrice = (decimal)dr["ServicePrice"] });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM ITEM_T WHERE isDeleted = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int supplierID;
                    int.TryParse(dr["supplierID"].ToString(), out supplierID);
                    MainVM.ProductList.Add(new Item() { ID = int.Parse(dr["id"].ToString()), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), MarkUpPerc = decimal.Parse(dr["markupPerc"].ToString()), TypeID = int.Parse(dr["typeID"].ToString()), UnitID = int.Parse(dr["unitID"].ToString()), SupplierID = supplierID, DateEffective = DateTime.Parse(dr["dateEffective"].ToString()) });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.id " +
                    "WHERE cs.isDeleted = 0 AND cs.companyType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var customer = (new Customer() { CompanyID = int.Parse(dr["companyID"].ToString()), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceID = int.Parse(dr["companyProvinceID"].ToString()), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() });
                    MainVM.Customers.Add(customer);
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.id " +
                    "WHERE cs.isDeleted = 0 AND cs.companyType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var supplier = new Customer() { CompanyID = int.Parse(dr["companyID"].ToString()), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceID = int.Parse(dr["companyProvinceID"].ToString()), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() };
                    MainVM.Suppliers.Add(supplier);
                }
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM emp_cont_t a JOIN position_t c ON a.positionID = c.positionid  WHERE isDeleted = 0 AND empType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int empType;
                    int.TryParse(dr["empType"].ToString(), out empType);
                    MainVM.Employees.Add(new Employee() { EmpID = int.Parse(dr["empID"].ToString()), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), PositionID = int.Parse(dr["positionID"].ToString()), EmpUserName = dr["empUserName"].ToString(), EmpType = empType, HasAccess = bool.Parse(dr["hasAccess"].ToString()) });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = int.Parse(dr["empID"].ToString()), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), PositionID = int.Parse(dr["positionID"].ToString()), EmpUserName = dr["empUserName"].ToString(), EmpType = empType, HasAccess = bool.Parse(dr["hasAccess"].ToString()) });
                }
                dbCon.Close();
            }
            //MainVM.LoginEmployee_ = MainVM.Employees.Where(x => x.EmpID.Equals(empID)).FirstOrDefault();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM emp_cont_t a  " +
                    "JOIN job_title_t d ON a.jobID = d.jobID " +
                    "WHERE a.isDeleted = 0 AND a.empType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.Contractor.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int empType;
                    int.TryParse(dr["empType"].ToString(), out empType);
                    DateTime dateTo;
                    DateTime.TryParse(dr["empDateTo"].ToString(), out dateTo);

                    DateTime dateFrom;
                    DateTime.TryParse(dr["empDateFrom"].ToString(), out dateFrom);

                    MainVM.Contractor.Add(new Employee() { EmpID = int.Parse(dr["empID"].ToString()), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(),EmpAddress = dr["empAddress"].ToString() ,JobID = int.Parse(dr["jobID"].ToString()), EmpDateFrom = dateFrom, EmpDateTo = dateTo, EmpUserName = dr["empUserName"].ToString(), EmpType = empType, HasAccess = bool.Parse(dr["hasAccess"].ToString()) });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = int.Parse(dr["empID"].ToString()), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), JobID = int.Parse(dr["jobID"].ToString()), EmpDateFrom = dateFrom, EmpDateTo = dateTo, EmpUserName = dr["empUserName"].ToString(), EmpType = empType, HasAccess = bool.Parse(dr["hasAccess"].ToString()) });
                }
                dbCon.Close();
            }


            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM items_availed_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var availedItems = new AvailedItem() { AvailedItemID = int.Parse(dr["id"].ToString()), SqNoChar = dr["sqNoChar"].ToString(), ItemID = int.Parse(dr["itemID"].ToString()), ItemQty = int.Parse(dr["itemQnty"].ToString()), TotalCost = decimal.Parse(dr["totalCost"].ToString()) };
                    MainVM.AvailedItems.Add(availedItems);
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM services_availed_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var availedservices = new AvailedService() { AvailedServiceID = int.Parse(dr["ID"].ToString()), SqNoChar = dr["sqNoChar"].ToString(), ServiceID = int.Parse(dr["serviceID"].ToString()), ProvinceID = int.Parse(dr["provinceID"].ToString()), City = dr["city"].ToString(), TotalCost = decimal.Parse(dr["totalCost"].ToString()) };
                    query = "SELECT * FROM fees_per_transaction_t WHERE servicesAvailedID = " + dr["ID"].ToString() + ";";
                    dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb2 = new DataSet();
                    DataTable fromDbTable2 = new DataTable();
                    dataAdapter.Fill(fromDb2, "t");
                    fromDbTable2 = fromDb2.Tables["t"];
                    foreach (DataRow dr2 in fromDbTable2.Rows)
                    {
                        availedservices.AdditionalFees.Add(new AdditionalFee() { FeeID = int.Parse(dr2["feeID"].ToString()), ServicesAvailedID = int.Parse(dr2["servicesAvailedID"].ToString()), FeeName = dr2["feeName"].ToString(), FeePrice = decimal.Parse(dr2["feeValue"].ToString()) });
                    }
                    MainVM.AvailedServices.Add(availedservices);
                }
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM sales_quote_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateOfIssue = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out dateOfIssue);

                    DateTime deliveryDate = new DateTime();
                    DateTime.TryParse(dr["deliveryDate"].ToString(), out deliveryDate);

                    DateTime validityDate = new DateTime();
                    DateTime.TryParse(dr["validityDate"].ToString(), out validityDate);

                    DateTime expiration = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out expiration);

                    bool vatIsExcluded = false;

                    if (dr["vatIsExcluded"].ToString().Equals("1"))
                    {
                        vatIsExcluded = true;
                    }

                    bool paymentIsLanded = false;

                    if (dr["paymentIsLanded"].ToString().Equals("1"))
                    {
                        paymentIsLanded = true;
                    }

                    int custId;
                    int.TryParse(dr["custID"].ToString(), out custId);

                    int estDelivery;
                    int.TryParse(dr["estDelivery"].ToString(), out estDelivery);

                    int validityDays;
                    int.TryParse(dr["validityDays"].ToString(), out validityDays);

                    decimal vat;
                    decimal.TryParse(dr["vat"].ToString(), out vat);

                    int termsDays;
                    int.TryParse(dr["termsDays"].ToString(), out termsDays);

                    int termsDP;
                    int.TryParse(dr["termsDP"].ToString(), out termsDP);

                    decimal penaltyAmt;
                    decimal.TryParse(dr["penaltyAmt"].ToString(), out penaltyAmt);

                    int penaltyPerc;
                    int.TryParse(dr["penaltyPerc"].ToString(), out penaltyPerc);

                    decimal markUpPerc;
                    decimal.TryParse(dr["markUpPercent"].ToString(), out markUpPerc);

                    decimal discountPerc;
                    decimal.TryParse(dr["discountPercent"].ToString(), out discountPerc);

                    MainVM.SalesQuotes.Add(new SalesQuote()
                    {
                        sqNoChar_ = dr["sqNoChar"].ToString(),
                        dateOfIssue_ = dateOfIssue,
                        custID_ = custId,
                        quoteSubject_ = dr["quoteSubject"].ToString(),
                        priceNote_ = dr["priceNote"].ToString(),
                        deliveryDate_ = deliveryDate,
                        estDelivery_ = estDelivery,
                        validityDays_ = validityDays,
                        validityDate_ = validityDate,
                        otherTerms_ = dr["otherTerms"].ToString(),
                        expiration_ = expiration,
                        vat_ = vat,
                        vatexcluded_ = vatIsExcluded,
                        paymentIsLanded_ = paymentIsLanded,
                        paymentCurrency_ = dr["paymentCurrency"].ToString(),
                        status_ = dr["status"].ToString(),
                        termsDays_ = termsDays,
                        termsDP_ = termsDP,
                        penaltyAmt_ = penaltyAmt,
                        penaltyPercent_ = penaltyAmt,
                        markUpPercent_ = markUpPerc,
                        discountPercent_ = discountPerc
                        
                    });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM sales_invoice_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.SalesInvoice.Clear();

                //query = "SELECT * FROM payment_hist_t;";
                //MySqlDataAdapter dataAdapter2 = dbCon.selectQuery(query, dbCon.Connection);
                //DataSet fromDb2 = new DataSet();
                //DataTable fromDbTable2 = new DataTable();
                //dataAdapter2.Fill(fromDb2, "t");
                //fromDbTable2 = fromDb2.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateOfIssue = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out dateOfIssue);

                    DateTime dueDate = new DateTime();
                    DateTime.TryParse(dr["dueDate"].ToString(), out dueDate);
                    MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(dr["custID"].ToString())).FirstOrDefault();

                    int invoiceNo;
                    int.TryParse(dr["invoiceNo"].ToString(), out invoiceNo);

                    int custId;
                    int.TryParse(dr["custID"].ToString(), out custId);

                    //int empId;
                    //int.TryParse(dr["empID"].ToString(), out empId);

                    int estDelivery;
                    int.TryParse(dr["invoiceNo"].ToString(), out estDelivery);

                    int termsDays;
                    int.TryParse(dr["termsDays"].ToString(), out termsDays);

                    decimal vat;
                    decimal.TryParse(dr["vat"].ToString(), out vat);

                    decimal sc_pwd_discount;
                    decimal.TryParse(dr["sc_pwd_discount"].ToString(), out sc_pwd_discount);

                    decimal withholdingTax;
                    decimal.TryParse(dr["withholdingTax"].ToString(), out withholdingTax);

                    MainVM.SalesInvoice.Add(new SalesInvoice() { invoiceNo_ = invoiceNo.ToString(), custID_ = custId, sqNoChar_ = dr["sqNoChar"].ToString(), tin_ = dr["tin"].ToString(), busStyle_ = dr["busStyle"].ToString(), dateOfIssue_ = dateOfIssue, terms_ = termsDays, dueDate_ = dueDate, purchaseOrderNumber_ = dr["purchaseOrderNumber"].ToString(), paymentStatus_ = dr["paymentStatus"].ToString(), vat_ = vat, sc_pwd_discount_ = sc_pwd_discount, withholdingTax_ = withholdingTax, notes_ = dr["notes"].ToString() });



                }
                //foreach (DataRow dr in fromDbTable2.Rows)
                //{
                //    DateTime paymentDate = new DateTime();
                //    DateTime.TryParse(dr["paymentDate"].ToString(), out paymentDate);

                //    int invoiceNo;
                //    int.TryParse(dr["invoiceNo"].ToString(), out invoiceNo);

                //    decimal custBalance;
                //    decimal.TryParse(dr["custBalance"].ToString(), out custBalance);
                //    MainVM.PaymentHistory_.Add(new PaymentHist() { custHistID_ = int.Parse(dr["custHistID"].ToString()), paymentDate_ = paymentDate, custBalance_ = custBalance, invoiceNo_ = invoiceNo, paymentStatus_ = dr["paymentStatus"].ToString() });
                //}
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM service_sched_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                query = "SELECT * FROM assigned_employees_t;";
                MySqlDataAdapter dataAdapter2 = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb2 = new DataSet();
                DataTable fromDbTable2 = new DataTable();
                dataAdapter2.Fill(fromDb2, "t");
                fromDbTable2 = fromDb2.Tables["t"];

                MainVM.ServiceSchedules_.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateStarted = new DateTime();
                    DateTime.TryParse(dr["dateStarted"].ToString(), out dateStarted);

                    DateTime dateEnded = new DateTime();
                    DateTime.TryParse(dr["dateEnded"].ToString(), out dateEnded);

                    int invoiceNo;
                    int.TryParse(dr["invoiceNo"].ToString(), out invoiceNo);

                    MainVM.SelectedServiceSchedule_ = (new ServiceSchedule() { serviceSchedNoChar_ = dr["serviceSchedNoChar"].ToString(), invoiceNo_ = invoiceNo, serviceStatus_ = dr["serviceStatus"].ToString(), dateStarted_ = dateStarted, dateEnded_ = dateEnded, schedNotes_ = dr["schedNotes"].ToString() });
                    if (fromDbTable2.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in fromDbTable2.Rows)
                        {

                            MainVM.SelectedEmployeeContractor = MainVM.AllEmployeesContractor.Where(x => x.EmpID.Equals(dr2["empID"].ToString())).FirstOrDefault();
                            MainVM.SelectedServiceSchedule_.assignedEmployees_.Add(MainVM.SelectedEmployeeContractor);
                        }
                    }

                }



                MainVM.ServiceSchedules_.Clear();
                dbCon.Close();
            }
        }
    }
}
