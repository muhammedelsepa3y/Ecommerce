using Ecommerce.Models;
using Ecommerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        EcommerceContext db = new EcommerceContext();
        [Authorize]
        public IActionResult AllProducts()
        {
            List<Product> products = db.Products.ToList();
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            foreach (Product product in products)
            {
                ProductViewModel productViewModel = new ProductViewModel();
                productViewModel.Name = product.Name;
                productViewModel.Price = product.Price;
                productViewModel.Image = product.ImageUrl;
                productViewModel.Quantity = product.Quantity;
                productViewModel.Id = product.Id;
                
                productViewModel.SellerName = db.Users.Find(product.SellerId).UserName;
                productViewModels.Add(productViewModel);
            }
            if (User.IsInRole("Customer"))
            {
                productViewModels= productViewModels.Where(p => p.Quantity > 0).ToList();
                return View(productViewModels);
            }
            else
            {
                productViewModels=productViewModels.Where(p => p.SellerName == User.Identity.Name).ToList();
                return View(productViewModels);
            }
            
        }
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IActionResult AddProduct(AddProductViewModel product)
        {
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public IActionResult AddProductAction(AddProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Quantity = productViewModel.Quantity;
                product.Description = productViewModel.Description;
                string fileName = Path.GetFileName(productViewModel.Image.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productViewModel.Image.CopyTo(fileStream);
                }
                product.ImageUrl = fileName;
                product.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("AllProducts");
            }
            else
            {
                return RedirectToAction("AddProduct", productViewModel);
            }
            
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            Product product = db.Products.Find(id);
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Name = product.Name;
            productViewModel.Price = product.Price;
            productViewModel.Image = product.ImageUrl;
            productViewModel.Quantity = product.Quantity;
            productViewModel.Id = product.Id;
            productViewModel.SellerName = db.Users.Find(product.SellerId).UserName;
            productViewModel.Description = product.Description;
            return View(productViewModel);

        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IActionResult Edit(int id)
        {
            Product product = db.Products.Where(p => p.Id == id).FirstOrDefault();
            EditProductViewModel editProductViewModel = new EditProductViewModel();
            editProductViewModel.Name = product.Name;
            editProductViewModel.Price = product.Price;
            editProductViewModel.Image = product.ImageUrl;
            editProductViewModel.Quantity = product.Quantity;
            editProductViewModel.Id = product.Id;
            editProductViewModel.Description = product.Description;
            return View(editProductViewModel);
            
        }
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public IActionResult Save (EditProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = db.Products.Where(p => p.Id == productViewModel.Id).FirstOrDefault(); 
                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Quantity = productViewModel.Quantity;
                product.Description = productViewModel.Description;
                if (productViewModel.ImageFile != null)
                {
                    string fileName = Path.GetFileName(productViewModel.ImageFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        productViewModel.ImageFile.CopyTo(fileStream);
                    }
                    product.ImageUrl = fileName;
                }
                db.Update(product);
                db.SaveChanges();
                return RedirectToAction("AllProducts");
                

            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return RedirectToAction("Edit", productViewModel);
            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("AllProducts");
            }
            catch (Exception ex)
            {
                return RedirectToAction("AllProducts");
            }


        }









    }
}
