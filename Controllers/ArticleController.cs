using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using System.IO;
using System.Configuration;
using System.Data;

namespace test.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult ArticleView()
        {

            return View();
        }


        
        public ActionResult ArticleCreate(string id)
        {
            if(int.Parse(id)!=0)
            {
                Article objArticle = new Article();
                DataTable article = objArticle.getArticle(int.Parse(id));
                objArticle.ArticleHeading = article.Rows[0]["Art_Heading"].ToString();
                objArticle.ArticleParagraph1 = article.Rows[0]["Art_Paragraph1"].ToString();
                objArticle.AuthorName = article.Rows[0]["Art_AuthorName"].ToString();
                objArticle.ArticleParagraph2 = article.Rows[0]["Art_Paragraph2"].ToString();
                objArticle.ArticleID = int.Parse(article.Rows[0]["Art_pkid"].ToString());
                
                string filePath = article.Rows[0]["Art_ImagePath"].ToString();
                String serverPath = Server.MapPath("/Images/");
                String relativePath = filePath.Substring(serverPath.Length, filePath.Length - serverPath.Length);
                objArticle.ImagePath = "~/Images/" + relativePath;

                return View(objArticle);
            }
            return View();
        }


        [HttpPost]
        public ActionResult ArticleCreate(Article membervalues)
        {
            Article objArticleModel = new Article();
            if(ModelState.IsValid)
            {
                try
                {
                    //Use Namespace called :  System.IO  
                    string FileName = Path.GetFileNameWithoutExtension(membervalues.ImageFile.FileName);

                    //To Get File Extension  
                    string FileExtension = Path.GetExtension(membervalues.ImageFile.FileName);

                    //Add Current Date To Attached File Name  
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                    //Get Upload path from Web.Config file AppSettings.  
                    //string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                    string UploadPath = Server.MapPath("/Images/");
                 
                    //Its Create complete path to store in server.  
                    membervalues.ImagePath = UploadPath + FileName;

                    //To copy and save file into server.  
                    membervalues.ImageFile.SaveAs(membervalues.ImagePath);

                    bool dataSaved = objArticleModel.saveArticle(membervalues);
                    
                    return View();

                }
                catch(Exception ex)
                {
                    ViewBag.FileStatus = "Error while saving the file";
                }
            }
            return View();
        }


        public ActionResult ArticleGrid()
        {
            Article objArticle = new Article();
            DataTable emps = objArticle.getArticles();

            ViewBag.ArticleList = emps;
            return View();
        }

    }
}