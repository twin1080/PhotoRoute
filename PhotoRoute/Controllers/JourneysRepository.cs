using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PhotoRoute.Models;

namespace PhotoRoute.Controllers
{
    public class JourneysRepository : IJourneysRepository
    {
        private photorouteEntities db = new photorouteEntities();

        public JourneysRepository()
        {

        }

        public Journey FindJourneybyId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            Journey journey = db.Journey.Find(id);
            return journey;
        }

        public IEnumerable<Journey> GetAllJourneys()
        {
            return db.Journey.ToList();
        }

        public void AddNewJourney(Journey journey)
        {
            db.Journey.Add(journey);
            db.SaveChanges();
        }

        public int GetFreePointId()
        {
            int i = 1;
            if (db.Point.Any())
            {
                i = db.Point.ToList().OrderBy(x => x.Id).Select(x => x.Id).Last() + 1;
            }
            return i;
        }

        public void AddNewPoints(Journey journey, List<Point> newPoints)
        {
            foreach (var newPoint in newPoints)
            {
                newPoint.JourneyId = journey.Id;
            }
            db.Point.AddRange(newPoints);
            db.Entry(journey).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteJourney(int id)
        {
            Journey journey = this.FindJourneybyId(id);
            db.Journey.Remove(journey);
            db.SaveChanges();
        }
    }
}