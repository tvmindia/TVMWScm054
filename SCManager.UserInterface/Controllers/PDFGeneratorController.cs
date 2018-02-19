using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace SCManager.UserInterface.Controllers
{
    public class PDFGeneratorController : Controller
    {
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
                    footobj.Header = XMLWorkerHelper.ParseToElementList(pDFToolsObj.Headcontent, null);
                    writer.PageEvent = footobj;
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                }
                string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), "Report.pdf");
                System.IO.File.WriteAllBytes(fname, bytes);
                return JsonConvert.SerializeObject(new { Result = "OK", URL = "./Content/Uploads/Report.pdf" });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message=ex.Message.Replace('\"',' ') });
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
                footerTbl.WriteSelectedRows(0, -1, doc.PageSize.Width-190, 30, writer.DirectContent);

            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                PdfPTable headerTbl = new PdfPTable(1);
                headerTbl.TotalWidth = document.PageSize.Width;
                //headerTbl.HeaderHeight = 60;
                headerTbl.HorizontalAlignment = Element.ALIGN_LEFT;
                float[] widths = new float[] { 100f, document.PageSize.Width - 100 };
                ColumnText ct = new ColumnText(writer.DirectContent);
                ct.SetSimpleColumn(new Rectangle(10, 495, 832, 590));
                foreach (IElement e in Header)
                {
                    ct.AddElement(e);
                }
                ct.Go();

                headerTbl.WriteSelectedRows(0, -1, 0, 490, writer.DirectContent);
            }
        }

    }
}