using Insurance.DAL;
using Insurance.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Insurance.Controllers
{
    /// <summary>
    /// Coverage controller.
    /// </summary>
    public class CoverageController : Controller
    {
        /// <summary>
        /// Insurance context.
        /// </summary>
        private InsuranceContext db = new InsuranceContext();

        // GET: Coverage
        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Coverages.ToList());
        }

        // GET: Coverage/Details/5
        /// <summary>
        /// Details action.
        /// </summary>
        /// <param name="id">Coverage id.</param>
        /// <returns>View with coverage details.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Coverage coverage = db.Coverages.Find(id);
            
            if (coverage == null)
                return HttpNotFound();
            
            return View(coverage);
        }

        // GET: Coverage/Create
        /// <summary>
        /// Create a coverage.
        /// </summary>
        /// <returns>Coverage view.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coverage/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create a coverage.
        /// </summary>
        /// <param name="coverage">Coverage.</param>
        /// <returns>Coverage view.</returns>
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
        /// <summary>
        /// Edit a coverage.
        /// </summary>
        /// <param name="id">Coverage id.</param>
        /// <returns>Coverage view.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Coverage coverage = db.Coverages.Find(id);
            
            if (coverage == null)
                return HttpNotFound();
            
            return View(coverage);
        }

        // POST: Coverage/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit a coverage.
        /// </summary>
        /// <param name="coverage">Coverage.</param>
        /// <returns>Coverage view.</returns>
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
        /// <summary>
        /// Delete coverage.
        /// </summary>
        /// <param name="id">Coverage id.</param>
        /// <returns>Coverage view.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Coverage coverage = db.Coverages.Find(id);
            
            if (coverage == null)
                return HttpNotFound();
            
            return View(coverage);
        }

        // POST: Coverage/Delete/5
        /// <summary>
        /// Confirm the deletion of a coverage.
        /// </summary>
        /// <param name="id">Coverage id.</param>
        /// <returns>Action index.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coverage coverage = db.Coverages.Find(id);

            db.Coverages.Remove(coverage);
            db.SaveChanges();

            return RedirectToAction("Index");
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
    }
}