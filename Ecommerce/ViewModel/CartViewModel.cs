using Ecommerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModel
{
    public class CartViewModel
    {
       
        public int Id { get; set; }
        public int ProductId { get; set; }

        public String ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductStock { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImageUrl { get; set; }

        public Decimal TotalPrice { get; set; }
        

    }
}
