using Ecommerce.Models;
using Ecommerce.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        EcommerceContext dbContext = new EcommerceContext();


        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccouantViewModel registerAccouantViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser();
                user.Email = registerAccouantViewModel.Email;
                user.UserName = registerAccouantViewModel.UserName;
                user.PhoneNumber= registerAccouantViewModel.PhoneNumber;
                
                
                
                

                IdentityResult result=  await  userManager.CreateAsync(user, registerAccouantViewModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                    await signInManager.SignInAsync(user, isPersistent: false);
                  
      

                    return RedirectToAction("AllProducts", "Product");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            
            return View(registerAccouantViewModel);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = "~/Product/AllProducts")
        {
            ViewData["ReturnUrl"] = ReturnUrl;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl = "~/Product/AllProducts")
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByEmailAsync(email: loginViewModel.Email);

                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if(userManager.IsInRoleAsync(user, "Admin").Result)
                        {
                            return RedirectToAction("AllProducts", "Product");
                        }

                        return LocalRedirect(ReturnUrl);
                    }else
                    {
                        ModelState.AddModelError("", "Invalid Password");
                    }

                }
                ModelState.AddModelError("", "Invalid Email");
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterAccouantViewModel registerAccouantViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser();
                user.Email = registerAccouantViewModel.Email;
                user.UserName = registerAccouantViewModel.UserName;
                user.PhoneNumber = registerAccouantViewModel.PhoneNumber;

                IdentityResult result = await userManager.CreateAsync(user, registerAccouantViewModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Seller");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("AllProducts", "Product");


                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(registerAccouantViewModel);
        }


    }
}
