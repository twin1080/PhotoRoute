using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            var journey = FindJourneybyId(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            return View(journey);
        }

        private Journey FindJourneybyId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            Journey journey = db.Journey.Find(id);
            return journey;
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
                int i = 1;
                if (db.Point.Any())
                {
                    i = db.Point.ToList().OrderBy(x => x.Id).Select(x => x.Id).Last() + 1;
                }
                foreach (var file in files)
                {
                    var fileName = FileHelper.SaveFileToHardDrive(file);
                    var newPoint = FileHelper.NewPointByStoredFile(fileName, ref i);

                    if (newPoint != null)
                    {
                        newPoint.JourneyId = journey.Id;
                        db.Point.Add(newPoint);
                    }
                }

                db.Entry(journey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(journey);
        }

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


        public JsonResult GetData(int? id)
        {
            var result = new List<dynamic>();
            // создадим список данных
            var journey = FindJourneybyId(id);

            return Json(journey.Point.Select(x => new
            {
                x.latitude,
                x.longitude,
                file = x.file.ToLower().Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"].ToLower(), Request.Url.GetLeftPart(UriPartial.Authority) + "/"),
                x.Time
            }).ToList(), JsonRequestBehavior.AllowGet);
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
