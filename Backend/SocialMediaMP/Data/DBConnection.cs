using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SocialMediaMP.Data
{
    class DBConnection
    {
        // how it connects and opening it
        // below is mums
       // public string ConnString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=SocialBackDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //public string ConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SocialMediaDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        // ^^ college 
        
        //public string ConnString = @"Data Source=(localdb)\\ProjectsV13;Initial Catalog=SocialBackDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         
        // public string ConnString = @"Data Source=socialmediaproject.database.windows.net;Initial Catalog=SocialMediaALevelProject;User ID=hjm8;Password=********;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         //below is azure
        public string ConnString = @"Server=tcp:socialmediaproject.database.windows.net,1433;Initial Catalog=SocialMediaALevelProject;Persist Security Info=False;User ID=hjm8;Password=James#4433001175;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
         

        //public string ConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SocialDBProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //^^ dads house

        public SqlConnection conn;

        public DBConnection()
        {
            conn = new SqlConnection(ConnString);
            conn.Open();
            //Console.WriteLine("Connection made");
        }

        public DataTable Select(string sql)
        {
            SqlDataAdapter DBAdapter = new SqlDataAdapter(sql, conn);

            DataTable Results = new DataTable();
            DBAdapter.Fill(Results);
            return Results;

        }

        public DataTable SelectParameters(string sql, List<SqlParameter> ps)
        {
            SqlDataAdapter DBAdapter = new SqlDataAdapter(sql, conn);
            foreach (SqlParameter p in ps)
            {
                DBAdapter.SelectCommand.Parameters.Add(p);
            }
            DataTable Results = new DataTable();
            DBAdapter.Fill(Results);
            return Results;

        }

        public void Insert(string sql, List<SqlParameter> reg)
        {
            SqlCommand DBcmd = new SqlCommand(sql, conn);
            foreach (SqlParameter p in reg)
            {
                DBcmd.Parameters.Add(p);
            }

            DBcmd.ExecuteNonQuery();

        }
        public void UpdateParams(string sql, List<SqlParameter> alt) // same as delete
        {
            SqlCommand DBcmd = new SqlCommand(sql, conn);
            foreach (SqlParameter p in alt)
            {
                DBcmd.Parameters.Add(p);
            }

            DBcmd.ExecuteNonQuery();
        }



        public void Close()
        {
            conn.Close();
        }
    }
}
