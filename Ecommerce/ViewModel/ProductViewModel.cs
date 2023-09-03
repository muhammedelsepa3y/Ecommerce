using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string SellerName { get; set; }
        
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        
    }
}
