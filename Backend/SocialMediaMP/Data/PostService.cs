using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SocialMediaMP.Data
{
    public class PostService
    {
      
        public async Task<Post> GetPostByPostID(int PostID) // retrieves the post of that PostID ->used for individual posts
        {
            Post p = new Post();
            if (PostID!=0)
            {
                int PostToFind = PostID;
                DBConnection socialDB = new();
                string sql = "Select Post.Post_Id,Post.PostCaption,Post.PostText,UserAccount.User_ID,UserAccount.Username FROM dbo.Post INNER JOIN dbo.UserAccount ON Post.User_ID = UserAccount.User_ID WHERE Post.Post_Id = @PostID";
                List<SqlParameter> sqlParameters= new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("PostID", PostID));
                DataTable results = socialDB.SelectParameters(sql,sqlParameters);
                if (results.Rows.Count==1)
                {
                    p.PostId = Convert.ToInt32(results.Rows[0]["Post_Id"]); // using [0] so it gets the first row -> kinda useless tbf
                    p.PostCaption = results.Rows[0]["PostCaption"].ToString();
                    p.PostText = results.Rows[0]["PostText"].ToString();
                    p.User_ID = Convert.ToInt32(results.Rows[0]["User_ID"]);
                    p.AuthorUN = results.Rows[0]["Username"].ToString();
                    return p;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<List<Reply>> GetRepliesByPostID(int PostID)
        {
            DBConnection socialDB = new();
            UserService uservice = new();
            
            List<Reply> ListOfReplies = new();
            if (PostID != 0)
            {
                string sql = "SELECT ReplyText,User_ID,Post_Id,Reply_Id FROM dbo.Reply WHERE Post_Id = @PostID";
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("PostID", PostID));
                DataTable results = socialDB.SelectParameters(sql, sqlParameters);
                if (results.Rows.Count > 0)
                {
                    foreach (DataRow row in results.Rows)
                    {
                        Reply reply = new Reply();
                        reply.ReplyText = row["ReplyText"].ToString();
                        reply.Reply_Id = Convert.ToInt32(row["Reply_Id"]);
                        reply.User_ID = Convert.ToInt32(row["User_ID"]);
                        reply.Post_Id = Convert.ToInt32(row["Post_Id"]);
                        reply.ReplierUN = uservice.GetUN(reply.User_ID);
                        ListOfReplies.Add(reply);
                    }
                }
            }
            socialDB.Close();
            return ListOfReplies; // returns null if PostID is invalid
        }
        public async Task<List<Like>> GetLikesByPostID(int PostID)
        {
            DBConnection socialDB = new();
            UserService uservice = new();
            
            List<Like> ListOfLikes = new();
            if (PostID != 0)
            {
                string sql = "SELECT * FROM Likes WHERE Post_Id = @PostID";
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("PostID", PostID));
                DataTable results = socialDB.SelectParameters(sql, sqlParameters);
                if (results.Rows.Count > 0)
                {
                    foreach (DataRow row in results.Rows)
                    {
                        Like _like = new Like();
                        _like.Like_ID = Convert.ToInt32(row["Like_ID"]);
                        _like.User_ID = Convert.ToInt32(row["User_ID"]);
                        _like.Post_Id = Convert.ToInt32(row["Post_Id"]);                        
                        ListOfLikes.Add(_like);
                    }
                }
            }
            socialDB.Close();
            return ListOfLikes; // returns null if PostID is invalid
        }
        public Task<List<Post>> GetPosts()
        {
            return Task.FromResult(Post.GetPosts());
        }
        //public Task<List<Post>>GetFriendsPosts()
        //{
        //    return Task.FromResult(Post.GetFriendsPosts)
        //}
        public async Task<List<Post>> GetPostsByUID(int UID)
        {
            List<Post> posts = new List<Post>();
            int PostsToFind = UID;
            if (PostsToFind != 0)
            {
                
                DBConnection SocialDB = new DBConnection();
                //string sql = "Select User_ID,Username,Bio,EmailAddress FROM UserAccount WHERE User_ID = @UserToFindID"; // prevents sql injection to get 
                //string sql = "Select Post_Id,PostCaption,PostText,User_ID FROM dbo.Post WHERE User_ID = @PostsToFind";
                string sql = "SELECT Post.Post_Id,Post.PostCaption, Post.PostText, UserAccount.User_ID, UserAccount.Username FROM dbo.Post INNER JOIN dbo.UserAccount ON Post.User_ID = UserAccount.User_ID WHERE Post.User_ID = @PostsToFind";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("PostsToFind", PostsToFind));
                DataTable results = SocialDB.SelectParameters(sql, parameters);
                if (results.Rows.Count > 0)
                {
                    

                    foreach (DataRow row in results.Rows)
                    {
                        Post p = new Post();
                        p.PostId = Convert.ToInt32(row["Post_Id"]);
                        p.PostCaption = row["PostCaption"].ToString();
                        p.PostText = row["PostText"].ToString();
                        p.User_ID = Convert.ToInt32(row["User_ID"]);
                        p.AuthorUN = row["Username"].ToString();
                        posts.Add(p);
                    }
                    return posts;
                }
                else
                {
                    return null;
                }
            }
            else { return null; }




        }
    }

}
