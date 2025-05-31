using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiApi;
using SampleWebAppUI.Models;

namespace SampleWebAppUI.Controllers;

public class HomeController : Controller
{
    private readonly ISampleWebApiClient _client;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _client = new SampleWebApiFlurlClient("http://localhost:5240");
    }

    public async Task<ActionResult<HomeViewModel>> Index()
    {
        var result = await _client.GetApiV1FranchisesAsync(new());
        
        return View(new HomeViewModel()
        {
            Franchises = result.Select(x => new FranchiseItem()
            {
                Id = x.Id,
                Name = x.Name,
                Slogan = x.Slogan
            }).ToList()
        });
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class HomeViewModel()
{
    public List<FranchiseItem> Franchises { get; set; } = new List<FranchiseItem>();
}

public class FranchiseItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slogan { get; set; }
}