using System;
using System.Linq;

namespace PhotoRoute.Controllers
{
    public class GPSHelper
    {
        private static ulong Rational(double a)
        {
            uint denom = 1000;
            uint num = (uint)(a * denom);
            ulong tmp;
            tmp = (ulong)denom << 32;
            tmp |= (ulong)num;
            return tmp;
        }

        private static float Unrational(ulong a)
        {
            var d = BitConverter.GetBytes(a);
            
            int up = BitConverter.ToInt32(d, 0);
            int down = BitConverter.ToInt32(d, 4);

            return (float)up / (float)down;
        }



        public static float RationalDegreesToReal(ulong degrees, ulong minutes, ulong seconds)
        {
            return Unrational(degrees) + Unrational(minutes) / 60 + Unrational(seconds) / 3600;
        }
    }
}