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
    public class ClanoviController : Controller
    {
        private ProjekatEntities db = new ProjekatEntities();

        [CustomApiAuthorize(Roles = "Admin,User")]
        // GET: Clanovi
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstName_desc" : "firstName";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var clanovi = from c in db.Clanovi
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                clanovi = clanovi.Where(c => c.Prezime.Contains(searchString)
                                       || c.Ime.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "lastName_desc":
                    clanovi = clanovi.OrderByDescending(c => c.Prezime);
                    break;
                case "firstName":
                    clanovi = clanovi.OrderBy(c => c.Ime);
                    break;
                case "firstName_desc":
                    clanovi = clanovi.OrderByDescending(c => c.Ime);
                    break;
                default:
                    clanovi = clanovi.OrderBy(s => s.Prezime);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(clanovi.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clanovi/Details/5
        public ActionResult Details(int? id)
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

        // GET: Clanovi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clanovi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClanID,Ime,Prezime,Email,Status,KorisnickoIme,Lozinka,Uloga")] Clanovi clanovi)
        {
            if (ModelState.IsValid)
            {
                //ako se u bazu upisuje hash password:
                //clanovi.Lozinka = Encrypt.GetMD5Hash(clanovi.Lozinka);
                db.Clanovi.Add(clanovi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clanovi);
        }

        // GET: Clanovi/Edit/5
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

        // POST: Clanovi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClanID,Ime,Prezime,Email,Status,KorisnickoIme,Lozinka,Uloga")] Clanovi clanovi)
        {
            if (ModelState.IsValid)
            {
                //ako se u bazu upisuje hash password:
                //clanovi.Lozinka = Encrypt.GetMD5Hash(clanovi.Lozinka);
                db.Entry(clanovi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clanovi);
        }

        // GET: Clanovi/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Clanovi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clanovi clanovi = db.Clanovi.Find(id);
            db.Clanovi.Remove(clanovi);
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
