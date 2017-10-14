using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using MigraDoc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prototype2.Properties;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System.Reflection;
using System.IO;
using System.Xml.XPath;
using System.Diagnostics;
using System.Globalization;
using System.Windows;


namespace prototype2
{
    class InvoiceDocument
    {
        public InvoiceDocument()
        {

        }
        public Document document;

        private Paragraph headerName;

        private Paragraph customerName;
        private Paragraph date;
        private Paragraph tinNumber;
        private Paragraph terms;
        private Paragraph addressFrame;
        private Paragraph busStyle;
        private Paragraph signature;
        
        private Paragraph paragraph1;
        private Paragraph letterHeaderFrame;
        private Paragraph paragraph2;
        private Table table;
        readonly XPathNavigator navigator;
        public Color TableBorder { get; private set; }
        public Color TableBlue { get; private set; }
        public Color TableGray { get; private set; }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public Document CreateDocument(string sqNo, string author)
        {
            this.document = new Document();
            this.document.Info.Title = "Invoice " + sqNo;
            this.document.Info.Subject = "Sales Invoice " + sqNo;
            this.document.Info.Author = author;
            this.document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            DefineStyles();
            CreatePage();
            FillContent();
            return document;
        }

        void DefineStyles()
        {
            // Get the predefined style Normal.
            MigraDoc.DocumentObjectModel.Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Calibri";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Calibri";
            style.Font.Name = "Calibri";
            style.Font.Size = 11;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            Section section = this.document.AddSection();

            // Put a logo in the header
            byte[] imagea = LoadImage("prototype2.Resources.odclogo.png");
            string imageFilename = MigraDocFilenameFromByteArray(imagea);
            Image image = section.Headers.Primary.AddImage(imageFilename);
            image.Height = "1cm";
            image.LockAspectRatio = true;
            image.Top = ShapePosition.Center;
            image.Left = ShapePosition.Center;
            image.WrapFormat.Style = WrapStyle.TopBottom;
            Paragraph paragraph = section.Headers.Primary.AddParagraph("Suite 3A Amparo Garden Corperate Bldg., No. 2116 Amparo cor Felix St. Sta Ana, Manila, 1009 Philippines, Tel No: 742 - 4199 / 566 - 3153, email: o.danny@odcphils.com");
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            
            headerName = section.AddParagraph();
            headerName.Format.SpaceBefore = "1.0cm";
            headerName.AddFormattedText("SALES INVOICE");
            headerName.Format.Font.Name = "Calibri";
            headerName.Format.Font.Bold = true;
            headerName.Format.Font.Size = 15;
            headerName.Format.SpaceAfter = "1.0cm";
            headerName.Format.Alignment = ParagraphAlignment.Left;


            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0;
            table.Rows.Alignment = RowAlignment.Right;
            

            Column column = table.AddColumn("5cm");

            Row row = table.AddRow();
            row.Cells[0].AddParagraph("No.: "+MainVM.SalesInvoice.Count + 1);

            DateTime dateToday = new DateTime();
            dateToday = DateTime.Now;

            row = table.AddRow();
            row.Cells[0].AddParagraph("Date Issued" + dateToday.ToLongDateString());

            
            date = section.AddParagraph();

            customerName = section.AddParagraph();
            addressFrame = section.AddParagraph();

            tinNumber = section.AddParagraph();
            
            
            terms = section.AddParagraph();
            
            busStyle = section.AddParagraph();
            signature = section.AddParagraph();

            this.paragraph1 = section.AddParagraph();
            

            customerName.AddFormattedText(MainVM.SelectedCustomerSupplier.CompanyName);
            customerName.Format.Font.Name = "Calibri";
            customerName.Format.Font.Size = 11;
            customerName.Format.Font.Bold = true;
            addressFrame.AddFormattedText(MainVM.SelectedCustomerSupplier.CompanyAddress + "\n" + MainVM.SelectedCustomerSupplier.CompanyCity + "\n" + MainVM.SelectedCustomerSupplier.CompanyProvinceName);
            addressFrame.Format.Font.Name = "Calibri";
            addressFrame.Format.Font.Size = 11;

            date.AddText("DATE ISSSUED: ");
            date.Format.Font.Name = "Calibri";
            date.Format.Font.Size = 11;
            
            date.Format.Font.Name = "Calibri";
            date.Format.Font.Size = 11;
            date.Format.Font.Bold = true;

            tinNumber.AddText("TIN: ");
            tinNumber.Format.Font.Name = "Calibri";
            tinNumber.Format.Font.Size = 11;
            tinNumber.AddFormattedText(MainVM.SelectedSalesInvoice.tin_);
            tinNumber.Format.Font.Name = "Calibri";
            tinNumber.Format.Font.Size = 11;
            tinNumber.Format.Font.Bold = true;

            terms.AddText("TERMS: ");
            terms.Format.Font.Name = "Calibri";
            terms.Format.Font.Size = 11;
            terms.AddFormattedText(MainVM.SelectedSalesInvoice.terms_.ToString()+" days");
            terms.Format.Font.Name = "Calibri";
            terms.Format.Font.Size = 11;
            terms.Format.Font.Bold = true;

            

            busStyle.AddText("BUS STYLE: ");
            busStyle.AddFormattedText(MainVM.SelectedSalesInvoice.busStyle_);
            busStyle.Format.Font.Name = "Calibri";
            busStyle.Format.Font.Size = 11;
            busStyle.Format.Font.Bold = true;
            busStyle.Format.SpaceAfter = "1.0cm";

            signature.AddFormattedText("SIGNATURE: ");
            signature.Format.Font.Name = "Calibri";
            signature.Format.Font.Size = 11;
            signature.Format.SpaceAfter = "1.0cm";


            // Create the item table

            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            column = this.table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;


            // Create the header of the table
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[0].AddParagraph("Line");
            row.Cells[0].AddParagraph("No.");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            row.Cells[1].AddParagraph("Item");
            row.Cells[1].AddParagraph("Name");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].AddParagraph("Description");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            row.Cells[3].AddParagraph("Unit");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            row.Cells[4].AddParagraph("Qty");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            row.Cells[5].AddParagraph("Unit Price");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[5].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            row.Cells[6].AddParagraph("Total Amount");
            row.Cells[6].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[6].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;

            decimal totalPrice = 0;
            foreach (RequestedItem item in MainVM.RequestedItems)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(item.lineNo);
                row.Cells[1].AddParagraph(item.itemName);
                row.Cells[2].AddParagraph(item.desc);
                row.Cells[3].AddParagraph("na");
                row.Cells[4].AddParagraph(item.qty.ToString());
                row.Cells[5].AddParagraph(item.unitPriceMarkUp.ToString());
                row.Cells[6].AddParagraph(item.totalAmountMarkUp.ToString());
                totalPrice += item.totalAmountMarkUp;
            }
            //
            this.table.SetEdge(0, 0, 2, 1, Edge.Box, BorderStyle.None, 0.25, Color.Empty);
            var filler = section.AddParagraph();
            filler.AddLineBreak();
            // Create the item table

            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            row = table.AddRow();
            row.Cells[0].AddParagraph("");
            row.Cells[0].AddParagraph("");
            row.Cells[1].AddParagraph("Total Sale VAT Inclusive");
            row.Cells[1].AddParagraph(MainVM.TotalSales_.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("");
            row.Cells[0].AddParagraph("");
            row.Cells[1].AddParagraph("Less: VAT");
            row.Cells[1].AddParagraph(MainVM.VatAmount_.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("");
            row.Cells[0].AddParagraph("");
            row.Cells[1].AddParagraph("TOTAl ");
            row.Cells[1].AddParagraph(MainVM.TotalSalesNoVat_.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("Vatable Sales");
            row.Cells[0].AddParagraph(MainVM.VatableSale.ToString());
            row.Cells[1].AddParagraph("Less: SC/PWD Discount: ");
            row.Cells[1].AddParagraph("");

            row = table.AddRow();
            row.Cells[0].AddParagraph("Zero - Rated Sales");
            row.Cells[0].AddParagraph();
            row.Cells[1].AddParagraph("Less: Withholding Tax");
            row.Cells[1].AddParagraph("");

            row = table.AddRow();
            row.Cells[0].AddParagraph("VAT Amount");
            row.Cells[0].AddParagraph(MainVM.VatAmount.ToString());
            row.Cells[1].AddParagraph("Add: VAT");
            row.Cells[1].AddParagraph(MainVM.VatAmount.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("TOTAL SALES");
            row.Cells[0].AddParagraph(MainVM.TotalSales.ToString());
            row.Cells[1].AddParagraph("AMOUNT DUE");
            row.Cells[1].AddParagraph(MainVM.TotalDue.ToString());

            this.table.SetEdge(0, 0, 2, 1, Edge.Box, BorderStyle.Single, 0.25, Color.Empty);


            this.paragraph2 = section.AddParagraph();
            paragraph2.Format.Alignment = ParagraphAlignment.Right;
            paragraph2.AddLineBreak();
            paragraph2.AddLineBreak();
            paragraph2.AddLineBreak();
            paragraph2.AddFormattedText("_________________________________", TextFormat.Bold);
            paragraph2.AddLineBreak();
            paragraph2.AddFormattedText("Authorized Representative", TextFormat.Italic);
        }
        void FillContent()
        {

        }
        
        private string MigraDocFilenameFromByteArray(byte[] image)
        {
            return "base64:" + Convert.ToBase64String(image);
        }

        byte[] LoadImage(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var allRessources = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }


}
