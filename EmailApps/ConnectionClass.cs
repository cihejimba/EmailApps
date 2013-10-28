using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;

namespace EmailApps
{
   public class ConnectionClass
    {
        SqlCeConnection Con = new SqlCeConnection("Data Source = 'EmailDB.sdf';Password='304970';");
        public string ExecuteQuery(string query)
        {
            string result = "";
            Con.Open();
            SqlCeCommand COMD = new SqlCeCommand(query, Con);
            try
            {
                result= COMD.ExecuteNonQuery().ToString();
            }
            catch (Exception EX)
            {
                result = EX.Message;
            }
            Con.Close();
            return result;
        }
        
        // this will query your database and return the result to your datatable
        public DataTable GetDataTable(string query)
        {
            DataTable dt = new DataTable();
            SqlCeCommand cmd = new SqlCeCommand(query, Con);
            Con.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
            da.Fill(dt);

            Con.Close();
          
            return dt;
        }
    }
}
