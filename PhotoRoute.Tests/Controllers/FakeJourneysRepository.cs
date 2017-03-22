using System;
using System.Collections.Generic;
using PhotoRoute.Models;

namespace PhotoRoute.Controllers.Tests
{
    public class FakeJourneysRepository : IJourneysRepository
    {
        public void AddNewJourney(Journey journey)
        {

        }

        public void AddNewPoints(Journey journey, List<Point> newPoints)
        {

        }

        public void DeleteJourney(int id)
        {

        }

        public Journey FindJourneybyId(int? id)
        {
            return new Journey()
            {
                Id = 1,
                Name = "Test",
                StartDate = DateTime.Now,
                Point = new List<Point> {
                                            new Point
                                            {
                                                file = "test.jpg",
                                                Id = 1,
                                                longitude = 0,
                                                latitude = 0,
                                                JourneyId = 1,
                                                Time = DateTime.Now
                                            }
                                        }
            };
        }

        public IEnumerable<Journey> GetAllJourneys()
        {
            return new List<Journey> { new Journey
            {
                Id = 1,
                Name = "Test",
                StartDate = DateTime.Now
            }};
        }

        public int GetFreePointId()
        {
            return 1;
        }
    }
}