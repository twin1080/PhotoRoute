using System;
using System.IO;
using System.Web;
using System.Windows.Media.Imaging;
using PhotoRoute.Models;

namespace PhotoRoute.Controllers
{
    public class FileHelper
    {
        public static float FetchLatitudeFromFile(string fileName)
        {
            float result = 0;
            var exif = BitmapMetadata(fileName);
            if (exif != null)
            {
                result = FetchLatitudeFromExif(exif);
            }
            return result;
        }

        public static float FetchLongitudeFromFile(string fileName)
        {
            float result = 0;
            var exif = BitmapMetadata(fileName);
            if (exif != null)
            {
                result = FetchLongitudeFromExif(exif);
            }
            return result;
        }

        public static DateTime? FetchDateFromFile(string fileName)
        {
            DateTime? result = null;
            var exif = BitmapMetadata(fileName);
            if (exif != null)
            {
                result = FetchDateFromExif(exif);
            }

            return result;
        }

        private static float FetchLatitudeFromExif(BitmapMetadata exif)
        {

            var latitude = (ulong[])exif.GetQuery("/app1/ifd/gps/{ushort=2}");
            // Documentation for EXIF http://www.exif.org/Exif2-2.PDF
            if (latitude != null)
            {
                return GPSHelper.RationalDegreesToReal(latitude[0], latitude[1], latitude[2]);
            }

            throw new ArgumentNullException(nameof(exif));

          
        }

        private static float FetchLongitudeFromExif(BitmapMetadata exif)
        {

            var longitude = (ulong[])exif.GetQuery("/app1/ifd/gps/{ushort=4}");
            // Documentation for EXIF http://www.exif.org/Exif2-2.PDF
            if (longitude != null)
            {
                return GPSHelper.RationalDegreesToReal(longitude[0], longitude[1], longitude[2]);
            }

            throw new ArgumentNullException(nameof(exif));
        }

        private static DateTime? FetchDateFromExif(BitmapMetadata exif)
        {
            var unparcedDate = exif.DateTaken;
            var photoTime = !String.IsNullOrEmpty(unparcedDate)
                ? (DateTime?)Convert.ToDateTime(unparcedDate)
                : null;
            return photoTime;
        }

        private static BitmapMetadata BitmapMetadata(string fileName)
        {
            BitmapMetadata exif = null;
            FileStream foto = File.Open(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                var decoder = BitmapDecoder.Create(foto, BitmapCreateOptions.IgnoreColorProfile,
                    BitmapCacheOption.Default);
                var imageMetadata = decoder.Frames[0].Metadata;
                if (imageMetadata != null)
                {
                    exif = (BitmapMetadata)imageMetadata.Clone();
                }
            }
            finally
            {
                foto.Close();
            }
            return exif;
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