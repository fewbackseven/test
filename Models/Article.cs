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
using System.Web.Mvc;

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
        [DisplayName("Article Paragraph content 1")]
        [AllowHtml]
        public string ArticleParagraph1 { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Article Paragraph content 2")]
        public string  ArticleParagraph2 { get; set; }

        public char isPublished { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public DateTime Articledate { get; set; }


        public DataTable getArticles(string sectionID)
        {
            string sSql = string.Empty;
            DataTable dtYear = new DataTable();
            DBHelper ObjDBHelper;
            try
            {
                ObjDBHelper = new DBHelper();
                sSql = "select * from [dbo].[Mas_Articles] where Art_isPublished = 'Y' and Art_Section='" + sectionID + "' order by Art_pkid desc ";
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

        public string saveArticle( Article article)
        {
            string sSql = string.Empty;
            string pkid = string.Empty;
            bool sResult = false;
            DBHelper objdbHelper;
            try
            {
                objdbHelper = new DBHelper();
                sSql = "Insert into [Mas_Articles] values( N'" + article.ArticleHeading + "', N'" + article.ArticleParagraph1 + "', N'" + article.ArticleParagraph2 + "','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "','N', N'" + article.AuthorName + "','" + article.ImagePath + "', 1)";
                sResult = objdbHelper.DBExecuteNoNQuery(sConString, sSql);
                if (sResult)
                {
                    sSql = "select Max(Art_pkid) from [dbo].[Mas_Articles]";
                    pkid = objdbHelper.DBExecuteScalar(sConString, sSql).ToString();
                }
                            
            }
            catch(Exception ex)
            {

            }
            return pkid;
        }

        public bool updateArticle(Article article)
        {
            string sSql = string.Empty;
            bool sResult = false;
            DBHelper objdbHelper;
            try
            {
                objdbHelper = new DBHelper();
                sSql = "Update [Mas_Articles] set  Art_Heading = N'" + article.ArticleHeading + "',  Art_Paragraph1 = N'" + article.ArticleParagraph1 + "', Art_Paragraph2 = N'" + article.ArticleParagraph2 + "',  Art_AuthorName = N'" + article.AuthorName + "',  Art_ImagePath = '" + article.ImagePath + "'  where Art_pkid = " + article.ArticleID + "";
                sResult = objdbHelper.DBExecuteNoNQuery(sConString, sSql);

            }
            catch (Exception ex)
            {

            }
            return sResult;
        }

        public bool deleteArticle(string id)
        {
            string sSql = string.Empty;
            bool sResult = false;
            DBHelper objDBHelper = new DBHelper();
            try
            {
                sSql = "Delete from [Mas_Articles] where Art_pkid = " + id + "";
                sResult = objDBHelper.DBExecuteNoNQuery(sConString, sSql);
            }
            catch(Exception ex)
            {

            }
            return sResult;
        }
    }
}