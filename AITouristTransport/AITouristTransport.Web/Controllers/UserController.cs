using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Services.Interfaces;

namespace AITouristTransport.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly PasswordHasher<User> _passwordHasher =
            new PasswordHasher<User>();

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ==============================
        // REGISTRATION
        // ==============================

        // Display registration page
        public IActionResult Register()
        {
            return View();
        }

        // Process registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser =
                    await _userService.GetUserByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(
                        "Email",
                        "An account with this email already exists.");

                    return View(user);
                }

                // Hash password before storing it
                user.Password =
                    _passwordHasher.HashPassword(user, user.Password);

                await _userService.AddUserAsync(user);

                return RedirectToAction("RegisterSuccess");
            }

            return View(user);
        }

        // Registration success page
        public IActionResult RegisterSuccess()
        {
            return View();
        }


        // ==============================
        // LOGIN
        // ==============================

        // Display login page
        public IActionResult Login()
        {
            return View();
        }

        // Process login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            // Find user by email
            var user =
                await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password.");

                return View();
            }

            // Verify entered password against stored password hash
            var result =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password.");

                return View();
            }

            // Check whether the account is active
            if (user.Status != "Active")
            {
                ModelState.AddModelError(
                    "",
                    "Your account is not active.");

                return View();
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.FirstName),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role)
            };

            // Create authenticated identity
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Authentication cookie settings
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            // Create authentication cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect to home page after successful login
            if (user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            if (user.Role == "VehicleProvider")
            {
                return RedirectToAction("Index", "VehicleProvider");
            }

            if (user.Role == "Driver")
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        // ==============================
        // LOGIN SUCCESS
        // ==============================

        // Temporary login success page
        public IActionResult LoginSuccess()
        {
            return View();
        }

        // Display Admin Login page
        public IActionResult AdminLogin()
        {
            return View();
        }

        // Process Admin Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid admin email or password.");
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid admin email or password.");
                return View();
            }

            if (user.Role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.FirstName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            await HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToAction("Dashboard", "Admin");
        }

        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }


}