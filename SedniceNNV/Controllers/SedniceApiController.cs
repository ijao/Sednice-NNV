using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SedniceNNV.Models;

namespace SedniceNNV.Controllers
{
    [BasicAuthentication]

    public class SedniceApiController : ApiController
    {
        private ProjekatEntities db = new ProjekatEntities();

        
        // GET: api/SedniceApi
        public IQueryable<ApiModel.SedniceApi> GetSednice()
        {
            var sednice = from s in db.Sednice
                          select new ApiModel.SedniceApi()
                          {
                              SednicaID = s.SednicaID,
                              Datum = s.Datum,
                              Vrsta = s.Vrsta,
                              Ucionica = s.Ucionica,
                              Zapisnik = s.Zapisnik,
                              Poziv = s.Poziv
                              
                          };

            return sednice;
        }

        // GET: api/SedniceApi/5
        [ResponseType(typeof(ApiModel.SedniceApi))]
        public IHttpActionResult GetSednice(int id)
        {


            var sednice =  db.Sednice.Select(s => 
                new ApiModel.SedniceApi()
                {
                    SednicaID = s.SednicaID,
                    Datum = s.Datum,
                    Vrsta = s.Vrsta,
                    Ucionica = s.Ucionica,
                    Zapisnik = s.Zapisnik,
                    Poziv = s.Poziv
                }).SingleOrDefault(s => s.SednicaID == id);
                
            if (sednice == null)
            {
                return NotFound();
            }

            return Ok(sednice);
        }



        

        //// PUT: api/SedniceApi/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutSednice(int id, Sednice sednice)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != sednice.SednicaID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(sednice).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SedniceExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/SedniceApi
        [ResponseType(typeof(ApiModel.SedniceApi))]
        public IHttpActionResult PostSednice(Sednice sednice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sednice.Add(sednice);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SedniceExists(sednice.SednicaID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var novaSednica = new ApiModel.SedniceApi()
            {

                SednicaID = sednice.SednicaID,
                Datum = sednice.Datum,
                Vrsta = sednice.Vrsta,
                Ucionica = sednice.Ucionica,
                Zapisnik = sednice.Zapisnik,
                Poziv = sednice.Poziv
            };

            return CreatedAtRoute("DefaultApi", new { id = sednice.SednicaID }, sednice);
        }

        //// DELETE: api/SedniceApi/5
        //[ResponseType(typeof(Sednice))]
        //public async Task<IHttpActionResult> DeleteSednice(int id)
        //{
        //    Sednice sednice = await db.Sednice.FindAsync(id);
        //    if (sednice == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Sednice.Remove(sednice);
        //    await db.SaveChangesAsync();

        //    return Ok(sednice);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SedniceExists(int id)
        {
            return db.Sednice.Count(e => e.SednicaID == id) > 0;
        }
    }
}