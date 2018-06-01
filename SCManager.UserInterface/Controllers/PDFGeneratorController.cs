using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net.Mail;
using System.Net;
using System;
using UserInterface.Models;
using iTextSharp.text.pdf.draw;
using Newtonsoft.Json;
using iTextSharp.tool.xml.pipeline;

namespace SCManager.UserInterface.Controllers
{
    public class PDFGeneratorController : Controller
    {

        //public static string FONT = "resources/fonts/Cardo-Regular.ttf";

        // GET: PDFGenerator
        public ActionResult Index()
        {
            return View();
        }

        public string PrintPDF(PDFTools pDFToolsObj)
        {
            try
            {
                string sw = pDFToolsObj.Content.Replace("<br>", "<br/>").ToString();
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 55f, 30f);
                byte[] bytes = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    Footer footobj = new Footer();
                    footobj.imageURL = Server.MapPath("~/Content/images/SCManager.png");
                    footobj.Header = XMLWorkerHelper.ParseToElementList(pDFToolsObj.Headcontent ==null?"": pDFToolsObj.Headcontent, null);
                    writer.PageEvent = footobj;
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                }
                string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), "Report.pdf");
                System.IO.File.WriteAllBytes(fname, bytes);
                return JsonConvert.SerializeObject(new { Result = "OK", URL = "../Content/Uploads/Report.pdf" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message.Replace('\"', ' ') });
            }


        }
        public partial class Footer : PdfPageEventHelper

        {
            public string imageURL { get; set; }
            public string Tableheader { get; set; }
            public ElementList Header;
            public override void OnEndPage(PdfWriter writer, Document doc)

            {
                Paragraph footer = new Paragraph("Date: " + DateTime.Now.ToString("dd-MMM-yyyy h:mm tt"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.ITALIC));
                footer.Alignment = Element.ALIGN_RIGHT;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = doc.PageSize.Width;
                footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.PaddingLeft = 10;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, doc.PageSize.Width - 190, 30, writer.DirectContent);               
                //footerTbl.WriteSelectedRows(0, -1, 250, 30, writer.DirectContent);
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {      


                Font documentFont = FontFactory.GetFont(FontFactory.TIMES, 14, iTextSharp.text.Font.BOLD);

                string documentName = Tableheader;
                Chunk chunkHeader = new Chunk(documentName);
                //chunkHeader.SetUnderline(1f, -6f);
                //Add Chunk to paragraph
                Paragraph header = new Paragraph(chunkHeader);
                Chunk linebreak = new Chunk(new LineSeparator(1f, document.PageSize.Width, BaseColor.BLACK, Element.ALIGN_CENTER, -3f));
                header.Add(linebreak);
                Phrase phraseDocumentName = new Phrase(documentName, documentFont);
                header.Add(phraseDocumentName);
                header.Alignment = Element.ALIGN_LEFT;
                PdfPTable headerTbl = new PdfPTable(2);
                headerTbl.TotalWidth = document.PageSize.Width;
                //headerTbl.HeaderHeight = 60;
                headerTbl.HorizontalAlignment = Element.ALIGN_LEFT;
                float[] widths = new float[] { 180f, document.PageSize.Width - 100 };
                headerTbl.SetWidths(widths);


                PdfPCell cell1 = new PdfPCell(header);
                cell1.Border = 0;
                cell1.PaddingLeft = 50;
                cell1.PaddingTop = 40;
                //cell1.Width = document.PageSize.Width - 90;
                headerTbl.AddCell(cell1);
                ColumnText ct = new ColumnText(writer.DirectContent);
                ct.SetSimpleColumn(new Rectangle(10, 790, 559, 600));
                foreach (IElement e in Header)
                {
                    ct.AddElement(e);
                }
                ct.Go();

                headerTbl.WriteSelectedRows(0, -1, 0, 832, writer.DirectContent);
            }
        }


            public FileResult Download(PDFTools pDFToolsObj)
            {
                //jpg.Alignment = Element.ALIGN_LEFT;  

                //BaseFont bf = BaseFont.CreateFont("/fonts/fontawesome-webfont.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //Font f = new Font(bf, 12);
                //Chunk chunkRupee = new Chunk(" \u20B9");
                //string htmlBody = pDFToolsObj.Content.Replace("<br>", "<br/>").ToString().Replace("CurrencySymbol", chunkRupee.ToString()).ToString();
                string htmlBody = pDFToolsObj.Content.Replace("<br>", "<br/>").ToString();
                StringReader reader = new StringReader(htmlBody.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 85f, 30f);
                byte[] bytes = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    Footer footobj = new Footer();
                    footobj.imageURL = Server.MapPath("~/Content/images/logo.png");
                    footobj.Header = XMLWorkerHelper.ParseToElementList(pDFToolsObj.Headcontent == null ? "" : pDFToolsObj.Headcontent, null);
                    footobj.Tableheader = pDFToolsObj.HeaderText;
                    writer.PageEvent = footobj;

                    // Our custom Header and Footer is done using Event Handler
                    //TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter();
                    //writer.PageEvent = PageEventHandler;
                    //// Define the page header
                    //PageEventHandler.Title = "Column Header";
                    //PageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
                    //PageEventHandler.HeaderLeft = "Group";
                    //PageEventHandler.HeaderRight = "1";
                    pdfDoc.Open();
                    //jpg.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top - 60);
                    //pdfDoc.Add(jpg);
                    PdfContentByte cb = writer.DirectContent;
                    //cb.MoveTo(pdfDoc.Left, pdfDoc.Top - 60);
                    //cb.LineTo(pdfDoc.Right, pdfDoc.Top - 60);
                    //cb.SetLineWidth(1);
                    //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                    //cb.Stroke();
                    cb.MoveTo(pdfDoc.Left, pdfDoc.Top - 12);
                    cb.LineTo(pdfDoc.Right, pdfDoc.Top - 12);
                    cb.SetLineWidth(1);
                    cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                    cb.Stroke();

                //Paragraph welcomeParagraph = new Paragraph("Hello, World!");
                // Our custom Header and Footer is done using Event Handler

                //pdfDoc.Add(welcomeParagraph);

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    //for (int i = 0; i <= 2; i++)
                    //{
                    //    // Define the page header
                    //    PageEventHandler.HeaderRight = i.ToString();
                    //    if (i != 1)
                    //    {
                    //        pdfDoc.NewPage();
                    //    }
                    //}
                    pdfDoc.Close();
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                }
                string contentFileName = pDFToolsObj.ContentFileName.ToString() == null ? "Report.pdf" : (pDFToolsObj.ContentFileName.ToString() + " - " + pDFToolsObj.CustomerName.ToString() + ".pdf");
                string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), contentFileName);
                System.IO.File.WriteAllBytes(fname, bytes);
                string contentType = "application/pdf";
                //Parameters to file are
                //1. The File Path on the File Server
                //2. The content type MIME type
                //3. The parameter for the file save by the browser
                return File(fname, contentType, contentFileName);
            }


        }
    }
