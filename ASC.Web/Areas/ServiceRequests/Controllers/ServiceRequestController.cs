using ASC.DataAccess;
using ASC.Model;
using ASC.Web.Controllers;
using ASC.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    public class ServiceRequestController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ServiceRequestController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> ServiceRequest()
        {
            var masterKeys = await _unitOfWork.Repository<MasterDataKey>().FindAllByAsync(k => k.IsActive);
            ViewBag.Services = masterKeys.ToList();
            return View(new ASC.Model.ServiceRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServiceRequest(ASC.Model.ServiceRequest model)
        {
            if (!ModelState.IsValid)
            {
                var keys = await _unitOfWork.Repository<MasterDataKey>().FindAllByAsync(k => k.IsActive);
                ViewBag.Services = keys.ToList();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            model.UniqueId = Guid.NewGuid().ToString();
            model.CustomerEmail = user!.Email;
            model.Status = "Pending";
            model.RequestedDate = DateTime.UtcNow;
            model.CreatedBy = user.Email;
            model.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<ASC.Model.ServiceRequest>().CreateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            // Send notification email
            await _emailSender.SendEmailAsync(user.Email!,
                "Service Request Created",
                $"<h3>Your service request has been created.</h3><p>Vehicle: {model.VehicleName}</p><p>Services: {model.RequestedServices}</p><p>Status: {model.Status}</p>");

            TempData["Success"] = "Service request created successfully!";
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var request = await _unitOfWork.Repository<ASC.Model.ServiceRequest>().FindAsync(id);
            if (request == null) return NotFound();

            // Admin can assign engineer
            if (User.IsInRole(ASC.Model.Constants.Roles.Admin))
            {
                var engineers = await _userManager.GetUsersInRoleAsync(ASC.Model.Constants.Roles.Engineer);
                ViewBag.Engineers = engineers.ToList();
            }

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(string id, string status, string? engineerEmail)
        {
            var request = await _unitOfWork.Repository<ASC.Model.ServiceRequest>().FindAsync(id);
            if (request == null) return NotFound();

            request.Status = status;
            request.UpdatedBy = User.Identity?.Name;
            request.UpdatedDate = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(engineerEmail))
                request.ServiceEngineer = engineerEmail;

            if (status == "Completed")
                request.CompletedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<ASC.Model.ServiceRequest>().UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();

            // Notify customer
            await _emailSender.SendEmailAsync(request.CustomerEmail!,
                $"Service Request Status Updated: {status}",
                $"<h3>Your service request status has been updated.</h3><p>New Status: <strong>{status}</strong></p>");

            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}
