using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Seller: IdentityUser
    {


        public string Name { get; set; }
       
        public virtual ICollection<Product> Products { get; set; }

    }
}
