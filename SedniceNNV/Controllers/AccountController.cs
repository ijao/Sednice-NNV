using SedniceNNV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SedniceNNV.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Clanovi clan)
        {
            //ako su u bazi hash passwords:
            //clan.Lozinka = Encrypt.GetMD5Hash(clan.Lozinka);

            using (ProjekatEntities db = new ProjekatEntities())
            {
                
                var user = db.Clanovi.SingleOrDefault(u => u.KorisnickoIme == clan.KorisnickoIme && u.Lozinka == clan.Lozinka); 
                

                    if (user != null)
                {
                    Session["KorisnickoIme"] = user.KorisnickoIme.ToString();
                    Session["Lozinka"] = user.Lozinka.ToString();
                    Session["Email"] = user.Email.ToString();
                    Session["Uloga"] = user.Uloga.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Pogresno korisnicko ime ili lozinka");
                }
            }
            return View();
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            return View("Index");
        }


    }
}