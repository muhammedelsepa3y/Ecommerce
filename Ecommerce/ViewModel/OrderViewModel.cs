namespace Ecommerce.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        
        public List<CartViewModel> CartItems { get; set; }
    
    }
}
