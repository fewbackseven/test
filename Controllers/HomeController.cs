using System;
using System.Collections.Generic;
using System.Data;
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


            DataTable emps = objArticle.getArticles();
            string filePath = emps.Rows[4]["Art_ImagePath"].ToString();
            //String serverPath = Request.PhysicalPath;
            String serverPath = Server.MapPath("/Images/");
            String relativePath = filePath.Substring(serverPath.Length, filePath.Length - serverPath.Length);
            ViewBag.imgSrc = "~/Images/"+ relativePath;
            //ViewBag.imgSrc = "~/Content/Images/dashboard/banner.jpg";
            ViewBag.ArticleHeading = emps.Rows[3]["Art_Heading"].ToString();
            ViewBag.User = "admin";
            return View();

        }

        public ActionResult About(string sType)
        {
            ViewBag.Message = "Your application description page.";
            Article objArticle = new Article();


            DataTable emps = objArticle.getArticles();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}