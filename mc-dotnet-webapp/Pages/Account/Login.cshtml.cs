using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;

namespace mw_dotnet_webapp.Pages.Account
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                        .WithRedirectUri(returnUrl)
                        .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            System.Diagnostics.Debug.WriteLine(accessToken);

        }

        public async Task<string> returnAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            System.Diagnostics.Debug.WriteLine(accessToken);
            return accessToken;
        }
    }
}
