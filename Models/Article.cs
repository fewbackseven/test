using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBlayer;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
//using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Article
    {
        string sBaseType = "C";

        string sConString = ConfigurationManager.ConnectionStrings["Easy"].ConnectionString;

        public int ArticleID { get; set; }

        //To change label title value  
        [DisplayName("Article Heading")]
        [MaxLength(100)]
        public string ArticleHeading { get; set; }

       

        //To change label title value  
        [MaxLength(50)]
        [DisplayName("Author Name")]
        public string AuthorName { get; set; }

        //To change label title value  
        [DisplayName("Upload Image")]
        public string ImagePath { get; set; }


        [DataType(DataType.MultilineText)]
        [DisplayName("Article Paragraph content 2")]
        public string ArticleParagraph1 { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Article Paragraph content 2")]
        public string  ArticleParagraph2 { get; set; }

        public char isPublished { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public DateTime Articledate { get; set; }


        public DataTable getArticles()
        {
            string sSql = string.Empty;
            DataTable dtYear = new DataTable();
            DBHelper ObjDBHelper;
            try
            {
                ObjDBHelper = new DBHelper();
                sSql = "select * from [dbo].[Mas_Articles]";
                dtYear = ObjDBHelper.DBExecDataTable(sConString, sSql);
            }
            catch (Exception ex)
            {
                //LogError(ex.Message, Convert.ToString(System.Web.HttpContext.Current.Session["Loccode"]), "Login", System.Reflection.MethodBase.GetCurrentMethod().Name, sSql, sBaseType);
               
            }
            return dtYear;
        }

        public DataTable getArticle(int v)
        {
            string sSql = string.Empty;
            DataTable dtYear = new DataTable();
            DBHelper ObjDBHelper;
            try
            {
                ObjDBHelper = new DBHelper();
                sSql = "select * from [dbo].[Mas_Articles] where Art_Pkid="+v+"";
                dtYear = ObjDBHelper.DBExecDataTable(sConString, sSql);
            }
            catch (Exception ex)
            {
                //LogError(ex.Message, Convert.ToString(System.Web.HttpContext.Current.Session["Loccode"]), "Login", System.Reflection.MethodBase.GetCurrentMethod().Name, sSql, sBaseType);

            }
            return dtYear;
            throw new NotImplementedException();
        }

        public bool saveArticle( Article article)
        {
            string sSql = string.Empty;
            bool sResult = false;
            DBHelper objdbHelper;
            try
            {
                objdbHelper = new DBHelper();
                sSql = "Insert into [Mas_Articles] values('" + article.ArticleHeading + "','" + article.ArticleParagraph1 + "','" + article.ArticleParagraph2 + "','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "','N','" + article.AuthorName + "','" + article.ImagePath + "')";
                sResult = objdbHelper.DBExecuteNoNQuery(sConString, sSql);
                            
            }
            catch(Exception ex)
            {

            }
            return sResult;
        }
    }
}