using AksUpdates.Api.Storage;
using AksUpdates.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AksUpdates.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureTableReadStorage azureTableStorage;

        public HomeController(ILogger<HomeController> logger, IAzureTableReadStorage azureTableStorage)
        {
            _logger = logger;
            this.azureTableStorage = azureTableStorage;
        }

        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await azureTableStorage.GetAllData();
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}