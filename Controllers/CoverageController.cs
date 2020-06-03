using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Insurance.DAL;
using Insurance.Models;

namespace Insurance.Controllers
{
    public class CoverageController : Controller
    {
        private InsuranceContext db = new InsuranceContext();

        // GET: Coverage
        public ActionResult Index()
        {
            return View(db.Coverages.ToList());
        }

        // GET: Coverage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coverage coverage = db.Coverages.Find(id);
            if (coverage == null)
            {
                return HttpNotFound();
            }
            return View(coverage);
        }

        // GET: Coverage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coverage/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Percentage,Period")] Coverage coverage)
        {
            if (ModelState.IsValid)
            {
                db.Coverages.Add(coverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coverage);
        }

        // GET: Coverage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coverage coverage = db.Coverages.Find(id);
            if (coverage == null)
            {
                return HttpNotFound();
            }
            return View(coverage);
        }

        // POST: Coverage/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Percentage,Period")] Coverage coverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coverage);
        }

        // GET: Coverage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coverage coverage = db.Coverages.Find(id);
            if (coverage == null)
            {
                return HttpNotFound();
            }
            return View(coverage);
        }

        // POST: Coverage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coverage coverage = db.Coverages.Find(id);
            db.Coverages.Remove(coverage);
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
