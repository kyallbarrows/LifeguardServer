using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parichay.Data.Entity;
using Parichay.Data.Helper;

namespace Parichay.MVC.Controllers
{
    public class MediaController : BaseController
    {
        public ActionResult Index()
        {
            IList<MemberUploads> myFiles = NHibernateHelper.FetchProjection<MemberUploads>(new string[] { "Id", "Attachmt", "FileSize", "CreateD" }, "Owner.Id", LoggedInUserKey,null,null,false,null,false);
            return View(myFiles);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add()
        {
            try
            {
                SaveImage();
            }
            catch (Exception exc1)
            {
                TempData["message"] = exc1.Message;
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddAvatar()
        {
            try
            {
                SaveAvImage();
            }
            catch (Exception exc1)
            {
                TempData["message"] = exc1.Message;
            }
            return RedirectToAction("Edit", "Account");
        }

        public ActionResult Avatar(Int32? id)
        {
            if(!id.HasValue)
            {
                return(new ImageResult(Utilities.FileToArray(Utilities.GetPhysicalLocation("Content/images/empty.jpg")), "image/jpeg"));
            }
            else
            {
            int avId = id.Value;
            //This is  method for getting the image information
            // including the image byte array from the image column in
            // a database.
            MemberUploads image = NHibernateHelper.UniqueResult<MemberUploads>(null, "Id", avId);

            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            ImageResult result = (image != null) ? new ImageResult(image.FileDetail, image.FileContentT) : new ImageResult(Utilities.FileToArray(Utilities.GetPhysicalLocation("Content/images/empty.jpg")), "image/jpeg");
            return result;
            }
        }




        public ActionResult AvatarByUId(Int32 id)
        {

            int? picId = NHibernateHelper.UniqueResult<MemberDetails>("PicId", "Id", id).PicId;
            //This is method for getting the image information
            // including the image byte array from the image column in
            // a database.
            MemberUploads image = NHibernateHelper.UniqueResult<MemberUploads>(null, "Id", picId.HasValue ? picId : 0);
            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            ImageResult result = new ImageResult(image.FileDetail, image.FileContentT);

            return result;
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteAvatar()
        {
            MemberDetails usr = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

            ////Delete previous avatar image if any
            if ((usr.PicId.HasValue) && (usr.PicId.Value != 0))
            {
                MemberUploads itm = NHibernateHelper.UniqueResult<MemberUploads>(null, "Id", usr.PicId.Value);

                if (itm != null)
                    NHibernateHelper.Delete<MemberUploads>(itm);

                TempData["message"] = "Your Avatar Image is deleted Now.";
            }

            return RedirectToAction("Edit", "Account");
        }

        private bool isImage(string fileName, bool ignoreCase)
        {
            string[] acceptedImgExt = new string[] { ".jpg", ".jpeg", ".bmp", ".gif", ".jpeg", ".png", ".tif" };
            string FileExtention = System.IO.Path.GetExtension(fileName);

            for (int i = 0; i <= acceptedImgExt.Length - 1; i++)
            {
                if ((string.Compare(FileExtention, acceptedImgExt[i], ignoreCase) == 0))
                    return true;
            }

            return false;
        }

        private void SaveAvImage()
        {
            ////Delete previous avatar image if any
            //MemberUploads itm = NHibernateHelper.UniqueResult<MemberUploads>(null, "Id", LoggedInUserKey);
            //if (itm != null)
            //    NHibernateHelper.Delete<MemberUploads>(itm);

            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[inputTagName];



                if (isImage(file.FileName, true))
                {

                    MemberDetails usr = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

                    ////Delete previous avatar image if any
                    ////Delete previous avatar image if any
                    if ((usr.PicId.HasValue) && (usr.PicId.Value != 0))
                    {
                        MemberUploads itm = NHibernateHelper.UniqueResult<MemberUploads>(null, "Id", usr.PicId.Value);

                        if (itm != null)
                            NHibernateHelper.Delete<MemberUploads>(itm);
                    }


                    if (file.ContentLength > 0)
                    {

                        TempData["message"] = "Resizing the file '" + file.FileName + "' to avatar Image now.";

                        MemberUploads attch = new MemberUploads();
                        attch.Owner = usr;//NHibernateHelper.UniqueResult<MemberDetails>(null,"Id",LoggedInUserKey);
                        attch.Attachmt = file.FileName;

                        attch.FileContentT = file.ContentType;
                        //attch.FileSize = file.ContentLength;
                        attch.CreateD = DateTime.Now;
                        attch.FileDetail = Utilities.CreateAvatar(100, file.InputStream);
                        attch.FileSize = attch.FileDetail.Length;
                        NHibernateHelper.Save<MemberUploads>(attch);
                        //Save the attachment into database according to the paperId, conferenceCode etc
                        usr.PicId = attch.Id;
                        NHibernateHelper.Update<MemberDetails>(usr);

                    }
                }
                else
                {
                    throw new Exception("Only JPG , BMP , PNG , GIF and TIF files can be uploaded as profile image.");
                }
            }
        }

        private Byte[] Convert(HttpPostedFileBase source)
        {
            Byte[] destination = new Byte[source.ContentLength];
            source.InputStream.Position = 0;
            source.InputStream.Read(destination, 0, source.ContentLength);
            return destination;
        } // Convert

        private void SaveImage()
        {
            ////Delete previous avatar image if any
            //MemberUploads itm = NHibernateHelper.UniqueResult<MemberUploads>(null, "Id", LoggedInUserKey);
            //if (itm != null)
            //    NHibernateHelper.Delete<MemberUploads>(itm);

            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[inputTagName];



                if (isImage(file.FileName, true))
                {

                    MemberDetails usr = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

                    int MyfileCount = NHibernateHelper.Count("select a.Id from MemberUploads a where a.Owner.Id=?", LoggedInUserKey,NHibernate.NHibernateUtil.Int32,false);

                    if (MyfileCount > 10)
                    {
                        throw new Exception("Maximum 10 file uploads are allowed per user. To upload more, please remove any of previous uploads.");
                    }

                    if (file.ContentLength > 0)
                    {

                        //TempData["message"] = "Resizing the file '" + file.FileName + "' to avatar Image now.";

                        MemberUploads attch = new MemberUploads();
                        attch.Owner = usr;
                        attch.Attachmt = file.FileName;

                        attch.FileContentT = file.ContentType;
                        //attch.FileSize = file.ContentLength;
                        attch.CreateD = DateTime.Now;
                        attch.FileDetail = Convert(file); //Utilities.CreateAvatar(100, file.InputStream);
                        attch.FileSize = attch.FileDetail.Length;
                        NHibernateHelper.Save<MemberUploads>(attch);
                        //Save the attachment into database according to the paperId, conferenceCode etc
                        //usr.PicId = attch.Id;
                        //NHibernateHelper.Update<MemberDetails>(usr);

                        //string filePath = Path.Combine(@"C:\MyUploadedFiles", Path.GetFileName(file.FileName));
                        //file.SaveAs(filePath);
                    }
                }
                else
                {
                    throw new Exception("Only JPG , BMP , PNG , GIF and TIF files can be uploaded as profile image.");
                }
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                NHibernateHelper.Delete("from MemberUploads a where a.Id=? and a.Owner.Id=?", new object[] { id, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });
            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable to delete." + ex1.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
