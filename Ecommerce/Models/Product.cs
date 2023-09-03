using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        public string SellerId { get; set; }
        public Seller Seller { get; set; }
    }
}
