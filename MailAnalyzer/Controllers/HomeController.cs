using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailAnalyzer.Models.DBFirst;
using System.Data.Entity.Validation;

namespace MailAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            Message message = new Message();
                            message.MailTo = workSheet.Cells[rowIterator, 1].Value.ToString();
                            message.SenderName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            message.MailFrom = workSheet.Cells[rowIterator, 3].Value.ToString();
                            message.ReceiptName = workSheet.Cells[rowIterator, 4].Value.ToString();
                            message.Subject = workSheet.Cells[rowIterator, 5].Value.ToString();
                            message.Body = workSheet.Cells[rowIterator, 6].Value.ToString();
                            message.MsgDate = (DateTime)(workSheet.Cells[rowIterator, 7].Value);
                            db.Messages.Add(message);
                        }
                        try
                        {
                            db.SaveChanges();
                            ModelState.AddModelError("","Done!!");
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
               
                    }
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}