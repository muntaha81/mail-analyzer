using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MailAnalyzer.Models.DBFirst;
using System.Text.RegularExpressions;

namespace MailAnalyzer.Controllers
{
    public class AnalyticsController : Controller
    {
        private Entities db = new Entities();

        // GET: Analytics
        public ActionResult Index()
        {
            foreach (var item in db.Analytics)
            {
                db.Analytics.Remove(item);
            }
            db.SaveChanges();
            foreach (var subDomain in db.SubDomains)
            {
                foreach (var expert in db.Experts)
                {
                    var messages = db.Messages.Where(m => m.MailFrom.Trim().Equals(expert.MailAddress.Trim()) || m.MailTo.Trim().Equals(expert.MailAddress.Trim())).ToList();

                    foreach (var message in messages.Where(m => m.Body.ToLower().Contains(subDomain.Name.ToLower())))
                    {
                        int numberOfOccurrence = Regex.Matches(message.Body, subDomain.Name.ToLower()).Count;
                        Analytic analytic = new Analytic();
                        analytic.Expert = expert;
                        analytic.Keyword = subDomain.Name;
                        analytic.Domain = subDomain.Domain;
                        analytic.NumberOfOccurrence = numberOfOccurrence;
                        db.Analytics.Add(analytic);
                    }
                }
            }
            db.SaveChanges();
            var analytics = db.Analytics.Include(a => a.Expert).Include(a => a.Domain).OrderBy(a => a.ExpertId);

            return View(analytics.ToList());
        }
        // GET: Analytics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Analytic analytic = db.Analytics.Find(id);
            if (analytic == null)
            {
                return HttpNotFound();
            }
            return View(analytic);
        }

        // GET: Analytics/Create
        public ActionResult Create()
        {
            ViewBag.MessageId = new SelectList(db.Messages, "Id", "MailFrom");
            ViewBag.DomainId = new SelectList(db.Domains, "Id", "code");
            return View();
        }
        // POST: Analytics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // GET: Analytics/Edit/5

        public ActionResult Score(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Analytic analytic = db.Analytics.Find(id);
            if (analytic == null)
            {
                return HttpNotFound();
            }
            return View(analytic);
        }

        // POST: Analytics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // GET: Analytics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Analytic analytic = db.Analytics.Find(id);
            if (analytic == null)
            {
                return HttpNotFound();
            }
            return View(analytic);
        }

        // POST: Analytics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Analytic analytic = db.Analytics.Find(id);
            db.Analytics.Remove(analytic);
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
