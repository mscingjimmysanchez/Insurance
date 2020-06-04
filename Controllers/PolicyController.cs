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
using Insurance.ViewModels;

namespace Insurance.Controllers
{
    public class PolicyController : Controller
    {
        private InsuranceContext db = new InsuranceContext();

        // GET: Policy
        public ActionResult Index()
        {
            return View(db.Policies.ToList());
        }

        // GET: Policy/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Policy policy = db.Policies.Find(id);
            if (policy == null)
            {
                return HttpNotFound();
            }
            return View(policy);
        }

        // GET: Policy/Create
        public ActionResult Create()
        {
            var coveragesList = db.Coverages.ToList();

            var policyViewModel = new PolicyViewModel
            {
                Policy = new Policy { Coverages = new List<Coverage>() },
                Coverages = new List<SelectListItem>(),
                SelectedCoverages = new List<int>(),
            };

            policyViewModel.Coverages = coveragesList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.ID.ToString()
            });

            return View(policyViewModel);
        }

        // POST: Policy/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,ValidityStart,Price,RiskType,Coverages")] PolicyViewModel policy)
        {
            if (ModelState.IsValid)
            {
                db.Policies.Add(policy.Policy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(policy);
        }

        // GET: Policy/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var policyViewModel = new PolicyViewModel
            {
                Policy = db.Policies.Include(i => i.Coverages).First(i => i.ID == id),
            };

            if (policyViewModel.Policy == null)
                return HttpNotFound();

            var coveragesList = db.Coverages.ToList();

            policyViewModel.Coverages = coveragesList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.ID.ToString()
            });

            ViewBag.Client =
                    new SelectList(db.Clients, "Id", "Name", policyViewModel.Policy.Client);

            return View(policyViewModel);
        }

        // POST: Policy/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,ValidityStart,Price,RiskType")] PolicyViewModel policy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(policy.Policy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(policy);
        }

        // GET: Policy/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Policy policy = db.Policies.Find(id);
            if (policy == null)
            {
                return HttpNotFound();
            }
            return View(policy);
        }

        // POST: Policy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Policy policy = db.Policies.Find(id);
            db.Policies.Remove(policy);
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
