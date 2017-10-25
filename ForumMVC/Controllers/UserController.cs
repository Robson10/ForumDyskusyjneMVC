using ForumMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumMVC.Controllers
{
    public class UserController : Controller
    {
        private ConnectionString _dbKategoria = new ConnectionString();
        private UserEntities dbUzytkownik = new UserEntities();
        private TopicEntities dbTemat = new TopicEntities();
        // GET: User
        public ActionResult Index()
        {
            return View(_dbKategoria.KATEGORIA.ToList());
        }

        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(KATEGORIA value)
        {
            if (ModelState.IsValid)
            {
                _dbKategoria.KATEGORIA.Add(value);
                _dbKategoria.SaveChanges();
                return RedirectToAction("Index","User");
            }
            else
            {
                return View(value);
            }
        }

        public ActionResult Settings()
        {
            int id = (int)Session["UserID"];
            var temp = (from UZYTKOWNIK in dbUzytkownik.UZYTKOWNIK where UZYTKOWNIK.UZ_id == id select UZYTKOWNIK).First();
            return View(temp);
        }
        [HttpPost]
        public ActionResult Settings(UZYTKOWNIK uzytkownikToEdit)
        {
            int id = (int)Session["UserID"];
            var temp=((from UZYTKOWNIK in dbUzytkownik.UZYTKOWNIK where UZYTKOWNIK.UZ_id == id select UZYTKOWNIK).First());
            if (!ModelState.IsValid) return View(temp);

            dbUzytkownik.Entry(temp).CurrentValues.SetValues(uzytkownikToEdit);
            dbUzytkownik.SaveChanges();
            return RedirectToAction("Index","User");
        }
        public ActionResult DeleteAccount()
        {
            int id = (int)Session["UserID"];
            var temp = (from UZYTKOWNIK in dbUzytkownik.UZYTKOWNIK where UZYTKOWNIK.UZ_id == id select UZYTKOWNIK).First();
            dbUzytkownik.Entry(temp).State = System.Data.Entity.EntityState.Deleted;
            dbUzytkownik.SaveChanges();
            return RedirectToAction("Index", "Logowanie");
        }

        public ActionResult SelectedCategory(int id, string categoryName)
        {
            Session["Kategoria"] = categoryName;
            string query = " SELECT TE_id,KA_id,TE_nazwa FROM TEMAT where KA_id = " + id;
            string query2 = "SELECT top 1 WIADOMOSC.WI_data,UZYTKOWNIK.UZ_pseudonim FROM TEMAT " +
                            "inner join WIADOMOSC on WIADOMOSC.TE_id = TEMAT.TE_id " +
                            "inner join UZYTKOWNIK on WIADOMOSC.UZ_id = UZYTKOWNIK.UZ_id " +
                            "where TEMAT.TE_id = @param " +
                            "order by WIADOMOSC.WI_data desc";
            var ds = Helper.SqlSelect(query);
            List<TematItem> temp = new List<TematItem>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                temp.Add(new TematItem()
                {
                    id = (int)ds.Tables[0].Rows[i][0],
                    idKategorii = (int)ds.Tables[0].Rows[i][1],
                    nazwa = ds.Tables[0].Rows[i][2].ToString(),
                    ostatnipost = Helper.SqlSelect(query2.Replace("@param", ds.Tables[0].Rows[i][0].ToString())).Tables[0].Rows[0][0].ToString(),
                    czyjostatnipost = Helper.SqlSelect(query2.Replace("@param", ds.Tables[0].Rows[i][0].ToString())).Tables[0].Rows[0][1].ToString()
                });
            }
            return View(temp);
        }
        public ActionResult SelectedTopic(int id, string topicName)
        {
            Session["Temat"] = topicName;
            string query = " SELECT " +
                "WIADOMOSC.WI_id, UZYTKOWNIK.UZ_pseudonim, WIADOMOSC.TE_id, WIADOMOSC.WI_tresc, WIADOMOSC.WI_data " +
                 "FROM WIADOMOSC " +
                 "inner join UZYTKOWNIK on UZYTKOWNIK.UZ_id = WIADOMOSC.UZ_id " +
                 "where WIADOMOSC.TE_id= " + id;
            string query2 = "SELECT top 1 WIADOMOSC.WI_data,UZYTKOWNIK.UZ_pseudonim FROM TEMAT " +
                            "inner join WIADOMOSC on WIADOMOSC.TE_id = TEMAT.TE_id " +
                            "inner join UZYTKOWNIK on WIADOMOSC.UZ_id = UZYTKOWNIK.UZ_id " +
                            "where TEMAT.TE_id = @param " +
                            "order by WIADOMOSC.WI_data desc";
            var ds = Helper.SqlSelect(query);
            var temp = new List<MessageItem>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                temp.Add(new MessageItem()
                {
                    id = (int)ds.Tables[0].Rows[i][0],
                    Uzytkownik = ds.Tables[0].Rows[i][1].ToString(),
                    idTematu = (int)ds.Tables[0].Rows[i][2],
                    tresc = ds.Tables[0].Rows[i][3].ToString(),
                    data = ds.Tables[0].Rows[i][4].ToString()
                });
            }
            return View(temp);
        }

        //CreateTopic
        public ActionResult CreateTopic()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTopic( string temat, string wiadomosc)
        {
            var value = new NewTopicItem();
            value.NazwaTematu = temat;
            value.trescWiadomosci = wiadomosc;
            var name = (string)Session["Kategoria"];
            value.idUzytkownika = (int)Session["UserID"];
            value.data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            value.idKategorii = (int)Helper.SqlSelect("Select KATEGORIA.KA_id from KATEGORIA where KATEGORIA.KA_nazwa='" + name + "'").Tables[0].Rows[0][0];
            //insert Temat
            Helper.SqlInsert("insert into TEMAT values("+value.idKategorii+",'"+ value.NazwaTematu+"')");
            value.idTemat= (int)Helper.SqlSelect("Select TEMAT.TE_id from TEMAT where TEMAT.TE_nazwa='" + value.NazwaTematu + "'").Tables[0].Rows[0][0];


            Helper.SqlInsert("insert into WIADOMOSC values("+value.idUzytkownika+","+value.idTemat+",'"+value.trescWiadomosci+"','"+value.data+"')");
            return RedirectToAction("Index", "User");
        }
        //CreateMessage

        public ActionResult CreateMessage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMessage(string wiadomosc)
        {
            var value = new NewTopicItem();

            var name = (string)Session["Temat"];
            value.idTemat = (int)Helper.SqlSelect("Select TEMAT.TE_id from TEMAT where TEMAT.TE_nazwa='" + name + "'").Tables[0].Rows[0][0];

            value.trescWiadomosci = wiadomosc;
            value.idUzytkownika = (int)Session["UserID"];
            value.data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Helper.SqlInsert("insert into WIADOMOSC values(" + value.idUzytkownika + "," + value.idTemat + ",'" + value.trescWiadomosci + "','" + value.data + "')");
            return RedirectToAction("Index", "User");
        }
    }
}