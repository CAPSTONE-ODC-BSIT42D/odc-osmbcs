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
    class SalesQuoteDocument
    {
        public Document document;
        private Paragraph addressFrame;
        private Paragraph customerName;
        private Paragraph dateFrame;
        private Paragraph paragraph1;
        private Paragraph letterHeaderFrame;
        private Paragraph paragraph2;
        private Table table;
        readonly XPathNavigator navigator;
        public Color TableBorder { get; private set; }
        public Color TableBlue { get; private set; }
        public Color TableGray { get; private set; }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;


        public SalesQuoteDocument()
        {

        }

        public Document CreateDocument(string sqNo, string author)
        {
            this.document = new Document();
            this.document.Info.Title = "Sales Quote "+sqNo;
            this.document.Info.Subject = "Sales Quote " + sqNo;
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
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Suite 3A Amparo Garden Corperate Bldg., No. 2116 Amparo cor Felix St. Sta Ana, Manila, 1009 Philippines, Tel No: 742 - 4199 / 566 - 3153, email: o.danny@odcphils.com");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address
            this.dateFrame = section.AddParagraph();
            this.customerName = section.AddParagraph();
            this.addressFrame = section.AddParagraph();
            this.letterHeaderFrame = section.AddParagraph();
            this.paragraph1 = section.AddParagraph();
            
            // Put sender in address frame
            DateTime timeToday = new DateTime();
            timeToday = DateTime.Today;
            dateFrame.AddFormattedText(timeToday.ToLongDateString());
            dateFrame.Format.Font.Name = "Calibri";
            dateFrame.Format.Font.Size = 11;
            dateFrame.Format.SpaceAfter = "1.0cm";
            customerName.AddFormattedText(MainVM.SelectedCustomerSupplier.CompanyName);
            customerName.Format.Font.Name = "Calibri";
            customerName.Format.Font.Size = 11;
            customerName.Format.Font.Bold = true;
            addressFrame.AddFormattedText(MainVM.SelectedCustomerSupplier.CompanyAddress +"\n"+ MainVM.SelectedCustomerSupplier.CompanyCity + "\n" + MainVM.SelectedCustomerSupplier.CompanyProvinceName);
            addressFrame.Format.Font.Name = "Calibri";
            addressFrame.Format.Font.Size = 11;
            addressFrame.Format.SpaceAfter = "1.0cm";
            letterHeaderFrame.AddFormattedText("Attention: ", TextFormat.Bold);
            letterHeaderFrame.AddFormattedText(""+MainVM.SelectedRepresentative.RepFullName);
            letterHeaderFrame.AddLineBreak();
            letterHeaderFrame.AddFormattedText("Subject: ", TextFormat.Bold);
            letterHeaderFrame.AddFormattedText(""+MainVM.SelectedSalesQuote.sqNoChar_);
            addressFrame.Format.Font.Name = "Calibri";
            addressFrame.Format.Font.Size = 11;
            addressFrame.Format.SpaceAfter = "1.0cm";
            paragraph1.AddText("Dear ");
            paragraph1.AddFormattedText(""+MainVM.SelectedRepresentative.RepTitle+" "+ MainVM.SelectedRepresentative.RepLastName, TextFormat.Bold);
            paragraph1.AddLineBreak();
            paragraph1.AddText("As per your request, we are formally offering our price proposal for the above subject as follows:");
            addressFrame.Format.Font.Name = "Calibri";
            addressFrame.Format.Font.Size = 11;
            addressFrame.Format.SpaceAfter = "1.0cm";


            // Create the item table

            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = this.table.AddColumn("1cm");
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
            Row row = table.AddRow();
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


            this.table.SetEdge(0, 0, 7, 1, Edge.Box, BorderStyle.Single, 0.25, Color.Empty);
            
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
            row = table.AddRow();
            row.Cells[0].AddParagraph("Total Price");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 5;
            row.Cells[6].AddParagraph("" + totalPrice.ToString());
            //
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
            


            // Create the header of the table
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[0].AddParagraph("TERMS AND CONDITIONS:");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].MergeRight = 1;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            row = table.AddRow();
            row.Cells[0].AddParagraph("PRICE: ");
            row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.priceNote_);
            row = table.AddRow();
            row.Cells[0].AddParagraph("PAYMENT: ");
            row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.termsDP_+"% DP upon release of order and "+ (MainVM.SelectedSalesQuote.termsDP_-100)+"% Balacne upon delivery");
            row = table.AddRow();
            row.Cells[0].AddParagraph("DELIVERY: ");
            if (MainVM.SelectedSalesQuote.estDelivery_ != 0)
            {
                row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.estDelivery_ + " days upon receipt of DP");
            }
            else
                row.Cells[1].AddParagraph("None");
            row = table.AddRow();
            row.Cells[0].AddParagraph("WARRANTY: ");
            if (MainVM.SelectedSalesQuote.warrantyDays_ != 0)
            {
                row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.warrantyDays_ + " year from the date of delivery");
            }
            else
                row.Cells[1].AddParagraph("None");

            row = table.AddRow();
            row.Cells[0].AddParagraph("VALIDITY: ");
            row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.validityDays_ + " days (price increase will take effect after validity period)");

            row = table.AddRow();
            row.Cells[0].AddParagraph("PENALTY: ");
            row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.penaltyPercent_ + "% of balance");

            row = table.AddRow();
            row.Cells[0].AddParagraph("ADDITIONAL TERMS: ");
            row.Cells[1].AddParagraph(MainVM.SelectedSalesQuote.additionalTerms_);

            
            this.table.SetEdge(0, 0, 2, 1, Edge.Box, BorderStyle.None, 0.25, Color.Empty);


            this.paragraph2 = section.AddParagraph();
            paragraph2.AddLineBreak();
            paragraph2.AddLineBreak();
            paragraph2.AddLineBreak();
            paragraph2.AddText("Thank you very much and we are looking forward for your valued order soon.");
            paragraph2.AddLineBreak();
            paragraph2.AddText("Very Truly Yours,");
            paragraph2.AddLineBreak();
            paragraph2.AddLineBreak();
            paragraph2.AddLineBreak();
            paragraph2.AddFormattedText("DANNY M. ORCENA", TextFormat.Bold);
            paragraph2.AddLineBreak();
            paragraph2.AddFormattedText("Chief Marketing Officer - Director", TextFormat.Italic);
        }
        void FillContent()
        {
            
        }
        /// <summary>
        /// Selects a subtree in the XML data.
        /// </summary>
        XPathNavigator SelectItem(string path)
        {
            XPathNodeIterator iter = this.navigator.Select(path);
            iter.MoveNext();
            return iter.Current;
        }

        /// <summary>
        /// Gets an element value from the XML data.
        /// </summary>
        static string GetValue(XPathNavigator nav, string name)
        {
            //nav = nav.Clone();
            XPathNodeIterator iter = nav.Select(name);
            iter.MoveNext();
            return iter.Current.Value;
        }

        /// <summary>
        /// Gets an element value as double from the XML data.
        /// </summary>
        static double GetValueAsDouble(XPathNavigator nav, string name)
        {
            try
            {
                string value = GetValue(nav, name);
                if (value.Length == 0)
                    return 0;
                return Double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
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
            
            //var assembly = GetType().Assembly;
            //var fullResourceName = string.Concat(assembly.GetName().Name, ".", name);
            //using (var stream = assembly.GetManifestResourceStream(fullResourceName))
            //{
            //    var buffer = new byte[stream.Length];
            //    stream.Read(buffer, 0, (int)stream.Length);
            //    return buffer;
            //}
        }

    }
}
