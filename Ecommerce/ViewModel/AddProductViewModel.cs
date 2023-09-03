using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModel
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        
        public string Name { get; set; }
        [Required]
        [ Range(1,1000)]
        public int Quantity { get; set; }
        [Required]
        [ Range(1,100000)]
        public decimal Price { get; set; }
        [Required]
        [ DataType(DataType.Upload)]

        public IFormFile Image { get; set; }
        [Required]
        [ DataType(DataType.MultilineText)]

        public string Description { get; set; }
    }
}
