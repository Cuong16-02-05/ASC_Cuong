using ASC.DataAccess;
using ASC.Model;
using ASC.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    public class DashboardController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole(ASC.Model.Constants.Roles.Admin);
            var isEngineer = User.IsInRole(ASC.Model.Constants.Roles.Engineer);

            IEnumerable<ServiceRequest> requests;

            if (isAdmin)
            {
                requests = await _unitOfWork.Repository<ServiceRequest>().FindAllAsync();
            }
            else if (isEngineer)
            {
                requests = await _unitOfWork.Repository<ServiceRequest>()
                    .FindAllByAsync(r => r.ServiceEngineer == user!.Email);
            }
            else
            {
                requests = await _unitOfWork.Repository<ServiceRequest>()
                    .FindAllByAsync(r => r.CustomerEmail == user!.Email);
            }

            ViewBag.TotalRequests = requests.Count();
            ViewBag.PendingRequests = requests.Count(r => r.Status == "Pending");
            ViewBag.InProgressRequests = requests.Count(r => r.Status == "InProgress");
            ViewBag.CompletedRequests = requests.Count(r => r.Status == "Completed");

            return View(requests.OrderByDescending(r => r.RequestedDate).Take(10).ToList());
        }
    }
}
