using ASC.Business;
using ASC.Model;
using ASC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    public class MasterDataController : BaseController
    {
        private readonly IMasterDataOperations _masterDataOps;

        public MasterDataController(IMasterDataOperations masterDataOps)
        {
            _masterDataOps = masterDataOps;
        }

        // GET: Master Keys list
        public async Task<IActionResult> MasterKeys()
        {
            var keys = await _masterDataOps.GetAllMasterKeysAsync();
            return View(keys.OrderBy(k => k.Name).ToList());
        }

        // POST: Create new Master Key
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMasterKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Key name cannot be empty.";
                return RedirectToAction("MasterKeys");
            }
            await _masterDataOps.CreateMasterKeyAsync(name, User.Identity?.Name ?? "system");
            TempData["Success"] = $"Master Key '{name}' created successfully.";
            return RedirectToAction("MasterKeys");
        }

        // POST: Toggle Master Key active status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMasterKey(string id, string name, bool isActive)
        {
            await _masterDataOps.UpdateMasterKeyAsync(id, name, !isActive, User.Identity?.Name ?? "system");
            TempData["Success"] = "Master Key updated.";
            return RedirectToAction("MasterKeys");
        }

        // GET: Master Values for a key
        public async Task<IActionResult> MasterValues(string keyId, string keyName)
        {
            var values = await _masterDataOps.GetMasterValuesByKeyAsync(keyId);
            ViewBag.KeyId = keyId;
            ViewBag.KeyName = keyName;
            return View(values.OrderBy(v => v.Name).ToList());
        }

        // POST: Create new Master Value
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMasterValue(string keyId, string keyName, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Value name cannot be empty.";
                return RedirectToAction("MasterValues", new { keyId, keyName });
            }
            await _masterDataOps.CreateMasterValueAsync(keyId, name, User.Identity?.Name ?? "system");
            TempData["Success"] = $"Master Value '{name}' created.";
            return RedirectToAction("MasterValues", new { keyId, keyName });
        }

        // POST: Toggle Master Value active status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMasterValue(string id, string name, bool isActive, string keyId, string keyName)
        {
            await _masterDataOps.UpdateMasterValueAsync(id, name, !isActive, User.Identity?.Name ?? "system");
            TempData["Success"] = "Master Value updated.";
            return RedirectToAction("MasterValues", new { keyId, keyName });
        }
    }
}
