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

            //if (dbCon.IsConnect())
            //{
            //    MainVM.PhasesGroup.Clear();
            //    string query = "SELECT * FROM phases_group_t";
            //    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            //    DataSet fromDb = new DataSet();
            //    DataTable fromDbTable = new DataTable();
            //    dataAdapter.Fill(fromDb, "t");
            //    fromDbTable = fromDb.Tables["t"];
            //    foreach (DataRow dr in fromDbTable.Rows)
            //    {
            //        MainVM.PhasesGroup.Add(new PhaseGroup()
            //        {
            //            PhaseGroupID = int.Parse(dr[0].ToString()),
            //            PhaseGroupName = dr[1].ToString(),
            //            SequenceNo = int.Parse(dr[2].ToString()),
            //            ServiceID = int.Parse(dr[3].ToString())

            //        });
            //    }
            //    dbCon.Close();
            //}

            if (dbCon.IsConnect())
            {
                MainVM.Phases.Clear();
                string query = "SELECT * FROM phase_t where isDeleted = 0";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Phases.Add(new Phase()
                    {
                        PhaseID = int.Parse(dr[0].ToString()),
                        PhaseName = dr[1].ToString(),
                        PhaseDesc = dr[2].ToString(),
                        SequenceNo = int.Parse(dr[3].ToString()),
                        ServiceID = int.Parse(dr[4].ToString())

                    });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                MainVM.Regions.Clear();
                MainVM.Provinces.Clear();
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
                    MainVM.Regions.Add(obj);
                }
                query = "SELECT * FROM provinces_t WHERE isDeleted = 0;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb2 = new DataSet();
                DataTable fromDbTable2 = new DataTable();
                dataAdapter.Fill(fromDb2, "t");
                fromDbTable2 = fromDb2.Tables["t"];
                foreach (DataRow dr2 in fromDbTable2.Rows)
                {
                    var province = new Province() { ProvinceID = int.Parse(dr2[0].ToString()), ProvinceName = dr2[1].ToString(), RegionID = int.Parse(dr2[2].ToString()) };
                    MainVM.Provinces.Add(province);
                }
                dbCon.Close();
            }


            if (dbCon.IsConnect())
            {
                MainVM.EmpPosition.Clear();
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
                MainVM.ContJobTitle.Clear();
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
                MainVM.ProductCategory.Clear();
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
                MainVM.ShipVia.Clear();
                string query = "SELECT * FROM ship_via_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ShipVia.Add(new ShipVia() { ShipViaID = int.Parse(dr[0].ToString()), Name = dr[1].ToString() });
                }

            }

            if (dbCon.IsConnect())
            {
                MainVM.MarkupHist.Clear();
                string query = "SELECT * FROM markup_hist_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.MarkupHist.Add(new Markup_History() { MarkupID = int.Parse(dr[0].ToString()), MarkupPerc = decimal.Parse(dr[1].ToString()), DateEffective = DateTime.Parse(dr[2].ToString()), ItemID = int.Parse(dr[3].ToString()) });
                }

            }

            if (dbCon.IsConnect())
            {
                MainVM.Units.Clear();
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
                MainVM.ServicesList.Clear();
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
                MainVM.ProductList.Clear();
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
                    MainVM.ProductList.Add(new Item() { ID = int.Parse(dr["id"].ToString()), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), TypeID = int.Parse(dr["typeID"].ToString()), UnitID = int.Parse(dr["unitID"].ToString()), SupplierID = supplierID });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                MainVM.Customers.Clear();
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
                    var customer = (new Customer() { CompanyID = int.Parse(dr["companyID"].ToString()), CompanyName = dr["companyName"].ToString(), BusStyle = dr["busStyle"].ToString(),TaxNumber = dr["taxNumber"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceID = int.Parse(dr["companyProvinceID"].ToString()), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), CompanyType = int.Parse(dr["companyType"].ToString()), CompanyPostalCode = dr["companyPostalCode"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() });
                    MainVM.Customers.Add(customer);
                }
            }

            if (dbCon.IsConnect())
            {
                MainVM.Suppliers.Clear();
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
                    var supplier = new Customer() { CompanyID = int.Parse(dr["companyID"].ToString()), CompanyName = dr["companyName"].ToString(), BusStyle = dr["busStyle"].ToString(), TaxNumber = dr["taxNumber"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceID = int.Parse(dr["companyProvinceID"].ToString()), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), CompanyType = int.Parse(dr["companyType"].ToString()), CompanyPostalCode = dr["companyPostalCode"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() };
                    MainVM.Suppliers.Add(supplier);
                }
            }
            if (dbCon.IsConnect())
            {
                MainVM.Employees.Clear();
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
                }
                dbCon.Close();
            }
            //MainVM.LoginEmployee_ = MainVM.Employees.Where(x => x.EmpID.Equals(empID)).FirstOrDefault();
            if (dbCon.IsConnect())
            {
                MainVM.Contractor.Clear();
                string query = "SELECT * FROM emp_cont_t a JOIN job_title_t d ON a.jobID = d.jobID WHERE a.isDeleted = 0 AND empType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int empType;
                    int.TryParse(dr["empType"].ToString(), out empType);

                    DateTime dateTo;
                    DateTime.TryParse(dr[11].ToString(), out dateTo);

                    DateTime dateFrom;
                    DateTime.TryParse(dr["empDateFrom"].ToString(), out dateFrom);

                    MainVM.Contractor.Add(new Employee() { EmpID = int.Parse(dr["empID"].ToString()), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(),EmpAddress = dr["empAddress"].ToString() ,JobID = int.Parse(dr["jobID"].ToString()), EmpDateFrom = dateFrom, EmpDateTo = dateTo, EmpUserName = dr["empUserName"].ToString(), EmpType = empType, HasAccess = bool.Parse(dr["hasAccess"].ToString()) });
                }
                dbCon.Close();
            }


            if (dbCon.IsConnect())
            {
                MainVM.AvailedItems.Clear();
                string query = "SELECT * FROM items_availed_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var availedItems = new AvailedItem() { AvailedItemID = int.Parse(dr["id"].ToString()), SqNoChar = dr["sqNoChar"].ToString(), ItemID = int.Parse(dr["itemID"].ToString()), ItemQty = int.Parse(dr["itemQnty"].ToString()), UnitPrice = decimal.Parse(dr["unitPrice"].ToString()) };
                    MainVM.AvailedItems.Add(availedItems);
                }
            }

            if (dbCon.IsConnect())
            {
                MainVM.AvailedServices.Clear();
                string query = "SELECT * FROM services_availed_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var availedservices = new AvailedService() { AvailedServiceID = int.Parse(dr["ID"].ToString()), SqNoChar = dr["sqNoChar"].ToString(), ServiceID = int.Parse(dr["serviceID"].ToString()), ProvinceID = int.Parse(dr["provinceID"].ToString()), Desc = dr["desc"].ToString() ,Address = dr["address"].ToString(), City = dr["city"].ToString(), TotalCost = decimal.Parse(dr["totalCost"].ToString()) };

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
                MainVM.SalesQuotes.Clear();
                string query = "SELECT * FROM sales_quote_t where isDeleted = 0;";
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
                    bool.TryParse(dr["vatIsExcluded"].ToString(),out vatIsExcluded);

                    bool paymentIsLanded = false;
                    bool.TryParse(dr["paymentIsLanded"].ToString(), out paymentIsLanded);

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

                    int fileID;
                    int.TryParse(dr["fileID"].ToString(), out fileID);


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
                        discountPercent_ = discountPerc,
                        fileID_ = fileID,
                         additionalTerms_ = dr["additionalNote"].ToString()
                    });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                MainVM.PurchaseOrder.Clear();
                string query = "SELECT * FROM purchase_order_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int suppID;
                    int.TryParse(dr[1].ToString(), out suppID);

                    DateTime orderDate = new DateTime();
                    DateTime.TryParse(dr[3].ToString(), out orderDate);

                    DateTime poDueDate = new DateTime();
                    DateTime.TryParse(dr[4].ToString(), out poDueDate);

                    bool asapDueDate = false;
                    if (dr[5].ToString().Equals("1"))
                    {
                        asapDueDate = true;
                    }
                    

                    int termsDays;
                    int.TryParse(dr["termsDays"].ToString(), out termsDays);

                    int termsDP;
                    int.TryParse(dr["termsDP"].ToString(), out termsDP);

                    MainVM.PurchaseOrder.Add(new PurchaseOrder()
                    {
                        PONumChar = dr[0].ToString(),
                        suppID = suppID,
                        shipTo = dr[2].ToString(),
                        orderdate = orderDate,
                        POdueDate = poDueDate,
                        asapDueDate = asapDueDate,
                        shipVia = dr[6].ToString(),
                        requisitioner = dr[7].ToString(),
                        incoterms = dr[8].ToString(),
                        POstatus = dr[9].ToString(),
                        currency = dr[10].ToString(),
                        importantNotes = dr[11].ToString(),
                        preparedBy = dr[12].ToString(),
                        approveBy = dr[13].ToString(),
                        refNo = dr[14].ToString(),
                        termsDays = termsDays,
                        termsDp = termsDP

                    });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                MainVM.SalesInvoice.Clear();
                string query = "SELECT * FROM sales_invoice_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.SalesInvoice.Clear();

                
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

                    MainVM.SalesInvoice.Add(new SalesInvoice() { invoiceNo_ = invoiceNo.ToString(), custID_ = custId, sqNoChar_ = dr["sqNoChar"].ToString(),dateOfIssue_ = dateOfIssue, terms_ = termsDays, dueDate_ = dueDate, paymentStatus_ = dr["paymentStatus"].ToString(), vat_ = vat, sc_pwd_discount_ = sc_pwd_discount, withholdingTax_ = withholdingTax, notes_ = dr["notes"].ToString() });



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

                
                MainVM.ServiceSchedules_.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateStarted = new DateTime();
                    DateTime.TryParse(dr["dateStarted"].ToString(), out dateStarted);

                    DateTime dateEnded = new DateTime();
                    DateTime.TryParse(dr["dateEnded"].ToString(), out dateEnded);

                    MainVM.SelectedServiceSchedule_ = (new ServiceSchedule() { ServiceSchedID = int.Parse(dr[0].ToString()),ServiceAvailedID = int.Parse(dr[1].ToString()), serviceStatus_ = dr["serviceStatus"].ToString(), dateStarted_ = dateStarted, dateEnded_ = dateEnded, schedNotes_ = dr["schedNotes"].ToString() });

                    query = "SELECT * FROM assigned_employees_t where serviceSchedId = '"+ int.Parse(dr[0].ToString()) + "';";
                    MySqlDataAdapter dataAdapter2 = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb2 = new DataSet();
                    DataTable fromDbTable2 = new DataTable();
                    dataAdapter2.Fill(fromDb2, "t");
                    fromDbTable2 = fromDb2.Tables["t"];


                    if (fromDbTable2.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in fromDbTable2.Rows)
                        {
                            MainVM.SelectedEmployeeContractor = MainVM.Employees.Where(x => x.EmpID == int.Parse(dr2[2].ToString())).First();
                            if (MainVM.SelectedEmployeeContractor == null)
                                MainVM.SelectedEmployeeContractor = MainVM.Contractor.Where(x => x.EmpID == int.Parse(dr2[2].ToString())).First();

                            MainVM.SelectedServiceSchedule_.assignedEmployees_.Add(MainVM.SelectedEmployeeContractor);
                        }
                    }
                    MainVM.ServiceSchedules_.Add(MainVM.SelectedServiceSchedule_);
                }
                
                dbCon.Close();
            }
        }
    }
}
