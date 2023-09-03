using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class CartItem
    {

        [Required(ErrorMessage = "Product ID is required.")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
        [ForeignKey("Customer")]

        public  string CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Order")]

        public int? OrderId { get; set; }
        public Order? Order { get; set; }

    }
}
