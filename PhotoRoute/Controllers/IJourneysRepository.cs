using System.Collections.Generic;
using PhotoRoute.Models;

namespace PhotoRoute.Controllers
{
    public interface IJourneysRepository
    {
        void AddNewJourney(Journey journey);
        void AddNewPoints(Journey journey, List<Point> newPoints);
        void DeleteJourney(int id);
        Journey FindJourneybyId(int? id);
        IEnumerable<Journey> GetAllJourneys();
        int GetFreePointId();
    }
}