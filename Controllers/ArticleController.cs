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
                objArticle.pageSectionID = int.Parse(article.Rows[0]["Art_Section"].ToString());
                objArticle.newsSectionID = int.Parse(article.Rows[0]["Art_NewsSection"].ToString());
                ViewBag.ArticleID = objArticle.ArticleID;
                string filePath = article.Rows[0]["Art_ImagePath"].ToString();

                //String serverPath = Server.MapPath("/Images/");
                //String relativePath = filePath.Substring(serverPath.Length, filePath.Length - serverPath.Length);
                //objArticle.ImagePath = "~/Images/" + relativePath;
                //Session["PreviousImage"] = objArticle.ImagePath;                
                //ViewBag.imageSrc = objArticle.ImagePath;
                ViewBag.Art_Section = objArticle.pageSectionID;
                ViewBag.imageSrc = filePath;
                Session["ArticleID"] = objArticle.ArticleID;
                Session["isPublished"] = objArticle.isPublished;


                return View(objArticle);

            }

            return View();
        }



        public ActionResult ArticleCreate(string Article_id, string Art_Section)
        {
            userInfo objUserDetails = new userInfo();
            Article objArticle = new Article();
            DataTable dtNewsSections = new DataTable();
            List<SelectListItem> newsItems = new List<SelectListItem>();

            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                ViewBag.loginMonth = objUserDetails.loginMonthName;
                Session["loginMonth"] = objUserDetails.loginMonthName;
                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";

                dtNewsSections = objArticle.getNewsSections();
                foreach (DataRow dr in dtNewsSections.Rows)
                {
                    newsItems.Add(new SelectListItem { Value = dr["NS_pkid"].ToString(), Text = dr["NS_name"].ToString() });
                }

                ViewBag.newsSections = newsItems;

                ViewBag.pkid = 0;
                //ViewBag.User = "Admin";
                ViewBag.Art_Section = Art_Section;
                Session["Art_Sections"] = Art_Section;
                Session["isPublished"] = 'N';
                Session["ArticleID"] = "0";
                Session["PreviousImage"] = null;



                if (int.Parse(Article_id) != 0)
                {

                    DataTable article = objArticle.getArticle(int.Parse(Article_id));
                    objArticle.ArticleHeading = article.Rows[0]["Art_Heading"].ToString();
                    objArticle.ArticleParagraph1 = article.Rows[0]["Art_Paragraph1"].ToString();
                    objArticle.AuthorName = article.Rows[0]["Art_AuthorName"].ToString();
                    objArticle.ArticleParagraph2 = article.Rows[0]["Art_Paragraph2"].ToString();
                    objArticle.ArticleID = int.Parse(article.Rows[0]["Art_pkid"].ToString());
                    objArticle.isPublished = article.Rows[0]["Art_isPublished"].ToString().ToCharArray()[0];
                    objArticle.pageSectionID = int.Parse(article.Rows[0]["Art_Section"].ToString());
                    objArticle.newsSectionID = int.Parse(article.Rows[0]["Art_NewsSection"].ToString());
                    dtNewsSections = objArticle.getNewsSections();
                    foreach (DataRow dr in dtNewsSections.Rows)
                    {
                        if (int.Parse(dr["NS_pkid"].ToString()) != int.Parse(article.Rows[0]["Art_NewsSection"].ToString()))
                            newsItems.Add(new SelectListItem { Value = dr["NS_pkid"].ToString(), Text = dr["NS_name"].ToString() });
                        else
                            newsItems.Add(new SelectListItem { Value = dr["NS_pkid"].ToString(), Text = dr["NS_name"].ToString(), Selected = true });
                    }

                    ViewBag.newsSections = newsItems;

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
        public ActionResult ArticleCreate(Article membervalues, string newsSections)
        {
            string previousImagePath = string.Empty;
            Article objArticleModel = new Article();
            userInfo objGetMonths = new userInfo();
            //if(ModelState.IsValid)
            //{
            try
            {
                char checkifPublished = Session["isPublished"].ToString().ToCharArray()[0];
                if (checkifPublished == 'N')
                {
                    membervalues.monthID = int.Parse(objGetMonths.getLoginMonthsForSelection(Session["loginMonth"].ToString()).Rows[0]["Month_pkid"].ToString());
                    membervalues.pageSectionID = int.Parse(Session["Art_Sections"] as string);

                    membervalues.newsSectionID = int.Parse(newsSections);

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
                        return RedirectToAction("ArticleGrid", new { sectionID = "1" });
                    }
                    else
                    {

                        bool dataSaved = objArticleModel.updateArticle(membervalues);
                        return RedirectToAction("ArticleGrid", new { sectionID = "1" });
                    }

                }
                else
                {
                    ViewBag.imageSrc = Session["PreviousImage"] as string;
                    ViewBag.FileStatus = "The Article is already published. Cant edit the file again.";
                }

            }
            catch (Exception ex)
            {

                ViewBag.FileStatus = "Error: " + ex.Message;
            }
            //}
            return View();
        }


        public ActionResult ArticlePublish(string Article_id, string Art_Section)
        {
            userInfo objUserDetails = new userInfo();
            Article objArticle = new Article();
            List<SelectListItem> listOfDate = new List<SelectListItem>();
            List<SelectListItem> listOfMonth = new List<SelectListItem>();
            List<SelectListItem> listOfYears = new List<SelectListItem>();
            List<SelectListItem> listOfHours = new List<SelectListItem>();
            List<SelectListItem> listOfMinutes = new List<SelectListItem>();
            List<SelectListItem> listOfSeconds = new List<SelectListItem>();
            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                ViewBag.loginMonth = objUserDetails.loginMonthName;
                Session["loginMonth"] = objUserDetails.loginMonthName;
                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";

                if (int.Parse(Article_id) != 0)
                {

                    DataTable article = objArticle.getArticle(int.Parse(Article_id));
                    objArticle.ArticleHeading = article.Rows[0]["Art_Heading"].ToString();
                    objArticle.ArticleParagraph1 = article.Rows[0]["Art_Paragraph1"].ToString();
                    objArticle.AuthorName = article.Rows[0]["Art_AuthorName"].ToString();
                    objArticle.ArticleParagraph2 = article.Rows[0]["Art_Paragraph2"].ToString();
                    objArticle.ArticleID = int.Parse(article.Rows[0]["Art_pkid"].ToString());
                    objArticle.isPublished = article.Rows[0]["Art_isPublished"].ToString().ToCharArray()[0];
                    objArticle.articlePublishDate = DateTime.Now;
                    ViewBag.articlePkid = Article_id;
                    int todaysDate = DateTime.Now.Day;
                    int todaysMonth = DateTime.Now.Month;
                    int todaysYear = DateTime.Now.Year;
                    int todaysHour = DateTime.Now.Hour;
                    int todaysMinute = DateTime.Now.Minute;

                    for (int i = 1; i < 32; i++)
                    {
                        if (i != todaysDate)
                            listOfDate.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                        else
                            listOfDate.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = true });
                    }
                    ViewBag.ddlDate = listOfDate;

                    for (int i = 1; i <= 12; i++)
                    {
                        if (i != todaysMonth)
                            listOfMonth.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                        else
                            listOfMonth.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = true });
                    }
                    ViewBag.ddlMonth = listOfMonth;


                    for (int i = 1990; i <= 2050; i++)
                    {
                        if (i != todaysYear)
                            listOfYears.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                        else
                            listOfYears.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = true });
                    }
                    ViewBag.ddlYear = listOfYears;


                    for (int i = 1; i <= 60; i++)
                    {
                        if (i != todaysMinute)
                            listOfMinutes.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                        else
                            listOfMinutes.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = true });

                        listOfSeconds.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                    }
                    ViewBag.ddlMinutes = listOfMinutes;
                    ViewBag.ddlSeconds = listOfSeconds;


                    for (int i = 0; i <= 24; i++)
                    {
                        if (i != todaysHour)
                            listOfHours.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                        else
                            listOfHours.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = true });
                    }
                    ViewBag.ddlHours = listOfHours;


                }
                return View(objArticle);
            }
            return View();
        }


        [HttpPost]
        public ActionResult ArticlePublish(Article membervalues, string ddlDate, string ddlMonth, string ddlYear, string ddlHours, string ddlMinutes, string ddlSeconds)
        {
            Article objArticleModel = new Article();
            membervalues.articlePublishDate = DateTime.Now;
            bool isPublished = false;
            //+		membervalues.articlePublishDate	{27-04-2022 00:09:33}	System.DateTime

            string toBePublishedDate = ddlDate.ToString() + "-" + ddlMonth.ToString() + "-" + ddlYear.ToString() + "  " +  ddlHours.ToString() + ":" + ddlMinutes.ToString() + ":" + ddlSeconds.ToString();
            membervalues.articlePublishDate = DateTime.Parse(toBePublishedDate);

            isPublished = objArticleModel.publishArticle(membervalues.ArticleID, membervalues.articlePublishDate);
            return RedirectToAction("ArticleGrid", new { sectionID = "1" });

        }

        public ActionResult ArticleGrid(string sectionID)
        {
            userInfo objUserDetails = new userInfo();
            string monthID = string.Empty;
            ViewBag.Art_Section = sectionID;
            ViewBag.sectionName = getSectionName(sectionID);
            Session["Art_Sections"] = sectionID;
            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                ViewBag.loginMonth = objUserDetails.loginMonthName;
                if (objUserDetails.getLoginMonthsForSelection(objUserDetails.loginMonthName.ToString()).Rows.Count > 0)
                    monthID = objUserDetails.getLoginMonthsForSelection(objUserDetails.loginMonthName.ToString()).Rows[0]["Month_pkid"].ToString();

                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";

                Article objArticle = new Article();
                DataTable emps = objArticle.getArticles(sectionID, monthID);

                ViewBag.ArticleList = emps;
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        private string getSectionName(string sectionID)
        {
            string res = "";
            if (sectionID == "1") { res = "Latest News"; } else if (sectionID == "2") { res = "Flash News"; } else if (sectionID == "3") { res = "Art & Literature"; } else if (sectionID == "4") { res = "Cinema"; } else if (sectionID == "5") { res = "Sports"; }
            return res;
        }

        public ActionResult sectionPage(string newsSectionID)
        {
            userInfo objUserDetails = new userInfo();
            if (Session["objUserInSeesion"] != null)
            {
                objUserDetails = Session["objUserInSeesion"] as userInfo;
                ViewBag.usrName = objUserDetails.fullName.ToString();
                ViewBag.loginMonth = objUserDetails.loginMonthName;
                if (objUserDetails.usrType == "A")
                    ViewBag.User = "Admin";
                else
                    ViewBag.User = "User";
            }

            Article objArticle = new Article();
            ViewBag.sectionName = getSectionName(newsSectionID);
            DataTable emps = objArticle.getNewsArticles(newsSectionID);
            ViewBag.ArticleList = emps;

            return View();
        }


        [HttpPost]
        public ActionResult DeleteGrid()
        {
            Article objarticle = new Article();
            string id = ViewBag.ArticleID;
            if (id != null)
            {
                bool isdeleted = objarticle.deleteArticle(id);
            }

            //if (Session["objUserInSeesion"] != null)
            //{
            //    Session["objUserInseesion"] = null;
            //}

            return RedirectToAction("Index", "Home");

        }




    }
}