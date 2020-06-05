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

            var policyViewModel = new PolicyViewModel
            {
                Policy = db.Policies.Include(i => i.Coverages).First(i => i.ID == id),
            };

            if (policyViewModel.Policy == null)
                return HttpNotFound();

            var coveragesList = db.Coverages.ToList();

            policyViewModel.Coverages = coveragesList.Select(o => new SelectListItem
            {
                Text = o.Name + " - " + o.Percentage + "% - " + o.Period + " months",
                Value = o.ID.ToString()
            });

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", policyViewModel.Policy.Client.ID);

            return View(policyViewModel);
        }

        // GET: Policy/Create
        public ActionResult Create()
        {
            var policyViewModel = new PolicyViewModel();
            var coveragesList = db.Coverages.ToList();

            ViewBag.Coverages = coveragesList.Select(o => new SelectListItem
            {
                Text = o.Name + " - " + o.Percentage + "% - " + o.Period + " months",
                Value = o.ID.ToString()
            });

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name");

            return View();
        }

        // POST: Policy/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyViewModel policyViewModel)
        {
            if (ModelState.IsValid)
            {
                var policyToAdd = db.Policies.Include(i => i.Coverages).First();

                if (TryUpdateModel(policyToAdd, "Policy", new string[] { "ID", "Name", "Description", "ValidityStart", "Price", "RiskType" }))
                {
                    var updatedPolicies = new HashSet<int>(policyViewModel.SelectedCoverages);

                    foreach (Coverage coverage in db.Coverages)
                    {
                        if (!updatedPolicies.Contains(coverage.ID))
                        {
                            policyToAdd.Coverages.Remove(coverage);
                        }
                        else
                        {
                            policyToAdd.Coverages.Add((coverage));
                        }
                    }

                    policyToAdd.Client = db.Clients.FirstOrDefault(p => p.ID == policyViewModel.Policy.Client.ID);
                }

                db.Policies.Add(policyToAdd);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name");

            return View(policyViewModel);
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
                Text = o.Name + " - " + o.Percentage + "% - " + o.Period + " months",
                Value = o.ID.ToString()
            });

            policyViewModel.SelectedCoverages = null;

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", policyViewModel.Policy.Client.ID);

            return View(policyViewModel);
        }

        // POST: Policy/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyViewModel policyViewModel)
        {
            if (policyViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                var policyToUpdate = db.Policies.Include(i => i.Coverages).First(i => i.ID == policyViewModel.Policy.ID);

                if (TryUpdateModel(policyToUpdate, "Policy", new string[] { "ID", "Name", "Description", "ValidityStart", "Price", "RiskType" }))
                {
                    var newCoverages = db.Coverages.Where(m => policyViewModel.SelectedCoverages.Contains(m.ID)).ToList();
                    var updatedCoverages = new HashSet<int>(policyViewModel.SelectedCoverages);
                    
                    foreach (Coverage coverage in db.Coverages)
                    {
                        if (!updatedCoverages.Contains(coverage.ID))
                            policyToUpdate.Coverages.Remove(coverage);
                        else
                            policyToUpdate.Coverages.Add((coverage));
                    }

                    policyToUpdate.Client = db.Clients.FirstOrDefault(p => p.ID == policyViewModel.Policy.Client.ID);

                    db.Entry(policyToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(policyViewModel);
        }

        // GET: Policy/Delete/5
        public ActionResult Delete(int? id)
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
                Text = o.Name + " - " + o.Percentage + "% - " + o.Period + " months",
                Value = o.ID.ToString()
            });

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", policyViewModel.Policy.Client.ID);

            return View(policyViewModel);
        }

        // POST: Policy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Policy policy = db.Policies.Include(p => p.Coverages).Include(p => p.Client).SingleOrDefault(p => p.ID == id);
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
