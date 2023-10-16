using AuthenticationCookie.Models;
using AuthenticationCookie.Services;
using CookieAuthentication.Context;
using CookieAuthentication.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationCookie.Controllers
{
    public class AuthController : Controller
    {
        private readonly BaseDbContext _context;
        private readonly IAuthService _authService;
        public AuthController(BaseDbContext context,IAuthService authService)
        {
            _context = context;
            _authService= authService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
               User? user= _context.Users.SingleOrDefault(_=>_.Username.ToLower()== loginViewModel.UserName);
                if (user !=null)
                {
                    bool verifiedPassword = _authService.VerifyPasswordHash(loginViewModel.Password, user.PasswordHash, user.PasswordSalt);
                    if (verifiedPassword)
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                        claims.Add(new Claim("UserName", user.Username));
                        claims.Add(new Claim(ClaimTypes.Role, user.Role));

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı yada şifre hatalı");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Kullanıcı adı yada şifre hatalı");
                }
            }
            return View(loginViewModel);
        }
        public IActionResult Register() { return View(); }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel) 
        {
            if(ModelState.IsValid)
            {
                if (_context.Users.Any(_=>_.Username.ToLower()== registerViewModel.UserName.ToLower()))
                {
                    ModelState.AddModelError(nameof(registerViewModel.UserName), "Farklı bir kullanıcı adı giriniz.");
                    View(registerViewModel);
                }
                _authService.CreatePasswordHash(registerViewModel.Password,out byte[] passwordHash,out byte[] passwordSalt);

                User user = new()
                {
                    Username = registerViewModel.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                _context.Users.Add(user);
              int count= _context.SaveChanges();
                if (count== 0 ) {
                    ModelState.AddModelError("", "Kullanıcı eklenemedi.");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }

            }
            return View(registerViewModel);
        }

        public IActionResult Logout() {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View(nameof(Login));
        }
    }
}
