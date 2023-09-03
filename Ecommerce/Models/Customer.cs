using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Customer : IdentityUser
    {


        public string Name { get; set; }

        
        public virtual ICollection<Order> Orders { get; set; }
        

    }
}
