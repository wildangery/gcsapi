using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace gcsapi
{
    public class Settings
    {
        public static string ConnectionString = "Server=alta.telkom.space;Initial Catalog=GCS;User Id=gcs;Password=telk0mS4t!";



        public static SqlConnection sqlCon = new SqlConnection(Settings.ConnectionString);

        public static DataSet LoadDataSet(string Sql)
        {
            SqlCommand cmd = new SqlCommand(Sql, sqlCon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();
            return ds;
        }

        public static string ToMD5Hash(string source)
        {
            StringBuilder sb = new StringBuilder();

            using (MD5 md5 = MD5.Create())
            {
                byte[] md5HashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

                foreach (byte b in md5HashBytes)
                {
                    sb.Append(b.ToString("X2")); // print byte as Hexadecimal string
                }
            }

            return sb.ToString();
        }

        public static SqlCommand ExNonQuery(string Sql)
        {
            sqlCon.Open();
            SqlCommand sqlCmd1 = new SqlCommand(Sql, sqlCon);
            sqlCmd1.ExecuteNonQuery();
            sqlCon.Close();
            return sqlCmd1;
        }

        public static string ExecuteScalar(string sql)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            string output = cmd.ExecuteScalar().ToString();
            sqlCon.Close();
            return output;
        }

        public static string ExsecuteSql2(string sql)
        {
            string s = null;
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            try
            {
                sqlCon.Open();
                cmd.ExecuteNonQuery();

                s = "";
            }
            catch (Exception err)
            {
                s = err.Message;
            }
            finally
            {
                sqlCon.Close();
                if (cmd != null)
                {
                    cmd.Dispose();
                }


            }
            return s;
        }


        public static string ExsecuteSql(string sql)
        {
            string s = null;
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            try
            {
                sqlCon.Open();
                cmd.ExecuteNonQuery();

                s = "";
            }
            catch (Exception err)
            {
                s = err.Message;
            }
            finally
            {
                sqlCon.Close();
                if (cmd != null)
                {
                    cmd.Dispose();
                }


            }
            return s;
        }
    }
}
