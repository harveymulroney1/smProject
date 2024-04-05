using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using SocialMediaMP.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialMediaMP

{
    public class SessionAuthenticationProvider : AuthenticationStateProvider
    {
        //[Inject]
        //Blazored.SessionStorage.ISessionStorageService session { get; set; }
        private readonly ISessionStorageService session;

        public SessionAuthenticationProvider(ISessionStorageService _session)
        {
            session = _session;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var isAuthenticated = await session.GetItemAsync<User>("UserLoggedIn");
                if (isAuthenticated != null)
                {

                    //if (isAuthenticated)
                    //{
                    User u = await session.GetItemAsync<User>("UserLoggedIn");

                    var UserID = u.User_ID.ToString();
                    var userName = u.UserName;
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, UserID),
                    new Claim(ClaimTypes.Name, userName),
                    };
                    var identity = new ClaimsIdentity(claims, "session");
                    var user = new ClaimsPrincipal(identity);
                    return await Task.FromResult(new AuthenticationState(user));


                    //}

                }
                //var isAuthenticated = await session.ContainKeyAsync("UserLoggedIn");


                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
            catch (System.Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // returns an empty / unauthed user
            }

    //var isAuthenticated = await session.ContainKeyAsync("UserLoggedIn");
    // basically logs the user out -> not logged in anonymous user


    //var state = new AuthenticationState(user);
    //NotifyAuthenticationStateChanged(Task.FromResult(user)); // test
    //NotifyAuthenticationStateChanged(Task.FromResult(state));


}
        public async Task RefreshAuthState()
        {
            var _authState = await GetAuthenticationStateAsync();
            if (_authState.User.Identity.IsAuthenticated)
            {
                // Create a new claims identity based on the current identity
                var identity = new ClaimsIdentity(_authState.User.Identity);

                // Create a new claims principal based on the new identity
                var principal = new ClaimsPrincipal(identity);

                // Create a new authentication state based on the new principal
                var newAuthState = new AuthenticationState(principal);

                // Notify the application that the authentication state has changed
                NotifyAuthenticationStateChanged(Task.FromResult(newAuthState));
            }
        }
        
        public async Task ClearAuthenticationStateAsync()
        {
            // Clear the authentication data from session storage
            
             // removes this item
            
            
            // Notify the authentication state has changed
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
