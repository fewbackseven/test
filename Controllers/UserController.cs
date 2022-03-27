using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class UserController : Controller
    {
        userInfo objUsrInfo = new userInfo();
        // GET: User
        public ActionResult userLogin()
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


            return View();
        }

        [HttpPost]
        public ActionResult userLogin(userInfo objUsrInfoFromPage)
        {
            DataTable dtUser = new DataTable();

            try
            {
                dtUser = objUsrInfo.getUser(objUsrInfoFromPage.usrEmailID, objUsrInfoFromPage.usrPassWord);
                if(dtUser.Rows.Count>0)
                {
                    objUsrInfo.fullName = dtUser.Rows[0]["usr_FullName"].ToString();
                    objUsrInfo.usrType = dtUser.Rows[0]["usr_Type"].ToString();
                    Session["objUserInSeesion"] = objUsrInfo;
                    return RedirectToActionPermanent("Index", "Home");
                }
                else
                {
                    TempData["msg"] = "<script>alert('Error! Couldnt login, Please Sign UP.');</script>";
                    return View();
                }
                
            }
            catch
            {
                return View(objUsrInfo);
            }
            
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult CreateUser()
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

            return View(objUsrInfo);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult CreateUser(userInfo objUsrInfo)
        {
            try
            {
                if(objUsrInfo.usrPassWord== objUsrInfo.cnfPassWord)
                {
                    string usrID = objUsrInfo.createUser(objUsrInfo);
                    if (usrID != "")
                    { 
                        return RedirectToActionPermanent("Index", "Home"); 
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Error Couldnt create the user');</script>";
                        return View();
                    }
                }
                else
                {
                    TempData["msg"] = "<script>alert('Passwords are not matching');</script>";
                    return View();
                }
                // TODO: Add insert logic here

                
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
