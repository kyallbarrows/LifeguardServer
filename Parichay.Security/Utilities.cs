using System;
using System.Collections.Generic;

using System.Text;

namespace Parichay
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Utilities for use with Parichay.Security and Parichay.Web
    /// </summary>
    public static class Utilities
    {
        private static Object LockTxtLog = new Object();

        /// <summary>
        /// String display for messages with time difference from the current DateTime
        /// e.g.  "1hrs 20min ago"
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDateTimeForDisplay(DateTime? date)
        {
            if (date == null)
                return string.Empty;
            else
            {
                TimeSpan diff = DateTime.Now - date.Value;

                if (diff.TotalDays < 1)
                    return diff.Hours.ToString() + "hrs " + diff.Minutes.ToString() + "min ago";
                else
                    return date.Value.ToShortDateString() + " " + date.Value.ToShortTimeString();
            }
        }

        /// <summary>
        /// Format DateTime value for easy insertion into Database.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// For parsing DateTime string from the format "yyyyMMdd"
        /// </summary>
        /// <param name="dtString">DateTime string in "yyyyMMdd" format</param>
        /// <returns></returns>
        public static DateTime ParseDate(string dtString)
        {
            return DateTime.ParseExact(dtString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// If the current exception is just a wrapper for InnerException, clips the Wrapper exception to 50 characters
        /// If InnerException is not null. Clip the current exception to 50 characters.  
        /// E.g. "Current exception Message...{upto 50characters} + "...->" + InnerException.Message
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatException(Exception ex)
        {
            string message = ex.Message;

            //Add the inner exception if present (showing only the first 50 characters of the first exception)
            if (ex.InnerException != null)
            {
                if (message.Length > 50)
                    message = message.Substring(0, 50);

                message += "...->" + ex.InnerException.Message;
            }

            return message;
        }

        
        /// <summary>
        /// Write message to Text log file
        /// The log file is maintained in the "logs/log.txt" document in the root directory of the application.
        /// </summary>
        /// <param name="Message"></param>
        public static void TextLog(string Message)
        {
            lock (LockTxtLog)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(NavHelper.GetPhysicalLocation("logs/log.txt"), true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + Message);
                }


            }
        }

        public static byte[] CreateAvatar(int sideLength, System.IO.Stream fromStream)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            using (var image = System.Drawing.Image.FromStream(fromStream))
            using (var thumbBitmap = new System.Drawing.Bitmap(sideLength, sideLength))
            {

                var a = Math.Min(image.Width, image.Height);

                var x = (image.Width - a) / 2;

                var y = (image.Height - a) / 2;



                using (var thumbGraph = System.Drawing.Graphics.FromImage(thumbBitmap))
                {

                    thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;



                    var imgRectangle = new System.Drawing.Rectangle(0, 0, sideLength, sideLength);

                    thumbGraph.DrawImage(image, imgRectangle, x, y, a, a, System.Drawing.GraphicsUnit.Pixel);

                    thumbBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                }

            }
            return (ms.ToArray());

        }

        public static byte[] FileToArray(string sFilePath)
        {
            System.IO.FileStream fs = new System.IO.FileStream(sFilePath,
                System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();
            return bytes;
        }

        /// <summary>
        /// Retrieves Physical Application Path for a location
        /// For example "C:\Users\Sam\Documents\Visual Studio 2008\WebSites\parichay\Default.Aspx" for the path "Default.aspx"
        /// </summary>
        /// <param name="path">Physical Path to be appended</param>
        /// <returns></returns>
        public static string GetPhysicalLocation(string path)
        {
            return (System.Web.HttpContext.Current.Request.PhysicalApplicationPath + path);
        }
    }

}
