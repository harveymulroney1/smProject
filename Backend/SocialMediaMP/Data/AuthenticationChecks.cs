using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SocialMediaMP.Data
{
    class AuthenticationChecks
    {
        public static bool UserAlterCheck(string EM, string PW) // checks if the password and the email exist in DB before moving onto next part
        {
            string sql = "SELECT * FROM dbo.UserAccount WHERE EmailAddress = @EM AND UPassword = @PW"; // checks if EM and PW exist
            List<SqlParameter> paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("@EM", EM));
            paramlist.Add(new SqlParameter("@PW", PW));
            // checking if the user exists
            DBConnection DBS = new DBConnection();
            DataTable u = DBS.SelectParameters(sql, paramlist);
            DBS.Close();
            if (u.Rows.Count == 1)
            {
                
                return true;
            }
            else
            {
                return false;
            }

        }
        // check for existing username when creating
        public static bool Login(string UN, string PW)
        {

            string sql = "SELECT * FROM dbo.UserAccount WHERE Username=@UN AND UPassword=@PW"; // checks for occurences of given UN and PW
            List<SqlParameter> paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("@UN", UN));
            paramlist.Add(new SqlParameter("@PW", PW));
            // searching for the Users with UN and PW
            DBConnection DBS = new DBConnection();
            DataTable u = DBS.SelectParameters(sql, paramlist);
            DBS.Close();
            if (u.Rows.Count == 1)
            {
                //global.CurrentUser = new User(u.Rows[0]); // adds it to class call it when to find whos logged in do the same with creating an account

                return true;
            }
            return false;
        }
        public static bool BackupCodeCheck(string BackupCode)
        {
            string sql = "SELECT * FROM dbo.UserAccount WHERE Backupcode = @BackupCode"; 
            List<SqlParameter> paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("BackupCode", BackupCode));

            // checking if a user has that BackupCode already
            DBConnection DBS = new DBConnection();
            DataTable u = DBS.SelectParameters(sql, paramlist);
            DBS.Close();
            if (u.Rows.Count == 1)
            {
                return true; // existing row -> BC exists
            }
            else
            {
                return false;
            }
        }
        public static bool ForgotPW(string BackupCode) // checks if the backup code exists in DB
        {
            string sql = "SELECT * FROM dbo.UserAccount WHERE Backupcode = @BackupCode"; // checks for occurences of given UN and PW
            List<SqlParameter> paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("@BackupCode", BackupCode));

            // searching for the User with that BackupCode
            DBConnection DBS = new DBConnection();
            DataTable u = DBS.SelectParameters(sql, paramlist);
            DBS.Close();
            if (u.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool CheckUN(string UN)
        {
            string sql = "SELECT * FROM dbo.UserAccount WHERE Username=@UN";
            List<SqlParameter> paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("@UN", UN));
            // checking if existing user w UN requested
            DBConnection DBS = new DBConnection();
            DataTable u = DBS.SelectParameters(sql, paramlist);


            if (u.Rows.Count == 1)
            { return true; } // returns true if the UN exists

            return false;

        }
        public static bool CheckEM(string EM)
        {
            string sql = "SELECT * FROM dbo.UserAccount WHERE EmailAddress=@EM";
            List<SqlParameter> paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("@EM", EM));
            // checking if existing user w UN requested
            DBConnection DBS = new DBConnection();
            DataTable u = DBS.SelectParameters(sql, paramlist);
            //DBS.Close();

            if (u.Rows.Count == 1)
            { return true; } // returns true if the EM exists
            return false;
        }
    }
}

// work through whole list of it to check if it contains