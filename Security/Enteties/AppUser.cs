using Microsoft.AspNetCore.Identity;

namespace Security.Enteties
{
    public class AppUser : IdentityUser
    {
        public AppUser() 
        { 
        
        }

        public AppUser(string userName) : base(userName)
        {

        }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set;}
    }
}
