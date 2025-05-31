using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Entities;
using System.Linq;

namespace SampleWebApi.Controllers;

[ApiController]
public class RestaurantController : Controller
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantController(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    /// <summary>
    /// Get a list of every single restaurant in the system
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("api/v1/restaurants")]
    [ProducesResponseType(200, Type = typeof(RestaurantGetResponse))]
    public async Task<ActionResult> GetRestaurants()
    {
        var restaurants = await _restaurantRepository.GetRestaurants();

        var responseItems = restaurants.Select(Map).ToList();
        
        var response = new RestaurantGetResponse()
        {
            Items = responseItems
        };
        
        return Ok(response);
    }
    
    /// <summary>
    /// Get a single restaurant by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/v1/restaurants/{id}")]
    [ProducesResponseType(200, Type = typeof(RestaurantGetResponseItem))]
    public async Task<ActionResult<RestaurantGetResponseItem>> GetRestaurantById(Guid id)
    {
        var restaurant = await _restaurantRepository.GetRestaurantById(id);
        
        if (restaurant.HasValue)
        {
            return Ok(Map(restaurant.Value!));
        }
        
        return NotFound();
    }
    
    /// <summary>
    /// Create a new restaurant
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/v1/restaurants")]
    [ProducesResponseType(201, Type = typeof(RestaurantGetResponseItem))]
    public async Task<ActionResult<RestaurantGetResponseItem>> AddOrUpdateRestaurant([FromBody] RestaurantPostRequest request)
    {
        var restaurant = new Restaurant()
        {
            Id = Guid.NewGuid(),
            FranchiseId = request.FranchiseId,
            StoreNumber = request.StoreNumber,
            Address = request.Address,
            Zip = request.Zip,
            City = request.City,
            State = request.State
        };
        await _restaurantRepository.AddOrUpdateRestaurant(restaurant);
        return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, Map(restaurant));
    }
    
    /// <summary>
    /// Deletes a single restaurant by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("api/v1/restaurants/{id}")]
    [ProducesResponseType(204)]
    public async Task<ActionResult> DeleteRestaurant(Guid id)
    {
        var restaurant = await _restaurantRepository.GetRestaurantById(id);
        
        if (restaurant.HasValue)
        {
            await _restaurantRepository.DeleteRestaurant(id);
            return NoContent();
        }
        
        return NotFound();
    }
    
    private static RestaurantGetResponseItem Map(Restaurant r)
    {
        return new RestaurantGetResponseItem()
        {
            Id = r.Id,
            FranchiseId = r.FranchiseId,
            StoreNumber = r.StoreNumber,
            Address = r.Address,
            Zip = r.Zip,
            City = r.City,
            State = r.State
        };
    }

}

/// <summary>
/// Request for creating a new restaurant
/// </summary>
public class RestaurantPostRequest
{
    /// <summary>
    /// The Franchise ID for the restaurant.
    /// </summary>
    public Guid FranchiseId { get; set; }
    
    /// <summary>
    /// Unique number franchises use to identify the restaurant.
    /// </summary>
    public int StoreNumber { get; set; }
    
    /// <summary>
    /// The physical address of the restaurant 
    /// </summary>
    public string Address { get; set; }
    
    /// <summary>
    /// The 5 digit postal code of the restaurant.
    /// </summary>
    public string Zip { get; set; }
    
    /// <summary>
    /// The city the restaurant is located in.
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// The state code the restaurant is located in.
    /// </summary>
    public State State { get; set; }    
}



/// <summary>
/// The response containing the list of restuarants 
/// </summary>
public class RestaurantGetResponse
{
    /// <summary>
    /// List of restaurants 
    /// </summary>
    public List<RestaurantGetResponseItem> Items { get; set; }
}


/// <summary>
/// A single restaurant item, which includes the restaurant's name, address, phone number, website, and description.
/// </summary>
public class RestaurantGetResponseItem
{
    /// <summary>
    /// The unique identifier for the restaurant.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The Franchise ID for the restaurant.
    /// </summary>
    public Guid FranchiseId { get; set; }
    
    /// <summary>
    /// Unique number franchises use to identify the restaurant.
    /// </summary>
    public int StoreNumber { get; set; }
    
    /// <summary>
    /// The physical address of the restaurant 
    /// </summary>
    public string Address { get; set; }
    
    /// <summary>
    /// The 5 digit postal code of the restaurant.
    /// </summary>
    public string Zip { get; set; }
    
    /// <summary>
    /// The city the restaurant is located in.
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// The state code the restaurant is located in.
    /// </summary>
    public State State { get; set; }
}

