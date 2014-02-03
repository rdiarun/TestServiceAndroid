using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace DetectorService
{
    /// <summary>
    /// Summary description for ImageUpload
    /// </summary>
    public class ImageUpload : IHttpHandler
    {
        //static ReportRepository db = new ReportRepository();
        //static PropertyRepository db2 = new PropertyRepository();
        public void ProcessRequest(HttpContext context)
        {
            string guid = context.Request["guid"];

            long reportID = 0;

            long.TryParse(context.Request["rid"], out reportID);

            bool isSuccessful = true;
            string errorMessage = "";

            if (guid != null && guid.Length > 0)
            {
                try
                {
                    string dictionary = "";
                    var Qury ="";
                    //var Qury = "SELECT   PropertyInfoId, LocationName, ImageName FROM   AlarmImages where Image_guid ='" + guid + "'";
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetImageDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("Image_guid", guid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dictionary = reader.GetInt32(0) + "/" + reader.GetString(1) + "/" + reader.GetInt32(2);
                            }
                        }
                    }
                    if (dictionary != "")
                    {
                        long nFileLen = context.Request.InputStream.Length;
                        //
                        byte[] myData = new byte[nFileLen];
                        //myFile.InputStream.Read(myData, 0, nFileLen);
                        context.Request.InputStream.Read(myData, 0, (int)nFileLen);

                        // Get Base URL
                        string url = context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath.TrimEnd('/') + "/";

                        // Get base local server path
                        string baseDir = HttpContext.Current.Server.MapPath("~");

                        // Create new directories to organize photos by Office and Report

                        string extraDir = string.Format("photos/{0}/", dictionary.Split('/')[0]);
                        string fullDir = baseDir + "/" + extraDir;
                        //string fullDir = baseDir + extraDir;
                        if (!System.IO.Directory.Exists(fullDir))
                        {
                            System.IO.Directory.CreateDirectory(fullDir);
                        }
                        //string path = HttpContext.Current.Server.MapPath(string.Format("{0}.jpg", guid));
                        string path = fullDir + string.Format(dictionary.Split('/')[0] + "_" + dictionary.Split('/')[2] + ".jpg", guid);

                        string finalUrl = url + extraDir;
                        System.IO.File.WriteAllBytes(path, myData);

                        var thumbpath = "/" + extraDir + string.Format(dictionary.Split('/')[0] + "_" + dictionary.Split('/')[2] + ".jpg", guid);
                        //FileStream f1 = new FileStream(path, FileMode.Open);
                        //int length = Convert.ToInt16(f1.Length);
                        //Byte[] b1 = new Byte[length];
                        //f1.Read(b1, 0, length);
                        //var thumbpath = fullDir + "/" + "Thumb_" + dictionary.Split('/')[0] + "_" + dictionary.Split('/')[1] + "_" + dictionary.Split('/')[2] + ".jpg";
                        //File.WriteAllBytes(thumbpath, b1);
                        //f1.Dispose();


                        Qury = "update AlarmImages set ImagePath = '" + thumbpath + "', thumbnailPath = '" + thumbpath + "' where Image_guid ='" + guid + "'";
                        using (SqlCommand command = new SqlCommand(Qury, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        //GenerateThumbnails(40, context.Request.InputStream, path);
                    }
                    else
                    {

                    }
                    connection.Close();
                    connection.Dispose();

                }
                catch (Exception e)
                {
                    isSuccessful = false;
                    errorMessage = e.Message;
                }
            }
            else
            {
                isSuccessful = false;
                errorMessage = "Missing GUID";
            }
            context.Response.ContentType = "text/plain";
            string response = "";
            if (isSuccessful)
            {
                response = "OK," + guid;

            }
            else
            {
                response = string.Format("ERROR,{0},{1}", guid, errorMessage);
            }
            context.Response.Write(response);

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }



        public void GenerateThumbNail(string thumbNailFolder, int width, string path)
        {
            //HttpPostedFile myFile = srcFile;
            //string filename = srcFile.FileName;
            //int oFileLength = myFile.ContentLength;
            //if (oFileLength != 0)
            //{
            //int i = filename.LastIndexOf("\\");
            //filename = filename.Substring(i + 1);
            //filename = DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-" + filename;
            string thumb_filename = "thumb_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-abc.jpg";
            //byte[] myData = new Byte[oFileLength];
            //myFile.InputStream.Read(myData, 0, oFileLength);

            //System.IO.FileStream newFile = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~/images/") + filename, System.IO.FileMode.Create);
            //newFile.Write(myData, 0, myData.Length);
            //newFile.Close();



            System.Drawing.Image image = System.Drawing.Image.FromFile(path);
            int srcWidth = image.Width;
            int srcHeight = image.Height;
            int thumbWidth = width;
            int thumbHeight;
            Bitmap bmp;
            if (srcHeight > srcWidth)
            {
                thumbHeight = (srcHeight / srcWidth) * thumbWidth;
                bmp = new Bitmap(thumbWidth, thumbHeight);
            }
            else
            {
                thumbHeight = thumbWidth;
                thumbWidth = (srcWidth / srcHeight) * thumbHeight;
                bmp = new Bitmap(thumbWidth, thumbHeight);
            }

            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
            bmp.Save(HttpContext.Current.Server.MapPath("~/" + thumbNailFolder + "/" + thumb_filename));
            bmp.Dispose();
            image.Dispose();

            //}
        }


        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                // can given width of image as we want
                var newWidth = (int)(image.Width * scaleFactor);
                // can given height of image as we want
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

    }
}