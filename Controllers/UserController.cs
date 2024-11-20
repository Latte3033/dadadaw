using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStock.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebStock.Data;

namespace WebStock.Controllers
{


    public class UserController : Controller
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ILogger<UserController> logger)
        {
            _passwordHasher = passwordHasher;
            _logger = logger;
            _context = context;
        }

        private bool IsUserLoggedIn()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to access this page.";
                return false;
            }
            return true;
        }

        // GET: User/ComposeEmail
        public IActionResult ComposeEmail()
        {
            var emailDraft = new EmailDraft(); // Ensure this is initialized
            return View(emailDraft); // Pass the empty model
        }


        // POST: User/ComposeEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ComposeEmail(EmailDraft email)
        {
            if (ModelState.IsValid)
            {
                // Add the email to the database
                _context.EmailDraft.Add(email);
                _context.SaveChanges();

                // Redirect or show success message
                TempData["SuccessMessage"] = "Email saved successfully!";
                return RedirectToAction("EmailList");  // Or wherever you want to redirect
            }

            // If the model is not valid, return the same view
            return View(email);
        }

        public ActionResult EmailList()
        {
            var emails = _context.EmailDraft.OrderByDescending(e => e.SentAt).ToList();
            return View(emails);
        }



        // GET: User/Index
        public ActionResult Index()
{
    _logger.LogInformation("Accessed Index page.");

    // Check if user is logged in
    if (!IsUserLoggedIn())
    {
        TempData["ErrorMessage"] = "You must be logged in to access this page.";
        return RedirectToAction("Login");
    }

    // Retrieve the logged-in user's ID from the session
    var userId = HttpContext.Session.GetString("UserId");

    if (userId == null)
    {
        TempData["ErrorMessage"] = "User session has expired. Please log in again.";
        return RedirectToAction("Login");
    }

    // Retrieve the logged-in user from the database
    var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);

    if (user == null)
    {
        TempData["ErrorMessage"] = "User not found.";
        return RedirectToAction("Login");
    }

    // Return the view with the logged-in user's details
    return View(user);
}


        // GET: User/Register
        public ActionResult Register()
        {
            _logger.LogInformation("Accessed Register page.");
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email || u.UserName == user.UserName))
                {
                    _logger.LogWarning($"Registration failed: Email '{user.Email}' or Username '{user.UserName}' already exists.");
                    ModelState.AddModelError(string.Empty, "Email or Username already exists.");
                    return View(user);
                }

                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                _logger.LogInformation($"User '{user.UserName}' registered successfully.");
                TempData["SuccessMessage"] = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }

            _logger.LogWarning("Registration failed: Invalid model state.");
            return View(user);
        }



        // GET: User/Login
        public IActionResult Login()
        {
            _logger.LogInformation("Accessed Login page.");
            return View(new LoginViewModel());
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Identifier || u.UserName == model.Identifier);

                if (user != null)
                {
                    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId", user.Id.ToString());
                        HttpContext.Session.SetString("UserName", user.UserName);

                        _logger.LogInformation($"User '{user.UserName}' logged in successfully.");
                        TempData["SuccessMessage"] = "Login successful!";
                        return RedirectToAction("Index", "User");
                    }
                }

                _logger.LogWarning("Login failed: Invalid credentials.");
                ModelState.AddModelError(string.Empty, "Invalid email/username or password.");
            }

            return View(model);
        }

        // GET: User/Logout
        public IActionResult Logout()
        {
            _logger.LogInformation("User logged out.");
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}
