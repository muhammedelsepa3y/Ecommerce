using Ecommerce.Models;
using Ecommerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace Ecommerce.Controllers
{
    public class OrderController : Controller
    {
        EcommerceContext db = new EcommerceContext();
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Checkout()
        {
            
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.CartItems = db.CartItems.Where(c => c.CustomerId == userId).ToList();
                db.Orders.Add(order);
                List <CartItem> cartItems = db.CartItems.Where(c => c.CustomerId == userId).ToList();
                foreach (CartItem item in cartItems)
                {
                    item.OrderId = order.Id;
                    Product product = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault();
                    product.Quantity -= item.Quantity;
                }
            
                db.SaveChanges();
                return RedirectToAction("AllProducts", "Product");
            }
            catch (System.Exception)
            {

                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        System.Console.WriteLine(error.ErrorMessage);
                    }
                }
                return RedirectToAction("AllProducts", "Product");
            }



        }


        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult GetOrders()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<OrderViewModel> orders = new List<OrderViewModel>();
            List<CartViewModel> cart = new List<CartViewModel>();
            List<CartItem> cartItems = db.CartItems.Where(c => c.CustomerId == userId && c.Order != null).ToList();
            foreach (CartItem item in cartItems)
            {
                CartViewModel cartItem = new CartViewModel();
                cartItem.ProductName = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Name;
                cartItem.ProductId = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Id;
                cartItem.ProductPrice = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Price;
                cartItem.ProductQuantity = item.Quantity;
                cartItem.ProductImageUrl = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().ImageUrl;
                cartItem.TotalPrice = item.Quantity * item.Product.Price;
                cart.Add(cartItem);
                Order  order = db.Orders.Where(o => o.Id == item.OrderId).FirstOrDefault();
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.Id = order.Id;
                orderViewModel.OrderDate = order.OrderDate;
                orderViewModel.CartItems = cart;
                orders.Add(orderViewModel);

            }
            return View(orders);
            

        }

        [HttpGet]
        [Authorize (Roles = "Customer")]
        public IActionResult Details(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        List<OrderViewModel> orders = new List<OrderViewModel>();
        List<CartViewModel> cart = new List<CartViewModel>();
        List<CartItem> cartItems = db.CartItems.Where(c => c.CustomerId == userId && c.Order != null).ToList();
            foreach (CartItem item in cartItems)
            {
                CartViewModel cartItem = new CartViewModel();
        cartItem.ProductName = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Name;
                cartItem.ProductId = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Id;
                cartItem.ProductPrice = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Price;
                cartItem.ProductQuantity = item.Quantity;
                cartItem.ProductImageUrl = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().ImageUrl;
                cartItem.TotalPrice = item.Quantity* item.Product.Price;
        cart.Add(cartItem);
                Order order = db.Orders.Where(o => o.Id == item.OrderId).FirstOrDefault();
        OrderViewModel orderViewModel = new OrderViewModel();
        orderViewModel.Id = order.Id;
                orderViewModel.OrderDate = order.OrderDate;
                orderViewModel.CartItems = cart;
                orders.Add(orderViewModel);

            }
            return View(orders);


}

        
    }
}
