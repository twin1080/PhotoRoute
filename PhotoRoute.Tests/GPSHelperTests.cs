using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoRoute.Controllers;

namespace PhotoRoute.Tests
{
    [TestClass]
    public class GPSHelperTests
    {
        [TestMethod]
        public void RationalDegreesToReal()
        {
            ulong degrees = 4294967353;
            ulong minutes = 4294967355;
            ulong seconds = 42949673448146;
            var result = GPSHelper.RationalDegreesToReal(degrees, minutes, seconds);
            var approval = 57.99689;

            Assert.AreEqual((decimal)result, (decimal)approval);
        }
    }
}
