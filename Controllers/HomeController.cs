using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            Article objArticle = new Article();
            userInfo objGetMonthInfo = new userInfo();

            int currentMonth = DateTime.Now.Month;
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth);
            string currentYear = DateTime.Now.Year.ToString();
            string monthID = objGetMonthInfo.getLoginMonthsForSelection(monthName + "-" + currentYear).Rows[0]["Month_pkid"].ToString();

            //For Latest News section, ID=1
            DataTable emps = objArticle.getPublishedArticles("1", monthID);
            ViewBag.ArticleList = emps;

            DataTable Flash_News = objArticle.getPublishedArticles("2", monthID);
            ViewBag.flashNewsList = Flash_News;

            DataTable sportsSection = objArticle.getArticlesNewsSectionWise("5", monthID);
            ViewBag.sportsNewsSection = sportsSection;

            DataTable cinemaSection = objArticle.getArticlesNewsSectionWise("4", monthID);
            ViewBag.cinemaNewsSection = cinemaSection;

            //string filePath = emps.Rows[0]["Art_ImagePath"].ToString();
            //ViewBag.imgSrc = filePath;
            //ViewBag.ArticleHeading = emps.Rows[0]["Art_Heading"].ToString();
            //ViewBag.ArticleId = emps.Rows[0]["Art_pkid"].ToString();

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
            
            return View();



            //String serverPath = Request.PhysicalPath;
            //String serverPath = Server.MapPath("~/Images/");
            //String relativePath = filePath.Substring(serverPath.Length, filePath.Length - serverPath.Length);

            //ViewBag.imgSrc = "~/Content/Images/dashboard/banner.jpg";

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

        public ActionResult About(string sType)
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



            ViewBag.Message = "Your application description page.";
            Article objArticle = new Article();


            DataTable emps = objArticle.getArticles("1","1");

            return View();
        }

        public ActionResult Contact()
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

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}