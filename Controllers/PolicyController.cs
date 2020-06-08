using Insurance.DAL;
using Insurance.Models;
using Insurance.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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

            return View(policyViewModel);
        }

        // GET: Policy/Create
        public ActionResult Create()
        {
            var policyViewModel = new PolicyViewModel();

            LoadCoverages();

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
                if (!ValidateCoverages(policyViewModel))
                    return View(policyViewModel);

                if (!ValidateHighRiskCoverages(policyViewModel))
                    return View(policyViewModel);

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
                }

                db.Policies.Add(policyToAdd);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            LoadCoverages();

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

            LoadCoverages();

            policyViewModel.SelectedCoverages = null;

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
                if (!ValidateCoverages(policyViewModel))
                    return View(policyViewModel);

                if (!ValidateHighRiskCoverages(policyViewModel))
                    return View(policyViewModel);

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

                    db.Entry(policyToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            LoadCoverages();

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

            return View(policyViewModel);
        }

        // POST: Policy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Policy policy = db.Policies.Include(p => p.Coverages).SingleOrDefault(p => p.ID == id);
            db.Policies.Remove(policy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Policy/Assign
        public ActionResult Assign()
        {
            var clientPolicyViewModel = new ClientPolicyViewModel();

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name");

            return View(clientPolicyViewModel);
        }

        // POST: Policy/Assign
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Assign(ClientPolicyViewModel clientPolicyViewModel)
        {            
            if (ModelState.IsValid)
            {
                return RedirectToAction("AssignPolicies", new { id = clientPolicyViewModel.ClientID });
            }

            ViewBag.ClientID = clientPolicyViewModel.ClientID;

            return View(clientPolicyViewModel);
        }

        // GET: Policy/AssignPolicies/5
        public ActionResult AssignPolicies(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var clientPolicyViewModel = new ClientPolicyViewModel
            {
                Client = db.Clients.SingleOrDefault(p => p.ID == id)
            };

            foreach (var policy in db.Policies.Include(i => i.Coverages))
            {
                var assignPolicyViewModel = new AssignPolicyViewModel()
                {
                    ID = policy.ID,
                    Name = policy.Name,
                    Description = policy.Description,
                    ValidityStart = policy.ValidityStart,
                    Price = policy.Price,
                    RiskType = policy.RiskType,
                    Coverages = policy.Coverages,
                    Assigned = false
                };

                clientPolicyViewModel.Policies.Add(assignPolicyViewModel);
            }

            if (clientPolicyViewModel.Client == null)
                return HttpNotFound();

            ViewBag.ClientID = clientPolicyViewModel.Client.ID;

            return View(clientPolicyViewModel);
        }

        // POST: Policy/AssignPolicies
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPolicies(ClientPolicyViewModel clientPolicyViewModel)
        {
            if (clientPolicyViewModel.Client == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                clientPolicyViewModel.ClientID = ViewBag.ClientID;

                var clientToUpdate = db.Clients.Include(i => i.Policies).First(i => i.ID == clientPolicyViewModel.ClientID);

                clientPolicyViewModel.Client = clientToUpdate;

                var selectedPolicies = clientPolicyViewModel.SelectedPolicies;

                foreach (Policy policy in db.Policies)
                {
                    if (!selectedPolicies.Contains(policy.ID))
                    {
                        clientToUpdate.Policies.Remove(policy);
                    }
                    else
                    {
                        clientToUpdate.Policies.Add(policy);
                    }
                }

                db.Entry(clientToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Assign");
            }

            LoadCoverages();

            return View(clientPolicyViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void LoadCoverages()
        {
            ViewBag.Coverages = db.Coverages.ToList().Select(o => new SelectListItem
            {
                Text = o.Name + " - " + o.Percentage + "% - " + o.Period + " months",
                Value = o.ID.ToString()
            });
        }

        private bool ValidateCoverages(PolicyViewModel policyViewModel)
        {
            if (policyViewModel.SelectedCoverages.Count == 0)
            {
                ModelState.AddModelError("CoveragesError", "At least one coverage must be selected");
                LoadCoverages();

                return false;
            }

            return true;
        }

        private bool ValidateHighRiskCoverages(PolicyViewModel policyViewModel)
        {
            if (policyViewModel.Policy.RiskType == RiskType.High)
            {
                foreach (var o in policyViewModel.SelectedCoverages)
                {
                    var coverage = db.Coverages.Find(o);

                    if (coverage.Percentage > 50)
                    {
                        ModelState.AddModelError("HighRiskCoveragesError", "A high risk policy can't have a coverage greater than 50%");
                        LoadCoverages();

                        return false;
                    }
                }
            }

            return true;
        }
    }
}
