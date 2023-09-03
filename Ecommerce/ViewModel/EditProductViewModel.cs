namespace Ecommerce.ViewModel
{
    public class EditProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }

        public IFormFile? ImageFile { get; set; }


    }
}
