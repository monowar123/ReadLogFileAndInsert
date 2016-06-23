using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLogFileAndInsert
{
    public class DbHandler
    {
        string host = "localhost";
        string port = "5433";
        string dbName = "AO01";
        string userId = "postgres";
        string password = "1234";

        string connString = string.Empty;

        NpgsqlConnection con;

        public DbHandler()
        {
            connString = string.Format("SERVER={0}; Port={1}; Database={2}; User id={3}; Password={4}; encoding=unicode;",
                                        host, port, dbName, userId, password);
            con = new NpgsqlConnection(connString);
            con.Open();
        }


        public void InsertData(string insertSql)
        {
            int affectedRows = 0;

            using (NpgsqlCommand cmd = new NpgsqlCommand(insertSql, con))
            {
                affectedRows = cmd.ExecuteNonQuery();
            }
        }

        public void Close()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }
    }
}
