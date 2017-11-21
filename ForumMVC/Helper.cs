using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ForumMVC
{
    public class Helper
    {
        static string ConnectionString = @"Data Source=DESKTOP-M4NPT9R; Initial Catalog=FORUM_DYSKUSYJNE;Integrated Security=SSPI";
        public static DataSet SqlSelect(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    conn.Close();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
                catch (Exception ex)
                { return null; }

            }
        }

        public static void SqlInsert(string query)
        {
            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}