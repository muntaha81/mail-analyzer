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
    public class SubDomainsController : Controller
    {
        private Entities db = new Entities();

        // GET: SubDomains
        public ActionResult Index()
        {
            var subDomains = db.SubDomains.Include(s => s.Domain);
            return View(subDomains.ToList());
        }

        // GET: SubDomains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDomain subDomain = db.SubDomains.Find(id);
            if (subDomain == null)
            {
                return HttpNotFound();
            }
            return View(subDomain);
        }

        // GET: SubDomains/Create
        public ActionResult Create()
        {
            ViewBag.DomainId = new SelectList(db.Domains, "Id", "code");
            return View();
        }

        // POST: SubDomains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,code,Name,DomainId")] SubDomain subDomain)
        {
            if (ModelState.IsValid)
            {
                db.SubDomains.Add(subDomain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DomainId = new SelectList(db.Domains, "Id", "code", subDomain.DomainId);
            return View(subDomain);
        }

        // GET: SubDomains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDomain subDomain = db.SubDomains.Find(id);
            if (subDomain == null)
            {
                return HttpNotFound();
            }
            ViewBag.DomainId = new SelectList(db.Domains, "Id", "code", subDomain.DomainId);
            return View(subDomain);
        }

        // POST: SubDomains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,code,Name,DomainId")] SubDomain subDomain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subDomain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DomainId = new SelectList(db.Domains, "Id", "code", subDomain.DomainId);
            return View(subDomain);
        }

        // GET: SubDomains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDomain subDomain = db.SubDomains.Find(id);
            if (subDomain == null)
            {
                return HttpNotFound();
            }
            return View(subDomain);
        }

        // POST: SubDomains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubDomain subDomain = db.SubDomains.Find(id);
            db.SubDomains.Remove(subDomain);
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
