using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System;

namespace SocialMediaMP.Data
{
    public class Profile:ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserService UService { get; set; }

        public int PageuserID { get; set; }

        public User user { get; set; }

        //protected override Task<User> OnInitializedAsync()
        //{
        //    Uri uri = new Uri(NavigationManager.Uri);
        //    string userIDString = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("User_ID");
        //    if (int.TryParse(userIDString, out int userIDValue))
        //    {
        //        PageuserID = userIDValue;
        //        user = await UService.GetUserByIDAsync(PageuserID);

        //    }

           
        //}
    }
}
