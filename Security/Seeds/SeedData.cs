using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Data;
using Security.Enteties;
using Serilog;
using System.Security.Claims;

namespace Security.Seeds
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();

                context.Database.Migrate();
                EnsureSeedData(context);
                //EnsureUsers(scope);

                scope.ServiceProvider.GetService<AppDbContext>().Database.Migrate();
                EnsureRoles(scope);
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Log.Debug("IdentityResources being populated");
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("IdentityResources already populated");
            }

            if (!context.ApiScopes.Any())
            {
                Log.Debug("ApiScopes being populated");
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }

            if (!context.ApiResources.Any())
            {
                Log.Debug("ApiResources being populated");
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiResources already populated");
            }

            if (!context.IdentityProviders.Any())
            {
                Log.Debug("OIDC IdentityProviders being populated");
                context.IdentityProviders.Add(new OidcProvider
                {
                    Scheme = "demoidsrv",
                    DisplayName = "IdentityServer",
                    Authority = "https://demo.duendesoftware.com",
                    ClientId = "login",
                }.ToEntity());
                context.SaveChanges();
            }
            else
            {
                Log.Debug("OIDC IdentityProviders already populated");
            }
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var alice = userMgr.FindByNameAsync("alice").Result;
            
            if (alice == null)
            {
                alice = new AppUser
                {
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                };
                
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Role, "Administrator")
                }).Result;
                
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            
            if (bob == null)
            {
                bob = new AppUser
                {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim("location", "somewhere"),
                    new Claim(JwtClaimTypes.Role, "User")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }

        private static void EnsureRoles(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var admin = roleManager.FindByNameAsync(Authorization.Role.Administrator.ToString()).Result;

            if (admin == null)
            {
                admin = new IdentityRole
                {
                    Name = Authorization.Role.Administrator.ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                _ = roleManager.CreateAsync(admin).Result;

                Log.Debug($"Role {Authorization.Role.Administrator} created");

                scope.ServiceProvider.GetService<AppDbContext>().SaveChanges();
            }

            var oper = roleManager.FindByNameAsync(Authorization.Role.Operator.ToString()).Result;

            if (oper == null)
            {
                oper = new IdentityRole
                {
                    Name = Authorization.Role.Operator.ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                _ = roleManager.CreateAsync(oper).Result;

                Log.Debug($"Role {Authorization.Role.Operator} created");

                scope.ServiceProvider.GetService<AppDbContext>().SaveChanges();
            }

            var user = roleManager.FindByNameAsync(Authorization.Role.User.ToString()).Result;

            if (user == null)
            {
                user = new IdentityRole
                {
                    Name = Authorization.Role.User.ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                _ = roleManager.CreateAsync(user).Result;

                Log.Debug($"Role {Authorization.Role.User} created");

                scope.ServiceProvider.GetService<AppDbContext>().SaveChanges();
            }
        }
    }
}
