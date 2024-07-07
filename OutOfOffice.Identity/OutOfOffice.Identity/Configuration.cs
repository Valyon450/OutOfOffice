using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace OutOfOffice.Identity
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
            new ApiScope("OutOfOfficeWebAPI", "Web API")
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("OutOfOfficeWebAPI", "Web API", new []
                    { JwtClaimTypes.Name})
                {
                    Scopes = {"OutOfOfficeWebAPI"}
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "outofoffice-web-app",
                    ClientName = "OutOfOffice Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris =
                    {
                        "http://localhost:3000/signin-oidc"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:3000"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:3000/signout-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "OutOfOfficeWebAPI"
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
