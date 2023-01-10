using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Security.Configurations
{
    public class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "alice",
                        Password = "alice",
                        Claims =
                        {
                          new Claim(JwtClaimTypes.Name, "Alice Smith"),
                          new Claim(JwtClaimTypes.GivenName, "Alice"),
                          new Claim(JwtClaimTypes.FamilyName, "Smith"),
                          new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                          new Claim(JwtClaimTypes.EmailVerified, "true", System.Security.Claims.ClaimValueTypes.Boolean),
                          new Claim(JwtClaimTypes.Role, "Administrator"),
                          new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                          new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                            IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "88421113",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                        {
                          new Claim(JwtClaimTypes.Name, "Bob Smith"),
                          new Claim(JwtClaimTypes.GivenName, "Bob"),
                          new Claim(JwtClaimTypes.FamilyName, "Smith"),
                          new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                          new Claim(JwtClaimTypes.EmailVerified, "true", System.Security.Claims.ClaimValueTypes.Boolean),
                          new Claim(JwtClaimTypes.Role, "User"),
                          new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                          new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                            IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    }
                };
            }
        }

        public static IEnumerable<IdentityResource> IdentityResources => new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("role", new[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes => new[]
            {
                new ApiScope("flightapi.read"),
                new ApiScope("flightapi.write")
            };

        public static IEnumerable<ApiResource> ApiResources => new[]
            {
                new ApiResource("flightapi")
                {
                    Scopes = new List<string> { "flightapi.read", "flightapi.write"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string> { "role" }
                },
                new ApiResource(
                    LocalApi.ScopeName,
                    "Local Api",
                    new [] { JwtClaimTypes.Role }
                )
            };

        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

                AllowedScopes = { "flightapi.read", "flightapi.write" }
            },

            new Client
            {
                ClientId = "interactive",
                RequirePkce = true,
                Enabled = true,

                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
                RedirectUris = {"https://localhost:8080/signin-oidc"},

                AllowOfflineAccess = true,
                AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    "flightapi.read",
                    "flightapi.write",
                    "role"
                },
            },

            new Client
            {
                ClientId = "postman",
                RequirePkce = true,
                Enabled = true,

                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = { new Secret("Johnbur1tto!".Sha256()) },
                RedirectUris = { "https://localhost:8080/signin-oidc" },

                AllowedScopes = { 
                    StandardScopes.OpenId,
                    StandardScopes.Profile, 
                    "flightapi.read",
                    "flightapi.write"
                }
            }
        };
    }
}
