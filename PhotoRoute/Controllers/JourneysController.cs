using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoRoute.Models;
using System.Windows.Media.Imaging;

namespace PhotoRoute.Controllers
{
    public class JourneysController : Controller
    {
        private photorouteEntities db = new photorouteEntities();

        // GET: Journeys
        public ActionResult Index()
        {
            return View(db.Journey.ToList());
        }

        // GET: Journeys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journey journey = db.Journey.Find(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            return View(journey);
        }

        // GET: Journeys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Journeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartDate")] Journey journey)
        {
            if (ModelState.IsValid)
            {
                db.Journey.Add(journey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(journey);
        }

        // GET: Journeys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journey journey = db.Journey.Find(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            return View(journey);
        }

       // POST: Journeys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartDate")] Journey journey, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                int i = db.Point.ToList().OrderBy(x => x.Id).Select(x => x.Id).Last() + 1;
                foreach (var file in files)
                {
                    var fileName = System.IO.Path.Combine("C:\\", file.FileName);
                    file.SaveAs(fileName);
                    System.IO.FileStream Foto = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    try
                    {
                        var decoder = JpegBitmapDecoder.Create(Foto, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                        BitmapMetadata exif = (BitmapMetadata)decoder.Frames[0].Metadata.Clone();
                        var latitude = (ulong[])exif.GetQuery("/app1/ifd/gps/{ushort=2}");
                        var longitude = (ulong[])exif.GetQuery("/app1/ifd/gps/{ushort=4}");
                        var photoTime = Convert.ToDateTime(exif.DateTaken);
                        var point = Newtonsoft.Json.JsonConvert.SerializeObject(new object[] { latitude, longitude });
                        db.Point.Add(new Point() { Id = i++, JourneyId = journey.Id, Location = point, Time = photoTime });
                    }
                    finally {
                        Foto.Close();
                    }
                }

                db.Entry(journey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            

            return View(journey);
        }

        //[HttpPost]
        //public ActionResult Edit(object files)
        //{
        //    return RedirectToAction("Edit");
        // }


        // GET: Journeys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journey journey = db.Journey.Find(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            return View(journey);
        }

        // POST: Journeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Journey journey = db.Journey.Find(id);
            db.Journey.Remove(journey);
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
