using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoRoute.Controllers;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PhotoRoute.Models;

namespace PhotoRoute.Controllers.Tests
{
    [TestClass()]
    public class JourneysControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.Index();
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.Details(1);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.Create();
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void CreateTest1()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.Create(new FakeJourneysRepository().FindJourneybyId(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod()]
        public void EditTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.Edit(1);
            Assert.IsNotNull(result);
        }

        
        //[TestMethod()]
        //public void EditTest1()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void DeleteTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.Delete(1);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.DeleteConfirmed(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod()]
        public void GetDataTest()
        {
            var controller = new JourneysController(new FakeJourneysRepository());
            var result = controller.GetData(1);
            Assert.IsNotNull(result);
            var data = result.Data as IEnumerable<dynamic>;
            Assert.IsTrue(data.Any());
        }
    }
}