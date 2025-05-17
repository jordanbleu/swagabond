using Microsoft.AspNetCore.Mvc;
using SampleWebApiApi;
using SampleWebApiApi.Models;

namespace SampleWebAppUI.Controllers;

public class RestaurantsController : Controller
{
    private readonly ISampleWebApiClient _client;
    
    public RestaurantsController()
    {
        _client = new SampleWebApiFlurlClient("http://localhost:5240");
    }
    
    public async Task<ActionResult> Index()
    {
        var result = await _client.GetApiV1RestaurantsAsync(new());

        var viewmodel = new RestaurantsViewModel();
        
        foreach (var item in result.Items)
        {
            var franchiseInfo =
                await _client.GetApiV1FranchisesidAsync(new(), item.FranchiseId);
            
            viewmodel.Restaurants.Add(new()
            {
                Id = item.Id,
                FranchiseId = item.FranchiseId,
                State = item.State.ToString(),
                StoreNumber = item.StoreNumber,
                Address = item.Address,
                FranchiseName = franchiseInfo.Name,
                FranchiseSlogan = franchiseInfo.Slogan,
                Zip = item.Zip
            });
        }
        
        return View("Restaurants", viewmodel);
    }
}

public class RestaurantsViewModel
{
    public List<RestaurantItem> Restaurants { get; set; } = new List<RestaurantItem>();
}

public class RestaurantItem
{
    public Guid Id { get; set; }
    public Guid FranchiseId { get; set; }
    public int StoreNumber { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    
    public string FranchiseName { get; set; }
    public string FranchiseSlogan { get; set; }
}