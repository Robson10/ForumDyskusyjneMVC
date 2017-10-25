using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumMVC.Models
{
    public class TematItem
    {
        public int id;
        public string nazwa;
        public int idKategorii;
        public string ostatnipost;
        public string czyjostatnipost;
    }
}