using ASC.Model;
using ASC.Web.Controllers;
using ASC.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: List Customers (Admin only)
        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            var customers = await _userManager.GetUsersInRoleAsync(Constants.Roles.User);
            return View(customers.ToList());
        }

        // GET: List Service Engineers (Admin only)
        [HttpGet]
        public async Task<IActionResult> ServiceEngineers()
        {
            var engineers = await _userManager.GetUsersInRoleAsync(Constants.Roles.Engineer);
            return View(engineers.ToList());
        }

        // GET: Create Service Engineer
        [HttpGet]
        public IActionResult CreateEngineer() => View();

        // POST: Create Service Engineer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEngineer(string email, string firstName, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError("", "A user with this email already exists.");
                return View();
            }

            var engineer = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(engineer, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(engineer, Constants.Roles.Engineer);
                await _emailSender.SendEmailAsync(email, "Welcome to ASC",
                    $"<h3>Welcome {firstName}!</h3><p>Your engineer account has been created.</p><p>Email: {email}</p>");
                TempData["Success"] = "Service engineer created successfully!";
                return RedirectToAction("ServiceEngineers");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        // GET: Toggle user active status
        [HttpGet]
        public async Task<IActionResult> ToggleActive(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);
            TempData["Success"] = $"User {(user.IsActive ? "activated" : "deactivated")} successfully.";
            return RedirectToAction("Customers");
        }

        // GET: My Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        // POST: Update Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(string firstName, string lastName)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FirstName = firstName;
            user.LastName = lastName;
            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Profile updated!";
            return RedirectToAction("Profile");
        }
    }
}
