using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaMP.Data
{
    public class Reply
    {
        public int User_ID { get; set; }
        public string ReplyText { get; set; }
        public int Reply_Id { get; set; }
        public int Post_Id { get; set; }
        public string ReplierUN { get; set; }
        // check user is logged in by checking the sessionVariable -> see if null if so redirect to login page
        // get user currently logged in(their id) as replyID correlate to a postID which relates to a userID 

    }
}
