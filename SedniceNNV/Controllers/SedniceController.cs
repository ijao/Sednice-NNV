using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SedniceNNV.Models;
using PagedList;

namespace SedniceNNV.Controllers
{
    [CustomApiAuthorize(Roles = "Admin")]
    public class SedniceController : Controller
    {
        private ProjekatEntities db = new ProjekatEntities();

        [CustomApiAuthorize(Roles = "Admin,User")]
        // GET: Sednice
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.RBSedniceSortParm = String.IsNullOrEmpty(sortOrder) ? "rbsednice_asc" : "";
            

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var sednice = from s in db.Sednice
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sednice = sednice.Where(s => s.Zapisnik.Contains(searchString)
                                       || s.Poziv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "rbsednice_asc":
                    sednice = sednice.OrderBy(s => s.SednicaID);
                    break;
                default:
                    sednice = sednice.OrderByDescending(s => s.SednicaID);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(sednice.ToPagedList(pageNumber, pageSize));
        }

        // GET: Sednice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sednice sednice = db.Sednice.Find(id);
            if (sednice == null)
            {
                return HttpNotFound();
            }
            return View(sednice);
        }

        // GET: Sednice/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sednice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SednicaID,Datum,Vrsta,Ucionica,Zapisnik,Poziv")] Sednice sednice)
        {
            if (ModelState.IsValid)
            {
                db.Sednice.Add(sednice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sednice);
        }

        // GET: Sednice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sednice sednice = db.Sednice.Find(id);
            if (sednice == null)
            {
                return HttpNotFound();
            }
            return View(sednice);
        }

        // POST: Sednice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SednicaID,Datum,Vrsta,Ucionica,Zapisnik,Poziv")] Sednice sednice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sednice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sednice);
        }

        // GET: Sednice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sednice sednice = db.Sednice.Find(id);
            if (sednice == null)
            {
                return HttpNotFound();
            }
            return View(sednice);
        }

        // POST: Sednice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sednice sednice = db.Sednice.Find(id);
            db.Sednice.Remove(sednice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PrisustvoIndex(int? id, string sortOrder, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstName_desc" : "firstName";

            

            

            var prisustvo = from p in db.Prisustvo
                          select p;

            

            switch (sortOrder)
            {
                case "lastName_desc":
                    prisustvo = prisustvo.OrderByDescending(p => p.Clanovi.Prezime);
                    break;
                case "firstName":
                    prisustvo = prisustvo.OrderBy(p => p.Clanovi.Ime);
                    break;
                case "firstName_desc":
                    prisustvo = prisustvo.OrderByDescending(p => p.Clanovi.Ime);
                    break;
                default:
                    prisustvo = prisustvo.OrderBy(p => p.Clanovi.Prezime);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(prisustvo.ToPagedList(pageNumber, pageSize));


           
        }


        // GET: Sednice/PrisustvoEdit/5
        public ActionResult PrisustvoEdit(int? SednicaID, int? ClanID)
        {
            if (SednicaID == null || ClanID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prisustvo prisustvo = db.Prisustvo.Find(SednicaID, ClanID);
            if (prisustvo == null)
            {
                return HttpNotFound();
            }
            
            return View(prisustvo);
        }

        // POST: Prisustvo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrisustvoEdit(Prisustvo prisustvo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prisustvo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(prisustvo);
        }

        [CustomApiAuthorize(Roles = "Admin,User")]

        public ActionResult PriloziIndex(int? id)
        {
            var prilozi = db.Prilozi.Include(p => p.Sednice).Where(s => s.SednicaID == id);
            return View(prilozi.ToList());
        }

        // GET: Sednice/PriloziDetails/5
        public ActionResult PriloziDetails(int? SednicaID, int? RedniBrojPriloga)
        {
            if (SednicaID == null || RedniBrojPriloga == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prilozi prilozi = db.Prilozi.Find(SednicaID, RedniBrojPriloga);
            if (prilozi == null)
            {
                return HttpNotFound();
            }

            return View(prilozi);
        }

        // GET: Sednice/PriloziCreate
        public ActionResult PriloziCreate()
        {
            ViewBag.SednicaID = new SelectList(db.Sednice, "SednicaID", "SednicaID");
            
            return View();
            
        }

        // POST: Sednice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PriloziCreate([Bind(Include = "SednicaID,RedniBrojPriloga,NazivDokumenta,Poslato,DatumSlanja")]  Prilozi prilozi )
        {
            if (ModelState.IsValid)
            {
                
                db.Prilozi.Add(prilozi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SednicaID = new SelectList(db.Sednice, "SednicaID", "SednicaID", prilozi.SednicaID);
           
            return View(prilozi);
        }

        // GET: Sednice/PriloziEdit/5
        public ActionResult PriloziEdit(int? SednicaID, int? RedniBrojPriloga)
        {
            if (SednicaID == null || RedniBrojPriloga == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prilozi prilozi = db.Prilozi.Find(SednicaID, RedniBrojPriloga);
            if (prilozi == null)
            {
                return HttpNotFound();
            }

            return View(prilozi);
        }

        // POST: Prilozi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PriloziEdit(Prilozi prilozi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prilozi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prilozi);
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
