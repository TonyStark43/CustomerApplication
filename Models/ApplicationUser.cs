using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CustomerApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        [RegularExpression(@"^[a-zA-Z0-9_]{10}$",
           ErrorMessage = "DomainName must be 10 characters' alphanumeric/underscore.")]
        public string DomainName { get; set; }
    }

}
