using Ecommerce.Models;
using Ecommerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        EcommerceContext db = new EcommerceContext();
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult AddToCart(int id)
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                CartItem cartItem = db.CartItems.Where(c => c.CustomerId == userId && c.ProductId == id && c.OrderId==null).FirstOrDefault();
                if (cartItem != null )
                {
                   
                    if (cartItem.Quantity < db.Products.Where(p => p.Id == id).FirstOrDefault().Quantity)
                    {
                        cartItem.Quantity++;
                    }
                   
                }
                else
                {
                    cartItem = new CartItem();
                    cartItem.CustomerId = userId;
                    cartItem.ProductId = id;
                    cartItem.Quantity = 1;
                    db.CartItems.Add(cartItem);
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
        public IActionResult GetCart()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<CartViewModel> cart = new List<CartViewModel>();
            List <CartItem> cartItems =  db.CartItems.Where(c => c.CustomerId == userId&& c.OrderId==null).ToList();
            foreach (CartItem item in cartItems)
            {
                CartViewModel cartItem = new CartViewModel();
                cartItem.ProductName =   db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Name;
                cartItem.ProductId = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Id;
                cartItem.ProductPrice = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Price;
                if (item.Quantity > db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Quantity)
                {
                    item.Quantity = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Quantity;
                    db.SaveChanges();
                }
                cartItem.ProductQuantity = item.Quantity;
                cartItem.ProductImageUrl = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().ImageUrl;
                cartItem.TotalPrice = item.Quantity * item.Product.Price;
                cart.Add(cartItem);
            }
            return View(cart);

        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Delete(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartItem cartItem = db.CartItems.Where(c => c.CustomerId == userId &&  c.ProductId == id&& c.OrderId==null).FirstOrDefault();
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("GetCart");

        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Minus(int id)
        {

            
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                CartItem cartItem = db.CartItems.Where(c => c.CustomerId == userId && c.ProductId == id && c.OrderId == null).FirstOrDefault();
                cartItem.Quantity--;
            if (cartItem.Quantity == 0)
            { 
                db.CartItems.Remove(cartItem);
                  
            }
                db.SaveChanges();
            return RedirectToAction("GetCart", "Cart");

        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Plus(int id)
        {


            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartItem cartItem = db.CartItems.Where(c => c.CustomerId == userId && c.ProductId == id && c.OrderId == null).FirstOrDefault();
            cartItem.Quantity++;
            if (cartItem.Quantity>db.Products.Where(p=>p.Id==id).FirstOrDefault().Quantity)
            {
                cartItem.Quantity= db.Products.Where(p => p.Id == id).FirstOrDefault().Quantity;
            }   
            db.SaveChanges();
            return RedirectToAction("GetCart","Cart");

        }
    }
}
