using ASC.Utilities;
using ASC.Web.Configuration;
using ASC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASC.Web.Controllers
{
    public class HomeController : AnonymousController
    {
        private readonly ApplicationSettings _settings;
        private readonly TransientLoggerService _transient1;
        private readonly TransientLoggerService _transient2;
        private readonly ScopedLoggerService _scoped1;
        private readonly ScopedLoggerService _scoped2;
        private readonly SingletonLoggerService _singleton1;
        private readonly SingletonLoggerService _singleton2;

        public HomeController(
            IOptions<ApplicationSettings> options,
            TransientLoggerService transient1,
            TransientLoggerService transient2,
            ScopedLoggerService scoped1,
            ScopedLoggerService scoped2,
            SingletonLoggerService singleton1,
            SingletonLoggerService singleton2)
        {
            _settings = options.Value;
            _transient1 = transient1;
            _transient2 = transient2;
            _scoped1 = scoped1;
            _scoped2 = scoped2;
            _singleton1 = singleton1;
            _singleton2 = singleton2;
        }

        // GET /
        public IActionResult Index()
        {
            HttpContext.Session.SetSession("Test", _settings);
            return View();
        }

        // GET /Home/DIDemo — Lab 1 Section 10: chứng minh 3 loại DI lifetime
        public IActionResult DIDemo()
        {
            ViewBag.Transient1   = _transient1.GetOperationId();
            ViewBag.Transient2   = _transient2.GetOperationId();
            ViewBag.TransientSame = _transient1.GetOperationId() == _transient2.GetOperationId();

            ViewBag.Scoped1   = _scoped1.GetOperationId();
            ViewBag.Scoped2   = _scoped2.GetOperationId();
            ViewBag.ScopedSame = _scoped1.GetOperationId() == _scoped2.GetOperationId();

            ViewBag.Singleton1   = _singleton1.GetOperationId();
            ViewBag.Singleton2   = _singleton2.GetOperationId();
            ViewBag.SingletonSame = _singleton1.GetOperationId() == _singleton2.GetOperationId();

            return View();
        }

        public IActionResult Error() => View();
    }
}
