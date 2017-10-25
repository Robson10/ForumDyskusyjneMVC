using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumMVC.Models
{
    public class MessageItem
    {
        public int id;
        public string Uzytkownik;
        public int idTematu;
        public string tresc;
        public string data;
    }
}