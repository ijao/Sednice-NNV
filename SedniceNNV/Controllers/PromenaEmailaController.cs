using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SedniceNNV.Models;

namespace SedniceNNV.Controllers
{
    public class PromenaEmailaController : Controller
    {
        private ProjekatEntities db = new ProjekatEntities();

        // GET: PromenaEmaila
        public ActionResult Index()
        {
            string userEmail = Session["Email"].ToString();
            var upit = db.Clanovi.Where(c => c.Email == userEmail);

            return View(upit);
        }

        

        

        // GET: PromenaEmaila/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clanovi clanovi = db.Clanovi.Find(id);
            if (clanovi == null)
            {
                return HttpNotFound();
            }
            return View(clanovi);
        }

        // POST: PromenaEmaila/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clanovi clanovi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clanovi).State = EntityState.Modified;
                db.SaveChanges();
                Session["Email"] = db.Clanovi.Find(clanovi.ClanID).Email.ToString();
                return RedirectToAction("Index");
            }
            return View(clanovi);
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
