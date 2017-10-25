using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumMVC.Models;

namespace ForumMVC.Controllers
{
    public class HomeController : Controller
    {
        private ConnectionString _dbKategoria = new ConnectionString();
        public ActionResult GuestIndex()
        {
            return View(_dbKategoria.KATEGORIA.ToList());
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
                    czyjostatnipost= Helper.SqlSelect(query2.Replace("@param", ds.Tables[0].Rows[i][0].ToString())).Tables[0].Rows[0][1].ToString()
                });
            }
            return View(temp);
        }
        public ActionResult SelectedTopic(int id, string topicName)
        {
            Session["Temat"] = topicName;
            string query = " SELECT " +
                "WIADOMOSC.WI_id, UZYTKOWNIK.UZ_pseudonim, WIADOMOSC.TE_id, WIADOMOSC.WI_tresc, WIADOMOSC.WI_data " +
                 "FROM WIADOMOSC "+
                 "inner join UZYTKOWNIK on UZYTKOWNIK.UZ_id = WIADOMOSC.UZ_id "+
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

    }
}