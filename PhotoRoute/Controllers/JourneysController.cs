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

        private readonly IJourneysRepository _journeysRepository;

        public JourneysController()
        {
            _journeysRepository = new JourneysRepository();
        }

        public JourneysController(IJourneysRepository journeysRepository)
        {
            _journeysRepository = journeysRepository;
        }

        // GET: Journeys
        public ActionResult Index()
        {
            return View(_journeysRepository.GetAllJourneys());
        }

        // GET: Journeys/Details/5
        public ActionResult Details(int? id)
        {
            var journey = _journeysRepository.FindJourneybyId(id);
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
                _journeysRepository.AddNewJourney(journey);
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
            Journey journey = _journeysRepository.FindJourneybyId(id);
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
                var i = _journeysRepository.GetFreePointId();

                var newPoints = new List<Point>();

                foreach (var file in files)
                {
                    var fileName = FileHelper.SaveFileToHardDrive(file);

                    DateTime? photoTime = FileHelper.FetchDateFromFile(fileName);

                    float realLongitude = FileHelper.FetchLongitudeFromFile(fileName);

                    float realLatitude = FileHelper.FetchLatitudeFromFile(fileName);

                    newPoints.Add(new Point()
                    {
                        file = fileName,
                        Id = i++,
                        Time = photoTime.Value,
                        latitude = realLatitude,
                        longitude = realLongitude
                    }
                        );
                }

                _journeysRepository.AddNewPoints(journey, newPoints);

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

            Journey journey = _journeysRepository.FindJourneybyId(id);
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
            _journeysRepository.DeleteJourney(id);
            return RedirectToAction("Index");
        }


        public JsonResult GetData(int? id)
        {
            var result = new List<dynamic>();
            // создадим список данных
            var journey = _journeysRepository.FindJourneybyId(id);

            var phisicalPlace = "";
            if (Request != null && !string.IsNullOrEmpty(Request.ServerVariables["APPL_PHYSICAL_PATH"]))
            {
                phisicalPlace = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToLower();
            }

            var domainName = "";
            if (Request != null && Request.Url != null)
            {
                domainName = Request.Url.GetLeftPart(UriPartial.Authority) + "/";
            }


            return Json(journey.Point.Select(x => new
            {
                x.latitude,
                x.longitude,
                file = (!string.IsNullOrEmpty(phisicalPlace) && !string.IsNullOrEmpty(domainName)) 
                            ? x.file.ToLower().Replace(phisicalPlace, domainName) 
                            : x.file,
                x.Time
            }).ToList(), JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
