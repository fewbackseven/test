using DBlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class userInfo
    {

        string sConString = ConfigurationManager.ConnectionStrings["Easy"].ConnectionString;

        public int userID { get; set; }


        [DisplayName("Enter your Full Name")]
        [Required(ErrorMessage ="Please enter your name here")]
        [StringLength(100)]
        public string fullName { get; set; }

        [DisplayName("Enter your Email Id")]
        [Required(ErrorMessage = "Please Enter your email-ID for next time Login")]
        [StringLength(100)]
        public string usrEmailID { get; set; }


        [DisplayName("Create Password")]       
        [StringLength(100)]
        [Required(ErrorMessage = "Please create password")]
        public string usrPassWord { get; set; }


        [DisplayName("Confirm your Password")]
        [Required(ErrorMessage = "Please create password")]              
        public string cnfPassWord { get; set; }


        [StringLength(10)]
        [DisplayName("Enter your Contact Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string usrPhoneNumber { get; set; }


        [DisplayName("Enter your Age")]        
        [IntegerValidator]
        public int usrAge { get; set; }


        [DisplayName("Select your Gender:")]
        public string usrGender { get; set; }


        [DisplayName("Enter your occupation")]
        public string usrOccupation { get; set; }

        
        public string usrRemarks { get; set; }


        public DateTime usrCreatedDate { get; set; }


        public string usrType { get; set; }

        public string createUser(userInfo objUsrInfo)
        {
            string sSql = string.Empty;
            string pkid = string.Empty;
            bool sResult = false;
            DBHelper objdbHelper;
            try
            {
                objdbHelper = new DBHelper();
                sSql = "Insert into [Mas_UsrMaster] values('" + objUsrInfo.fullName + "', '" + objUsrInfo.usrEmailID + "', '" + objUsrInfo.usrPassWord + "', " + objUsrInfo.usrPhoneNumber + ", " + objUsrInfo.usrAge + ", '" + objUsrInfo.usrGender + "' , '" + objUsrInfo.usrOccupation + "','', '" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "', 'U'  )";
                sResult = objdbHelper.DBExecuteNoNQuery(sConString, sSql);
                if (sResult)
                {
                    sSql = "select Max(usr_pkid) from [dbo].[Mas_UsrMaster]";
                    pkid = objdbHelper.DBExecuteScalar(sConString, sSql).ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return pkid;
        }


        public DataTable getUser(string usrEmailID, string usrPassWord)
        {
            string sSql = string.Empty;
            DataTable dtUser = new DataTable();
            DBHelper objDBHelper = new DBHelper();

            try
            {
                sSql = "select * from Mas_UsrMaster where usr_EmailID = '" + usrEmailID + "' and usr_PassWord='" + usrPassWord + "'";
                dtUser = objDBHelper.DBExecDataTable(sConString, sSql);
            }
            catch(Exception ex)
            {

            }

            return dtUser;
        }

    }
}