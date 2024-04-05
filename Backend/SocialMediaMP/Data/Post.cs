using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SocialMediaMP.Data
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostCaption { get; set; }
        public string PostText { get; set; }
        public int User_ID { get; set; }
        public string AuthorUN { get; set; }
        public string ImageURL { get; set; }
        // UID
        List<Reply> ReplyList = new List<Reply>(); // sort by datePosted or by likes?
                                                   // DateTime TimePosted;
        public Post()
        {
            int Post_Id;
            string PostCaption;
            string PostText;
            string User_ID;
            string AuthorUN;
        }
        public static void FilterText()
        {
            // Dictionary
        }
        public Post(DataRow PostRow)
        {
            PostId = int.Parse(PostRow["Post_Id"].ToString());
            PostCaption = PostRow["PostCaption"].ToString();
            PostText = PostRow["PostText"].ToString();
            //ReplyList = PostRow["ReplyList"].ToString();
            //TimePosted = DateTime.Parse(PostRow["TimePosted"].ToString()); // get the current date & time posted and insert it into the DB
        }

        public Post(string PostCaption, string PostText,int User_ID)
        { 
            DBConnection PostDB = new DBConnection();
            //string sql = "INSERT INTO dbo.Post (PostCaption,PostText) VALUES (@PostCaption,@PostText)";
            string sql = "INSERT INTO dbo.Post (PostCaption,PostText,User_ID) VALUES (@PostCaption,@PostText,@User_ID)";
            List<SqlParameter> paramss = new List<SqlParameter>();
            paramss.Add(new SqlParameter("PostCaption", PostCaption));
            paramss.Add(new SqlParameter("PostText", PostText));
            paramss.Add(new SqlParameter("User_ID", User_ID));
            PostDB.Insert(sql, paramss);
            PostDB.Close();

        }



        static public List<Post> GetPosts() // Gets everyones posts
        {
            DBConnection SocialDB = new DBConnection();
            string sql = "SELECT Post.Post_Id,Post.PostCaption, Post.PostText, UserAccount.User_ID, UserAccount.Username FROM dbo.Post INNER JOIN dbo.UserAccount ON Post.User_ID = UserAccount.User_ID";
            DataTable results = SocialDB.Select(sql);
            List<Post> Posts = new List<Post>();
            foreach (DataRow row in results.Rows)
            {
                int postID = (int)row["Post_Id"];
                string postCaption = (string)row["PostCaption"]; 
                string postText = (string)row["PostText"];
                int userID = (int)row["User_ID"];

                string username = (string)row["Username"];
                
                Post u = new Post(row);
                u.PostId= postID;
                u.PostText= postText;
                u.User_ID =userID;
                u.AuthorUN = username;
                Posts.Add(u);
                // process the post and user information as needed
            }
            
            //string sql = "SELECT * From dbo.Post";
            //DBConnection SocailDB = new DBConnection();
            //DataTable results = SocailDB.Select(sql);
            //foreach (DataRow r in results.Rows)
            //{
            //    Post u = new Post(r);
            //    Posts.Add(u);

            //}
            return Posts;
        }
        //static public List<Post>GetFriendsPosts()
        //{ // could do this SQL statement finding if Post_ID exists in UserConnections
        //    DBConnection SocialDB = new();
        //    string sql = "SELECT Post.Post_Id,Post.PostCaption, Post.PostText, UserAccount.User_ID, UserAccount.Username FROM dbo.Post INNER JOIN dbo.UserAccount ON Post.User_ID = UserAccount.User_ID";
            
        //    DataTable results = SocialDB.Select(sql);
        //    List<Post> Posts = new();
        //    foreach (DataRow row in results.Rows)
        //    {
                
        //        int postID = (int)row["Post_Id"];
        //        string postCaption = (string)row["PostCaption"];
        //        string postText = (string)row["PostText"];
        //        int userID = (int)row["User_ID"];
        //        string username = (string)row["Username"];                
        //        Post u = new Post(row);
        //        u.PostId = postID;
        //        u.PostText = postText;
        //        u.User_ID = userID;
        //        u.AuthorUN = username;
                
        //        Posts.Add(u);
        //        // process the post and user information as needed
        //    }


        //    return Posts;
        //}


    }
}
