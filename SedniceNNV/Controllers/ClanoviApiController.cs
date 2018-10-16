using System.Linq;
using System.Web.Http;
using SedniceNNV.Models;

using System.Data;
using System.Web.Http.Description;

namespace SedniceNNV.Controllers
{
    //[BasicAuthentication]
    public class ClanoviApiController : ApiController
    {
        private ProjekatEntities db = new ProjekatEntities();

        
        // GET: api/ClanoviApi
        
        public IQueryable<ApiModel.ClanoviApi> GetAllClanovi()
        {
            var clanovi = from c in db.Clanovi
                        select new ApiModel.ClanoviApi()
                        {
                            ClanID = c.ClanID,
                            Ime = c.Ime,
                            Prezime = c.Prezime,
                            KorisnickoIme = c.KorisnickoIme,
                            Status = c.Status,
                            Lozinka = c.Lozinka,
                            Email = c.Email,
                            Uloga = c.Uloga
                        };

            return clanovi;
        }
        
        // GET: api/ClanoviApi/5
        
        public IHttpActionResult GetClanById(int id)
        {
            var clan = db.Clanovi.Select(c =>
        new ApiModel.ClanoviApi()
        {
            ClanID = c.ClanID,
            Ime = c.Ime,
            Prezime = c.Prezime,
            KorisnickoIme = c.KorisnickoIme,
            Status = c.Status,
            Lozinka = c.Lozinka,
            Email = c.Email,
            Uloga = c.Uloga
            
        }).SingleOrDefault(c => c.ClanID == id);

            if (clan == null)
            {
                return NotFound();
            }

            return Ok(clan);
        }

        // POST: api/ClanoviApi
        [ResponseType(typeof(ApiModel.ClanoviApi))]
        public IHttpActionResult PostClan(Clanovi clan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clanovi.Add(clan);
            db.SaveChanges();

            

            var noviClan = new ApiModel.ClanoviApi ()
            {
                //ClanID = clan.ClanID,
                Ime = clan.Ime,
                Prezime = clan.Prezime,
                KorisnickoIme = clan.KorisnickoIme,
                Status = clan.Status,
                Lozinka = clan.Lozinka,
                Email = clan.Email,
                Uloga = clan.Uloga
            };

            return CreatedAtRoute("DefaultApi", new { id = clan.ClanID }, clan);
        }


    }

}
