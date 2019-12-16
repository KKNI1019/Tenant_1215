using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace tenantApp.Line
{
    public class LineOAuth2 : OAuth2Base
    {
        private static readonly Lazy<LineOAuth2> lazy = new Lazy<LineOAuth2>(() => new LineOAuth2());

        public static LineOAuth2 Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private LineOAuth2()
        {
            Initialize();
        }

        void Initialize()
        {
            ProviderName = "Line";
            Description = "Line Login Provider";
            Provider = SNSProvider.Line;
            ClientId = "1619610726";
            ClientSecret = "db328e1363eb4d3179951cba8cd8c31b";
            Scope = "openid profile email";
            AuthorizationUri = new Uri("https://access.line.me/oauth2/v2.1/authorize");
            RequestTokenUri = new Uri("https://api.line.me/oauth2/v2.1/token");
            RedirectUri = new Uri("https://www.facebook.com/connect/login_success.html");
            UserInfoUri = new Uri("https://api.line.me/v2/profile");
        }
        
        public override async Task<User> GetUserInfoAsync(Account account)
        {
            User user = null;
            string token = account.Properties["access_token"];
            string refreshToke = account.Properties["refresh_token"];
            int expriesIn;
            int.TryParse(account.Properties["expires_in"], out expriesIn);

            Dictionary<string, string> dictionary = new Dictionary<string, string> { { "Authorization", token+", name, email"} };
            var request = new OAuth2Request("POST", UserInfoUri, dictionary, account);
            var response = await request.GetResponseAsync();
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string userJson = await response.GetResponseTextAsync();
                var lineUser = JsonConvert.DeserializeObject<LineUser>(userJson);
                
                user = new User
                {
                    Id = lineUser.Id,
                    Token = token,
                    RefreshToken = refreshToke,
                    email = lineUser.email,
                    Name = lineUser.Name,
                    ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(expriesIn)),
                    PictureUrl = lineUser.ProfileImage,
                    Provider = SNSProvider.Line,
                    LoggedInWithSNSAccount = true,
                };
            }

            return user;
        }

        public override Task<(bool IsRefresh, User User)> RefreshTokenAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
