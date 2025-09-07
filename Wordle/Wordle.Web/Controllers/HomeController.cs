using Microsoft.AspNetCore.Mvc;
using Wordle.Web.Services;

namespace Wordle.Web.Controllers;

public class HomeController : Controller
{
    private readonly IApiHealthService _apiHealthService;

    public HomeController(IApiHealthService apiHealthService)
    {
        _apiHealthService = apiHealthService;
    }

    public async Task<IActionResult> Index()
    {
        var apiStatus = await _apiHealthService.GetApiStatusAsync();
        return View(apiStatus);
    }
}