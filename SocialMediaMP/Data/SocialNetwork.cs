using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SocialMediaMP.Pages;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Blazored.SessionStorage;
using System.Linq;
using System.Data.Common;

namespace SocialMediaMP.Data
{
    public class SocialNetwork  // adjacency list
    {
        DBConnection socialnetworkdb = new DBConnection(); // connecting to the database
        
        public Dictionary<int, User> Users { get; set; } // Defined by User_ID and contains the User info
        public Dictionary<int, List<int>> Connections { get; set; } // the dictionary holds the Lists correlating to the userID (uses id as a key) and then the list shows the connections i think
        public SocialNetwork()
        {
            Users = new Dictionary<int, User>();
            Connections = new Dictionary<int, List<int>>();
             LoadUsers();
             LoadConnections();
        }
        // kinda needs to be static to call it in other programs?
        public List<int> GetConnectionsByID(int UID)
        {
            if (!Connections.ContainsKey(UID)) // checks if the UID is in connections if not then it returns an empty list
            {
                return new List<int>();
            }

            return Connections[UID]; // returns the List of connections of that UID
        }
        public virtual async Task OnInitializedAsync() // public virtual
        {
            await LoadUsers();
            await LoadConnections();
            //
        }
        [JSInvokable]
        public Task SaveConnections()
        {
            //split the lists
            DBConnection socialDB = new DBConnection();
            foreach (var keyValuePair in Connections)
            {
                var userID = keyValuePair.Key;
                var connectionIDs = string.Join(",", keyValuePair.Value);
                string Selectsql = ($"SELECT * FROM UserConnections WHERE User_ID = {userID}"); // gets the userconnections where UID is there to check how many there is
                List<SqlParameter> SelectParam = new List<SqlParameter>();
                DataTable results = socialDB.SelectParameters(Selectsql, SelectParam);
                if (results.Rows.Count > 0) // if there is a row that exists then it updates it rather than creating a new collumn -> no repeating columns with diff data but same UID
                {
                    string deleteSQL = $"DELETE FROM UserConnections WHERE User_ID = {userID}"; // deletes the current row so no duplicates                                                                                                // (Its okay as the db is loaded into connections upon initialization)                    
                    List<SqlParameter> DeleteParams = new List<SqlParameter>();
                    DeleteParams.Add(new SqlParameter("userID", userID));
                    socialDB.UpdateParams(deleteSQL, DeleteParams);
                    // not needed as allows duplicates
                    //string UpdateSQL = $"UPDATE UserConnections SET ConnectionUser_ID =  '{connectionIDs}' WHERE User_ID = {userID}"; // ConnectionUser_ID removed
                    //List<SqlParameter> UpdateParams = new List<SqlParameter>();
                    //UpdateParams.Add(new SqlParameter("userID", userID));
                    //UpdateParams.Add(new SqlParameter("connectionIDs", connectionIDs));
                    //socialDB.UpdateParams(UpdateSQL, UpdateParams);
                    string sql = "INSERT INTO UserConnections (User_ID, ConnectionUser_ID) VALUES (@userID, @connectionIDs)"; // inserts into the db
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("userID", userID));
                    parameters.Add(new SqlParameter("connectionIDs", connectionIDs));
                    socialDB.Insert(sql, parameters);
                }
                else
                {
                    string sql = "INSERT INTO UserConnections (User_ID, ConnectionUser_ID) VALUES (@userID, @connectionIDs)"; // inserts into the db if theres not alrdy a column
                    List<SqlParameter> parameters = new();
                    parameters.Add(new SqlParameter("userID", userID));
                    parameters.Add(new SqlParameter("connectionIDs", connectionIDs));
                    socialDB.Insert(sql, parameters);
                }



            }
            return Task.CompletedTask;

        }
        public List<int> SuggestFriends(int UserLoggedIn) // Called with UserLoggedIn return a List of users to suggest then random and suggest these to the User
        {
            //return a List of IDs to suggest -> then suggest each one one by one
            List<int> FriendsToSuggest = new();
            // search connections UID and look for their connections + make sure to remove your UID from it
            foreach (int ID in Connections[UserLoggedIn]) // gets each ID in Connections UserLoggedIn
            {
                // look in their connections
                List<int> friendConns= new();
                friendConns = GetConnectionsByID(ID);
                foreach (int friendConn in friendConns)
                {
                    // check if exists in Connections[UserLoggedIn]
                    if (Connections[UserLoggedIn].Contains(friendConn) ==false && friendConn!=UserLoggedIn && !FriendsToSuggest.Contains(friendConn))//if UserLoggedIn doesnt contain the value then suggest the friend checks if friendConn is LoggedInUser
                    { // also prevents duplicate suggestions as friends may all have mutual friends

                            FriendsToSuggest.Add(friendConn); // adds to List of Friendstosuggest
                      
                        
                    }
                    
                }

            }
            return FriendsToSuggest;
           

        }
        public List<int> SuggestedPosters(int UserLoggedIn) 
        {
           
            List<int> PostersToSuggest = new();
           
            foreach (int ID in Connections[UserLoggedIn]) // gets each connections ID of User logged ins 
            {
                // look in their connections
                List<int> friendConns = new();
                friendConns = GetConnectionsByID(ID);
                foreach (int friendConn in friendConns)
                {
                    
                    if (Connections[UserLoggedIn].Contains(friendConn) == false && friendConn != UserLoggedIn && !PostersToSuggest.Contains(friendConn))                    
                    // Checks if userlogged has existing connection & the connection isnt the userlogged in and checks for duplicates
                    {
                        PostersToSuggest.Add(friendConn); // List of userIDs whos post you should suggest
                    }

                }

            }
            return PostersToSuggest;


        }
        private Task LoadConnections()
        {
            foreach (int UID in Users.Keys) // checks each ID in List of users
            {
                string sql = "Select ConnectionUser_ID FROM UserConnections WHERE User_ID = @UID"; // gets all connection userIDs of UID
                List<SqlParameter> parameters = new List<SqlParameter>();
                List<int> connectionIDs = new List<int>();
                parameters.Add(new SqlParameter("UID", UID));
                DataTable results = socialnetworkdb.SelectParameters(sql, parameters);



                foreach (DataRow row in results.Rows) // checks each row with that UID (Should only be one row -> list of Connection ids)
                {
                    string connIDs = row["ConnectionUser_ID"].ToString(); // converts the row into a string
                    string[] ConnIDSArray = connIDs.Split(','); // splits up the string(comma seperated list) and makes an array
                    foreach (string id in ConnIDSArray) // searches for each id in the array
                    {

                        if (int.TryParse(id, out int connectionID)) // gets each id from array -> TryParse incase its not valid-> miss entry
                        {
                            if (!Connections[UID].Contains(connectionID)) // checks if it hasnt already been added (Duplicates)
                            {
                                Connections[UID].Add(connectionID); //adds it to the UID connections
                            }
                            
                        }
                    }




                }


                // didnt use this cause it would have many rows for eachUID
                //    foreach (int UID in Users.Keys)
                //{
                //    string sql = "Select ConnectionUser_ID FROM UserConnections WHERE User_ID = @UID";
                //    List<SqlParameter> parameters = new List<SqlParameter>();
                //    List<int> connectionIDs= new List<int>();
                //    parameters.Add(new SqlParameter("UID", UID));
                //    DataTable results =socialnetworkdb.SelectParameters(sql, parameters);
                //    foreach (DataRow row in results.Rows)
                //    {
                //        int connectionID = Convert.ToInt32(row["ConnectionUser_ID"]);
                //        connectionIDs.Add(connectionID);
                //    }
                //    Connections.Add(UID, connectionIDs);

                //}
                //foreach (var item in Connections) -> use this to output it
                //{
                //    Console.WriteLine("User ID: " + item.Key);
                //    Console.WriteLine("Connection IDs: " + string.Join(",", item.Value));
                //}

            }
        return Task.CompletedTask;
        }
        public Task LoadUsers() // needs to be fixed to allow changes to dictionary when changes to the userinfo are made 
        {
            // LoadsUsers when account created/if it doesnt exist to make sure the account can have connections
            List<SqlParameter> loadusersparas = new List<SqlParameter>();
            string sql = "SELECT User_ID,Username,EmailAddress FROM dbo.UserAccount"; // gets userid,username,email from DB to be used as a key -> userID to be used as a key + findable in list
            socialnetworkdb.SelectParameters(sql, loadusersparas); // calls dbconnection with the parameters and then its returned to it
            DataTable results = socialnetworkdb.Select(sql);
            foreach (DataRow DR in results.Rows) // searches through the datarows in the results table and adds it to variables
            {
                int User_ID = int.Parse(DR["User_ID"].ToString()); // done before so it can be compared
                if (!Users.ContainsKey(User_ID)) // checks if the dictionary already contains the userid if it does it moves onto the next to avoid duplicates
                {
                    User user = new User();
                    user.User_ID = User_ID;
                    user.UserName = DR["Username"].ToString();    
                    user.Email = DR["EmailAddress"].ToString();
                     // instantiating User so it can be passed into the dictionary no  values -> doesnt need it?
                    Users[User_ID] = user;
                    Connections[User_ID] = new List<int>();
                }


            }
            return Task.CompletedTask;


        }
        // to add a user do call the socialnetwork again when a new user created

        //public static void UpdateUserEM(int User_ID, string newEM) // pass the userid of the persons whos info is changed from user when its altered, get the UserID and then pass it the variables e.g username,email to be updated in the socialnetwork.
        //{

        //}
        //public static void UpdateUserUN(int User_ID, string newEM)
        //{

        //}

        public async Task AddConnection(int fromUser, int toUser) // from user=orig user , toUser=connection to 
        {

            if (fromUser != toUser && !Connections[fromUser].Contains(toUser)) // extra level of checking
            {
                Connections[fromUser].Add(toUser);
                Connections[toUser].Add(fromUser);
                
                await SaveConnections();
            }
            
            
            
        } // when Adding a friend call Add connection and get both UserIDs -> fromUser(person adding the other) toUser(person being added) and add them to the variables
        public async Task DeleteConnection(int fromUser,int toUser)  // fromUser is session var to user is pageID 
        {
            if (fromUser != toUser && Connections[fromUser].Contains(toUser))
            {
                Connections[fromUser].Remove(toUser);
                Connections[toUser].Remove(fromUser);
                
                await SaveConnections();
            }
                

            // only allow if existing connection -> only show button if logged in?
        }
        static void CreateNetwork()
        {
            //DBConnection SocailDB = new DBConnection();
            //string sql = "Select User_ID FROM dbo.UserAccount"; // brings in all data -> doesnt need the * as that brings it all in needs to be changed to just get whats needed, needs to get links
            //SqlCommand command = new SqlCommand(sql, connection);
            //SqlDataReader reader = command.ExecuteReader(); // allows reading of the table
            //List<User> users = new List<User>();
            //foreach (User user in users)
            //{
            //    string[] linkIDs = user.Links.Split(','); // create a links column in table to make this work
            //    foreach (string linkID in linkIds)
            //    {
            //        int uID = Convert.ToInt32(linkID); //creates the link id into a seperate userid }
            //        User linkedUser = users.Find(x => x.id == uID); // dno why this doesnt work

            //    }
            //}


        }
    }
}




























