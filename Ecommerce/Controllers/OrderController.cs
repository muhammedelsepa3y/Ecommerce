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
                      
                List <CartItem> cartItems = db.CartItems.Where(c => c.CustomerId == userId&& c.OrderId==null).ToList();
                order.CartItems = cartItems;
                order.CustomerId = userId;

                db.Orders.Add(order);
                db.SaveChanges();
                foreach (CartItem item in cartItems)
                {
                    item.OrderId = order.Id;
                    db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Quantity -= item.Quantity;
                    db.CartItems.Update(item);
                    
                   
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
            List<OrderViewModel> ordersViewModel = new List<OrderViewModel>();
            List<Order> orders = db.Orders.Where(o => o.CustomerId == userId).ToList();
            foreach (Order order in orders)
            {
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.Id = order.Id;
                orderViewModel.OrderDate = order.OrderDate;
                List<CartItem> cartItems = db.CartItems.Where(c => c.CustomerId == userId && c.OrderId ==order.Id).ToList();
                decimal totalPrice = 0;
                foreach (CartItem item in cartItems)
                {
                    totalPrice += item.Quantity * db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Price;
                }
                orderViewModel.TotalPrice = totalPrice;
                ordersViewModel.Add(orderViewModel);
            }
     
            return View(ordersViewModel);
            

        }

        [HttpGet]
        [Authorize (Roles = "Customer")]
        public IActionResult Details(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        List<CartViewModel> carts = new List<CartViewModel>();
        List<CartItem> cartItems = db.CartItems.Where(c => c.CustomerId == userId && c.OrderId == id).ToList();
            decimal totalPrice = 0;
            foreach (CartItem item in cartItems)
            {
                CartViewModel cartItem = new CartViewModel();
                cartItem.ProductName = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Name;
                cartItem.ProductId = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Id;
                cartItem.ProductPrice = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Price;
                cartItem.ProductQuantity = item.Quantity;
                cartItem.ProductImageUrl = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().ImageUrl;
                cartItem.TotalPrice = item.Quantity* db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Price;
                
                totalPrice += cartItem.TotalPrice;


                carts.Add(cartItem);


            }
            ViewBag.TotalPrice = totalPrice;
            return View(carts);


}

        
    }
}
