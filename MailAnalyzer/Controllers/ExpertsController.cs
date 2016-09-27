using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MailAnalyzer.Models.DBFirst;

namespace MailAnalyzer.Controllers
{
    public class ExpertsController : Controller
    {
        private Entities db = new Entities();

        // GET: Experts
        public ActionResult Index()
        {
            var messages = db.Messages.Distinct().ToList();
            foreach (var item in messages)
            {
                Expert expertSender = new Expert();
                expertSender.FullName = item.SenderName;
                expertSender.MailAddress = item.MailFrom;
                if (!db.Experts.Any(e => e.MailAddress.ToLower().Trim().Equals(expertSender.MailAddress.ToLower().Trim())))
                {
                    db.Experts.Add(expertSender);
                }
                Expert expertReceipt = new Expert();
                expertReceipt.FullName = item.ReceiptName;
                expertReceipt.MailAddress = item.MailTo;
                if (!db.Experts.Any(e => e.MailAddress.ToLower().Trim().Equals(expertReceipt.MailAddress.ToLower().Trim())))
                {
                    db.Experts.Add(expertReceipt);
                }
            }
            db.SaveChanges();
            return View(db.Experts.ToList());
        }

        // GET: Experts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        // GET: Experts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Experts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,MailAddress")] Expert expert)
        {
            if (ModelState.IsValid)
            {
                db.Experts.Add(expert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expert);
        }

        // GET: Experts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        // POST: Experts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,MailAddress")] Expert expert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expert);
        }

        // GET: Experts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        // POST: Experts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expert expert = db.Experts.Find(id);
            db.Experts.Remove(expert);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
