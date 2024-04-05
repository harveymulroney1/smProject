using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Components;
using Blazored.SessionStorage;

namespace SocialMediaMP.Data
{
    public class UserService

    {
        //[Inject]
        //private ISessionStorageService session { get; set; }
        private readonly ISessionStorageService _session;

        public UserService(ISessionStorageService session)
        {
            _session = session;
        }
        public UserService() { }
        public async Task<int> GetUIDSessionAsync()
        {
            // need to call this to get the value
            int userLoggedinID = 0;
            User userLoggedin = await _session.GetItemAsync<User>("UserLoggedIn");
            if (userLoggedin != null)
            {
                userLoggedinID = userLoggedin.User_ID;
            }


            return userLoggedinID;
        }
        public int GetUIDByUN(string UN)
        {
            DBConnection SocialDB = new DBConnection();
            string sql = "SELECT User_ID FROM dbo.UserAccount WHERE Username = @UN";
            List<SqlParameter> UNparams = new List<SqlParameter>();
            UNparams.Add(new SqlParameter("UN", UN));
            try
            {
                DataTable results = SocialDB.SelectParameters(sql, UNparams);
                if (results!=null)
                {
                    int UID = Convert.ToInt32(results.Rows[0]["User_ID"]);
                    return UID;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        }
        public string GetUN(int UID)
        {
            DBConnection SocialDB = new DBConnection();
            string sql = "SELECT Username FROM UserAccount WHERE User_ID =@UID";
            List<SqlParameter> UNparams = new List<SqlParameter>();
            UNparams.Add(new SqlParameter("UID", UID));
            DataTable results = SocialDB.SelectParameters(sql, UNparams);
            string UN = Convert.ToString(results.Rows[0]["Username"]);
            return UN;
        }
        public async Task<User> GetUserByID(int UID)
        {

            int UserToFindID = UID;
            if (UserToFindID!=0)
            {
                User u = new User();
                DBConnection SocialDB = new DBConnection();
                string sql = "Select User_ID,Username,Bio,EmailAddress FROM UserAccount WHERE User_ID = @UserToFindID"; // prevents sql injection to get 
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("UserToFindID", UserToFindID));
                DataTable results = SocialDB.SelectParameters(sql, parameters);
                if (results.Rows.Count > 0)
                {
                    DataRow row = results.Rows[0];
                    u.User_ID = (Convert.ToInt32(row["User_ID"]));
                    u.UserName = row["Username"].ToString();
                    u.Email = row["EmailAddress"].ToString();
                    u.Bio = row["Bio"].ToString();
                    return u;
                }
                else
                {
                    return null;
                }
            }
            else { return null; }
            
            


        }



        //    public async Task<User> GetProfileInfoAsync()
        //    {
        //        User user = new User()
        //        Uri uri = new Uri(_navigationManager.Uri);
        //        string userIDString = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("User_ID");
        //        if (int.TryParse(userIDString, out int userIDValue))
        //        {
        //            DBConnection socialdb = new DBConnection();
        //            List<SqlParameter> parameters = new List<SqlParameter>();
        //            string sql = "Select Username, Bio, EmailAddress FROM UserAccount WHERE User_ID = @User_ID";
        //            parameters.Add(new SqlParameter("User_ID", userIDValue));
        //            DataTable results = socialdb.SelectParameters(sql, parameters);
        //            if (results.Rows.Count > 0)
        //            {
        //                DataRow row = results.Rows[0];
        //                Username = row["Username"].ToString();
        //                UserProfile = row["EmailAddress"].ToString();
        //                Bio = row["Bio"].ToString();

        //            }
        //        }
        //    }
        //}

        //void GetProfileInfo()
        //{
        //    Uri uri = new Uri(_navigationManager.Uri);
        //    string userIDString = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("User_ID");
        //    if (int.TryParse(userIDString, out int userIDValue))
        //    {
        //        PageuserID = userIDValue;
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        string sql = "Select Username,Bio,EmailAddress FROM UserAccount WHERE User_ID = @PageuserID";
        //        parameters.Add(new SqlParameter("User_ID", PageuserID));
        //        DBConnection socialDB = new DBConnection();
        //        DataTable results = socialDB.SelectParameters(sql, parameters);
        //        Bio = Convert.ToString(results.Rows[0]["Bio"]);
        //        if (results.Rows.Count > 0)
        //        {
        //            DataRow row = results.Rows[0];
        //            string username = row["Username"].ToString();
        //            string email = row["EmailAddress"].ToString();
        //            string bio = row["Bio"].ToString();
        //            //string profilePic = row["ProfilePic"].ToString(); only need if implemented
        //        }





        //    }

        //public async Task LoginUser()
        //{ 

        //}
        //public async Task LogoutUser()
        //{

        //}
        /// <summary>
        /// Its for a admin role of retrieving all the Users - needs to be configured so pass arent shown? or not cause hashed
        /// </summary>
        /// <returns></returns>
        //public Task<List<User>> GetUsersAsync()
        //{
        //    List<User> ulist = new List<User>();
        //    string sql = "Select * from users";
        //    DBConnection dbc = new DBConnection();
        //    DataTable results = dbc.Select(sql);
        //    foreach (DataRow r in results.Rows)
        //    {
        //        User u = new User(r);
        //        ulist.Add(u);
        //    }
        //    return ulist;
        //}

    }
}


