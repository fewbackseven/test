using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using System.IO;
using System.Configuration;
using System.Data;
using ImageResizer;

namespace test.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult ArticleView(string id)
        {
            

            userInfo objUserDetails = new userInfo();
            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";
            }

                Article objArticle = new Article();
            if (int.Parse(id) != 0)
            {
                DataTable article = objArticle.getArticle(int.Parse(id));
                objArticle.ArticleHeading = article.Rows[0]["Art_Heading"].ToString();
                objArticle.ArticleParagraph1 = article.Rows[0]["Art_Paragraph1"].ToString();
                objArticle.AuthorName = article.Rows[0]["Art_AuthorName"].ToString();
                objArticle.ArticleParagraph2 = article.Rows[0]["Art_Paragraph2"].ToString();
                objArticle.ArticleID = int.Parse(article.Rows[0]["Art_pkid"].ToString());
                objArticle.isPublished = article.Rows[0]["Art_isPublished"].ToString().ToCharArray()[0];
                ViewBag.ArticleID = objArticle.ArticleID;
                string filePath = article.Rows[0]["Art_ImagePath"].ToString();

                //String serverPath = Server.MapPath("/Images/");
                //String relativePath = filePath.Substring(serverPath.Length, filePath.Length - serverPath.Length);
                //objArticle.ImagePath = "~/Images/" + relativePath;
                //Session["PreviousImage"] = objArticle.ImagePath;                
                //ViewBag.imageSrc = objArticle.ImagePath;

                ViewBag.imageSrc = filePath;
                Session["ArticleID"] = objArticle.ArticleID;
                Session["isPublished"] = objArticle.isPublished;


                return View(objArticle);

            }

            return View();
        }



        public ActionResult ArticleCreate(string id)
        {
            userInfo objUserDetails = new userInfo();
            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";



                ViewBag.pkid = 0;
                ViewBag.User = "Admin";
                Session["isPublished"] = 'N';
                Session["ArticleID"] = "0";
                Session["PreviousImage"] = null;



                if (int.Parse(id) != 0)
                {
                    Article objArticle = new Article();
                    DataTable article = objArticle.getArticle(int.Parse(id));
                    objArticle.ArticleHeading = article.Rows[0]["Art_Heading"].ToString();
                    objArticle.ArticleParagraph1 = article.Rows[0]["Art_Paragraph1"].ToString();
                    objArticle.AuthorName = article.Rows[0]["Art_AuthorName"].ToString();
                    objArticle.ArticleParagraph2 = article.Rows[0]["Art_Paragraph2"].ToString();
                    objArticle.ArticleID = int.Parse(article.Rows[0]["Art_pkid"].ToString());
                    objArticle.isPublished = article.Rows[0]["Art_isPublished"].ToString().ToCharArray()[0];


                    //Uncomment For Debugging
                    //string filePath = article.Rows[0]["Art_ImagePath"].ToString();
                    //String serverPath = Server.MapPath("/Images/");
                    //String relativePath = filePath.Substring(serverPath.Length, filePath.Length - serverPath.Length);
                    //objArticle.ImagePath = "~/Images/" + relativePath;
                    //Session["Imagefile"] = relativePath;

                    objArticle.ImagePath = article.Rows[0]["Art_ImagePath"].ToString();
                    Session["PreviousImage"] = objArticle.ImagePath;

                    ViewBag.imageSrc = objArticle.ImagePath;
                    Session["ArticleID"] = objArticle.ArticleID;
                    Session["isPublished"] = objArticle.isPublished;
                    ViewBag.pkid = int.Parse(objArticle.ArticleID.ToString());


                    return View(objArticle);
                }
                return View();
            }
            return RedirectToActionPermanent("Index", "Home");
        }


        [HttpPost]
        public ActionResult ArticleCreate(Article membervalues)
        {
            string previousImagePath = string.Empty;
            Article objArticleModel = new Article();
            //if(ModelState.IsValid)
            //{
            try
            {
                char checkifPublished = Session["isPublished"].ToString().ToCharArray()[0];
                if (checkifPublished == 'N')
                {
                    if (membervalues.ImageFile != null)
                    {
                        //Use Namespace called :  System.IO  
                        string FileName = Path.GetFileNameWithoutExtension(membervalues.ImageFile.FileName);

                        //To Get File Extension  
                        string FileExtension = Path.GetExtension(membervalues.ImageFile.FileName);

                        //Add Current Date To Attached File Name  
                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        //Get Upload path from Web.Config file AppSettings.  
                        //string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                        string UploadPath = Server.MapPath("~/Images/");

                        //Its Create complete path to store in server.  
                        membervalues.ImagePath = UploadPath + FileName;

                        //To copy and save file into server.  
                        membervalues.ImageFile.SaveAs(membervalues.ImagePath);


                        ImageBuilder.Current.Build(membervalues.ImagePath, membervalues.ImagePath,
                           new ResizeSettings("width=1450&height=820&crop=none"));


                        membervalues.ImagePath = "~/Images/" + FileName;

                        previousImagePath = Session["PreviousImage"] as string;
                        if (previousImagePath != null)
                        {
                            FileInfo file = new FileInfo(previousImagePath);
                            if (file.Exists)
                                file.Delete();
                        }

                        
                        //Uncomment For Debugging
                        //ViewBag.imageSrc = "~/Images/" + FileName;

                        ViewBag.imageSrc = membervalues.ImagePath;
                        Session["PreviousImage"] = membervalues.ImagePath;
                    }
                    else
                    {


                        if (int.Parse(Session["ArticleID"].ToString()) != 0)
                        {
                            ViewBag.imageSrc = Session["PreviousImage"] as string;
                            membervalues.ImagePath = Session["PreviousImage"] as string;
                        }
                        else
                        {
                            ViewBag.imageSrc = Session["PreviousImage"] as string;
                            ViewBag.FileStatus = "Please choose Image for the article";
                            return View();
                        }
                        //Uncomment for debugging
                        //previousImagePath = Session["Imagefile"] as string;                                                
                        //membervalues.ImagePath = Server.MapPath("/Images/") + previousImagePath;
                        //ViewBag.imageSrc = "~/Images/" + previousImagePath;
                    }

                    int ArticleID = int.Parse(Session["ArticleID"].ToString());
                    ViewBag.pkid = ArticleID;


                     

                    if (ArticleID == 0)
                    {
                        string dataSaved = objArticleModel.saveArticle(membervalues);
                        Session["ArticleID"] = dataSaved;
                        ViewBag.pkid = int.Parse(dataSaved);
                    }
                    else
                    {
                        
                        bool dataSaved = objArticleModel.updateArticle(membervalues);
                    }
                    return View();
                }
                else
                {
                    ViewBag.imageSrc = Session["PreviousImage"] as string;
                    ViewBag.FileStatus = "The Article is already published. Cant edit the file again.";
                }

            }
            catch (Exception ex)
            {

                ViewBag.FileStatus = "Error while saving the file" + ex.Message;
            }
            //}
            return View();
        }


        public ActionResult ArticleGrid(string sectionID)
        {
            userInfo objUserDetails = new userInfo();
            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";

                Article objArticle = new Article();
                DataTable emps = objArticle.getArticles(sectionID);

                ViewBag.ArticleList = emps;
                return View();
            }
            return RedirectToActionPermanent("Index", "Home");

        }


        
        public ActionResult DeleteGrid()
        {
            //Article objArticle = new Article();
            //if (id != null)
            //{
            //    bool isDeleted = objArticle.deleteArticle(id);
            //}

            if (Session["objUserInSeesion"] != null)
            {
                Session["objUserInseesion"] = null;
            }

                return RedirectToAction("Index", "Home");
            
        }

    }
}