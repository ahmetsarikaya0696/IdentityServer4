using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return
            [
                new ApiResource("api1")
                {
                    Scopes = { "api1.read", "api1.write", "api1.update" },
                    ApiSecrets = [new Secret("secretapi1".Sha256())]
                },
                new ApiResource("api2")
                {
                    Scopes = { "api2.read", "api2.write", "api2.update" },
                    ApiSecrets = [new Secret("secretapi2".Sha256())]
                },
            ];
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return
            [
                new ApiScope("api1.read","api1 için okuma izni"),
                new ApiScope("api1.write","api1 için yazma izni"),
                new ApiScope("api1.update","api1 için güncelleme izni"),

                new ApiScope("api2.read","api2 için okuma izni"),
                new ApiScope("api2.write","api2 için yazma izni"),
                new ApiScope("api2.update","api2 için güncelleme izni"),
            ];
        }

        public static IEnumerable<Client> GetClients()
        {
            return
            [
                new Client()
                {
                    ClientName = "Client1 Uygulaması",
                    ClientId = "Client1",
                    ClientSecrets =
                    [
                        new Secret("secret".Sha256()),
                    ],
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =["api1.read"]
                },
                new Client()
                {
                    ClientName = "Client2 Uygulaması",
                    ClientId = "Client2",
                    ClientSecrets =
                    [
                        new Secret("secret".Sha256()),
                    ],
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =["api1.read", "api1.update", "api2.write", "api2.update"]
                },
                new Client()
                {
                    ClientName = "Client1 Mvc Uygulaması",
                    ClientId = "Client1-Mvc",
                    RequirePkce= false,
                    ClientSecrets =
                    [
                        new Secret("secret".Sha256()),
                    ],
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = ["https://localhost:7259/signin-oidc"],
                    PostLogoutRedirectUris = ["https://localhost:7259/signout-callback-oidc"],
                    AllowedScopes = [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "CountryAndCity",
                        "Roles",
                        IdentityServerConstants.StandardScopes.Email,
                    ],
                    AccessTokenLifetime = 2 * 60 * 60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 60 * 24 * 60 * 60,
                    AllowOfflineAccess = true, // refresh token yayınlanmasını sağlar
                    RequireConsent = true,
                },
                new Client()
                {
                    ClientName = "Client2 Mvc Uygulaması",
                    ClientId = "Client2-Mvc",
                    RequirePkce= false,
                    ClientSecrets =
                    [
                        new Secret("secret".Sha256()),
                    ],
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = ["https://localhost:7245/signin-oidc"],
                    PostLogoutRedirectUris = ["https://localhost:7245/signout-callback-oidc"],
                    AllowedScopes = [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "CountryAndCity",
                        "Roles"
                    ],
                    AccessTokenLifetime = 2 * 60 * 60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 60 * 24 * 60 * 60,
                    AllowOfflineAccess = true, // refresh token yayınlanmasını sağlar
                    RequireConsent = true,
                },
                  new Client()
                {
                    ClientName = "Client1 ResourceOwner Mvc Uygulaması",
                    ClientId = "Client1-ResourceOwner-Mvc",
                    ClientSecrets =
                    [
                        new Secret("secret".Sha256()),
                    ],
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "CountryAndCity",
                        "Roles",
                        IdentityServerConstants.StandardScopes.Email,
                    ],
                    AccessTokenLifetime = 2 * 60 * 60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 60 * 24 * 60 * 60,
                    AllowOfflineAccess = true, // refresh token yayınlanmasını sağlar
                },
            ];
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return
            [
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource()
                {
                    Name = "CountryAndCity",
                    DisplayName="Country and City",
                    Description="User's country and city information.",
                    UserClaims = ["country", "city" ]
                },
                new IdentityResource()
                {
                    Name = "Roles",
                    DisplayName="Roles",
                    Description="User's roles.",
                    UserClaims = ["role"]
                },
            ];
        }

        public static List<TestUser> GetTestUsers()
        {
            return
            [
                new TestUser()
                {
                    SubjectId = "1",
                    Username="ahmetsarikaya",
                    Password="password",
                    Claims =
                    [
                        new Claim("given_name", "Ahmet"),
                        new Claim("family_name", "Sarıkaya"),
                        new Claim("country", "Turkey"),
                        new Claim("city", "Ankara"),
                        new Claim("role", "admin"),
                    ]

                },
                new TestUser()
                {
                    SubjectId = "2",
                    Username="erdalsarikaya",
                    Password="password",
                    Claims =
                    [
                        new Claim("given_name", "Erdal"),
                        new Claim("family_name", "Sarıkaya"),
                        new Claim("country", "Turkey"),
                        new Claim("city", "Istanbul"),
                        new Claim("role", "customer"),
                    ],
                }
            ];
        }
    }
}
