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
    /// <summary>
    /// Policy controller.
    /// </summary>
    public class PolicyController : Controller
    {
        /// <summary>
        /// Insurance context.
        /// </summary>
        private InsuranceContext db = new InsuranceContext();

        // GET: Policy
        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Policies.ToList());
        }

        // GET: Policy/Details/5
        /// <summary>
        /// Details action.
        /// </summary>
        /// <param name="id">Policy id.</param>
        /// <returns>View with policy details.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var policyViewModel = new PolicyViewModel
            {
                Policy = db.Policies.Include(i => i.Coverages).First(i => i.ID == id),
            };

            if (policyViewModel.Policy == null)
                return HttpNotFound();

            return View(policyViewModel);
        }

        // GET: Policy/Create
        /// <summary>
        /// Create a policy.
        /// </summary>
        /// <returns>Policy view.</returns>
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
        /// <summary>
        /// Create a policy.
        /// </summary>
        /// <param name="policyViewModel">Policy view model.</param>
        /// <returns>Policy view model.</returns>
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
                    var selectedCoverages = new HashSet<int>(policyViewModel.SelectedCoverages);

                    foreach (Coverage coverage in db.Coverages)
                        if (!selectedCoverages.Contains(coverage.ID))
                            policyToAdd.Coverages.Remove(coverage);
                        else
                            policyToAdd.Coverages.Add((coverage));
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
        /// <summary>
        /// Edit a policy.
        /// </summary>
        /// <param name="id">Policy id.</param>
        /// <returns>Policy view model.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
        /// <summary>
        /// Edit a policy.
        /// </summary>
        /// <param name="policyViewModel">Policy view model.</param>
        /// <returns>Policy view model.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicyViewModel policyViewModel)
        {
            if (policyViewModel == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                if (!ValidateCoverages(policyViewModel))
                    return View(policyViewModel);

                if (!ValidateHighRiskCoverages(policyViewModel))
                    return View(policyViewModel);

                var policyToUpdate = db.Policies.Include(i => i.Coverages).First(i => i.ID == policyViewModel.Policy.ID);

                if (TryUpdateModel(policyToUpdate, "Policy", new string[] { "ID", "Name", "Description", "ValidityStart", "Price", "RiskType" }))
                {
                    var selectedCoverages = new HashSet<int>(policyViewModel.SelectedCoverages);
                    
                    foreach (Coverage coverage in db.Coverages)
                        if (!selectedCoverages.Contains(coverage.ID))
                            policyToUpdate.Coverages.Remove(coverage);
                        else
                            policyToUpdate.Coverages.Add((coverage));

                    db.Entry(policyToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            LoadCoverages();

            return View(policyViewModel);
        }

        // GET: Policy/Delete/5
        /// <summary>
        /// Delete policy.
        /// </summary>
        /// <param name="id">Policy id.</param>
        /// <returns>Policy view model.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var policyViewModel = new PolicyViewModel
            {
                Policy = db.Policies.Include(i => i.Coverages).First(i => i.ID == id),
            };

            if (policyViewModel.Policy == null)
                return HttpNotFound();

            return View(policyViewModel);
        }

        // POST: Policy/Delete/5
        /// <summary>
        /// Confirm the deletion of a policy.
        /// </summary>
        /// <param name="id">Policy id.</param>
        /// <returns>Action index.</returns>
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
        /// <summary>
        /// Assign the policy to a client.
        /// </summary>
        /// <returns>Client policy view model.</returns>
        public ActionResult Assign()
        {
            var clientPolicyViewModel = new ClientPolicyViewModel();

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name");

            return View(clientPolicyViewModel);
        }

        // POST: Policy/Assign
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Assign the policy to a client.
        /// </summary>
        /// <param name="clientPolicyViewModel">Client policy view model.</param>
        /// <returns>Client policy view model.</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Assign(ClientPolicyViewModel clientPolicyViewModel)
        {            
            if (ModelState.IsValid)
                return RedirectToAction("ListPolicies", new { id = clientPolicyViewModel.ClientID });

            ViewBag.ClientID = clientPolicyViewModel.ClientID;

            return View(clientPolicyViewModel);
        }

        // GET: Policy/ListPolicies/5
        /// <summary>
        /// List the policies to assign to a client.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Client policy view model.</returns>
        public ActionResult ListPolicies(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
                    Assigned = policy.Clients.Count > 0
                };

                clientPolicyViewModel.Policies.Add(assignPolicyViewModel);
            }

            if (clientPolicyViewModel.Client == null)
                return HttpNotFound();

            Session["clientPolicyViewModel"] = clientPolicyViewModel;

            return View(clientPolicyViewModel);
        }

        // GET: Policy/AssignPolicy/5
        /// <summary>
        /// Assign a policy to a defined client.
        /// </summary>
        /// <param name="id">Policy id.</param>
        /// <returns>Client policy view model.</returns>
        public ActionResult AssignPolicy(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clientPolicyViewModel = (ClientPolicyViewModel) Session["clientPolicyViewModel"];
            var policy = db.Policies.Include(i => i.Coverages).First(i => i.ID == id);

            clientPolicyViewModel.Policies[0] = new AssignPolicyViewModel
            {
                ID = policy.ID,
                Name = policy.Name,
                Description = policy.Description,
                ValidityStart = policy.ValidityStart,
                Price = policy.Price,
                RiskType = policy.RiskType,
                Assigned = policy.Clients.SingleOrDefault(i => i.ID == clientPolicyViewModel.Client.ID) != null,
                Coverages = policy.Coverages
            };

            Session["clientPolicyViewModel"] = clientPolicyViewModel;

            return View(clientPolicyViewModel);
        }

        // POST: Policy/AssignPolicies
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Assign a policy to a defined client.
        /// </summary>
        /// <param name="clientPolicyViewModel">Client policy view model.</param>
        /// <returns>Client policy view model.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPolicy(ClientPolicyViewModel clientPolicyViewModel)
        {
            clientPolicyViewModel = (ClientPolicyViewModel) Session["clientPolicyViewModel"];

            if (clientPolicyViewModel.Client == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                clientPolicyViewModel.ClientID = clientPolicyViewModel.Client.ID;

                var clientToUpdate = db.Clients.Include(i => i.Policies).First(i => i.ID == clientPolicyViewModel.ClientID);

                clientPolicyViewModel.Client = clientToUpdate;

                var clientID = clientPolicyViewModel.Policies[0].ID;
                var policy = db.Policies.SingleOrDefault(i => i.ID == clientID);
                
                if (!clientToUpdate.Policies.Contains(policy))
                    clientToUpdate.Policies.Add(policy);
                else
                    clientToUpdate.Policies.Remove(policy);

                db.Entry(clientToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ListPolicies/" + clientPolicyViewModel.ClientID);
            }

            LoadCoverages();

            return View(clientPolicyViewModel);
        }

        /// <summary>
        /// Dispose the view.
        /// </summary>
        /// <param name="disposing">Indicates if dispose a coverage or not.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Load all the database coverages.
        /// </summary>
        private void LoadCoverages()
        {
            ViewBag.Coverages = db.Coverages.ToList().Select(o => new SelectListItem
            {
                Text = o.Name + " - " + o.Percentage + "% - " + o.Period + " months",
                Value = o.ID.ToString()
            });
        }

        /// <summary>
        /// Validate that at least a coverage be selected.
        /// </summary>
        /// <param name="policyViewModel">Policy view model.</param>
        /// <returns>True if there are selected coverages, else if not.</returns>
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

        /// <summary>
        /// Validate if there are high risk coverages.
        /// </summary>
        /// <param name="policyViewModel">Policy view model.</param>
        /// <returns>True if there are high risk coverages, else if not.</returns>
        private bool ValidateHighRiskCoverages(PolicyViewModel policyViewModel)
        {
            if (policyViewModel.Policy.RiskType == RiskType.High)
            {
                foreach (var c in policyViewModel.SelectedCoverages)
                {
                    var coverage = db.Coverages.Find(c);

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