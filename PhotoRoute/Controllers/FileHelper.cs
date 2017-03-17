using System;
using System.IO;
using System.Web;
using System.Windows.Media.Imaging;
using PhotoRoute.Models;

namespace PhotoRoute.Controllers
{
    public class FileHelper
    {
        public static Point NewPointByStoredFile(string fileName, ref int i)
        {
            Point newPoint = null;
            FileStream foto = File.Open(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                var decoder = BitmapDecoder.Create(foto, BitmapCreateOptions.IgnoreColorProfile,
                    BitmapCacheOption.Default);
                var imageMetadata = decoder.Frames[0].Metadata;
                if (imageMetadata != null)
                {
                    BitmapMetadata exif = (BitmapMetadata) imageMetadata.Clone();
                    var latitude = (ulong[]) exif.GetQuery("/app1/ifd/gps/{ushort=2}"); // Documentation for EXIF http://www.exif.org/Exif2-2.PDF
                    var longitude = (ulong[]) exif.GetQuery("/app1/ifd/gps/{ushort=4}");
                    var unparcedDate = exif.DateTaken;
                    DateTime? photoTime = !String.IsNullOrEmpty(unparcedDate)
                        ? (DateTime?) Convert.ToDateTime(unparcedDate)
                        : null;
                    if (latitude != null && longitude != null && photoTime != null)
                    {
                        var realLatitude = GPSHelper.RationalDegreesToReal(latitude[0], latitude[1], latitude[2]);
                        var realLongitude = GPSHelper.RationalDegreesToReal(longitude[0], longitude[1], longitude[2]);
                        newPoint = new Point()
                        {
                            Id = i++,
                            Time = photoTime.Value,
                            latitude = realLatitude,
                            longitude = realLongitude,
                            file = fileName
                        };
                    }
                }
            }
            finally
            {
                foto.Close();
            }

            return newPoint;
        }

        public static string SaveFileToHardDrive(HttpPostedFileBase file)
        {
            var folder = Path.Combine(HttpRuntime.AppDomainAppPath, "Photo");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var fileName = Path.Combine(HttpRuntime.AppDomainAppPath, "Photo", file.FileName);
            file.SaveAs(fileName);
            return fileName;
        }
    }
}