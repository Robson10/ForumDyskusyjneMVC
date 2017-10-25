using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumMVC.Controllers
{
    public class LogowanieController : Controller
    {
        Models.UserEntities dbUser = new Models.UserEntities();
        // GET: Logowanie
        public ActionResult Index()
        {
            try
            {
                Session["UserID"] = null;
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.UZYTKOWNIK objUser)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if(dbUser.UZYTKOWNIK.Any(x=>x.UZ_login==objUser.UZ_login&& x.UZ_haslo==objUser.UZ_haslo))
                    {
                        var temp = dbUser.UZYTKOWNIK.First(x => x.UZ_login == objUser.UZ_login && x.UZ_haslo == objUser.UZ_haslo);
                        Session["UserID"] = temp.UZ_id;
                        Session["Pseudonim"] = temp.UZ_pseudonim;
                        return RedirectToAction("Index","User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Login data is incorrect!");
                    }
                }
            }
            return View(objUser);
        }
    }
}