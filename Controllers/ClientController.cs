using Insurance.DAL;
using Insurance.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Insurance.Controllers
{
    /// <summary>
    /// Client controller.
    /// </summary>
    public class ClientController : Controller
    {
        /// <summary>
        /// Insurance context.
        /// </summary>
        private InsuranceContext db = new InsuranceContext();

        // GET: Client
        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Client/Details/5
        /// <summary>
        /// Details action.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>View with client details.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Client client = db.Clients.Find(id);

            if (client == null)
                return HttpNotFound();

            return View(client);
        }

        // GET: Client/Create
        /// <summary>
        /// Create a client.
        /// </summary>
        /// <returns>Client view.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create a client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <returns>Client view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Client/Edit/5
        /// <summary>
        /// Edit a client.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Client view.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Client client = db.Clients.Find(id);
            
            if (client == null)
                return HttpNotFound();
            
            return View(client);
        }

        // POST: Client/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit a client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <returns>Client view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Client/Delete/5
        /// <summary>
        /// Delete client.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Client view.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Client client = db.Clients.Find(id);
            
            if (client == null)
                return HttpNotFound();
            
            return View(client);
        }

        // POST: Client/Delete/5
        /// <summary>
        /// Confirm the deletion of a client.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Action index.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);

            db.Clients.Remove(client);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Dispose the view.
        /// </summary>
        /// <param name="disposing">Indicates if dispose a client or not.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            
            base.Dispose(disposing);
        }
    }
}