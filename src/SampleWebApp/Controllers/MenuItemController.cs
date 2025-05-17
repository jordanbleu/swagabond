using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Entities;
using SampleWebApp.Repositories;

namespace SampleWebApp.Controllers;

/// <summary>
/// Endpoint for CRUD operations on menu tiems.  This demonstrates a lot of complex / inner types.
/// </summary>
[ApiController]
public class MenuItemController : Controller
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IFranchiseRepository _franchiseRepository;

    public MenuItemController(IMenuItemRepository menuItemRepository, IFranchiseRepository franchiseRepository)
    {
        _menuItemRepository = menuItemRepository;
        _franchiseRepository = franchiseRepository;
    }

    /// <summary>
    /// Get a big ole list of every menu item in the whole system.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/v1/menuitems")]
    public async Task<ActionResult<MenuItemResponse>> GetAll()
    {
        var allItems = await _menuItemRepository.GetMenuItems();

        if (!allItems.Any())
            return Ok(new MenuItemResponse());
        
        var mappedItems = allItems.Select(Map);

        return Ok(new MenuItemResponse
        {
            Items = mappedItems.ToList(),
            TotalCount = allItems.Count()
        });
    }
    
    /// <summary>
    /// Get a single menu item by ID
    /// </summary>
    /// <param name="id">the menu item's ID</param>
    /// <returns></returns>
    [HttpGet]
    [Route(("/api/v1/menuitems/{id}"))]
    public async Task<ActionResult<MenuItemResponseItem>> GetById(Guid id)
    {
        var menuItem = await _menuItemRepository.GetMenuItemById(id);

        if (menuItem.HasValue == false)
            return NotFound();

        return Ok(Map(menuItem.Value!));
    }
    
    /// <summary>
    /// Same as GetById, but includes franchise information as well.
    /// </summary>
    /// <param name="id">The menu item ID</param>
    /// <returns></returns>
    [HttpGet]
    [Route(("/api/v1/menuitems/{id}/full"))]
    public async Task<ActionResult<FullMenuItemGetResponse>> GetByIdFull(Guid id)
    {
        var menuItem = await _menuItemRepository.GetMenuItemById(id);

        if (menuItem.HasValue == false)
            return NotFound();

        var franchise = await _franchiseRepository.GetFranchiseById(menuItem.Value!.FranchiseId);

        if (franchise.HasValue == false)
            return NotFound();

        return Ok(new FullMenuItemGetResponse()
        {
            Item = Map(menuItem.Value!),
            Franchise = new FranchiseInformation()
            {
                Id = franchise.Value!.Id,
                Name = franchise.Value!.Name,
                Slogan = franchise.Value!.Slogan
            }
        });
    }

    /// <summary>
    /// Add a new menu item
    /// </summary>
    /// <param name="request">The menu item request </param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/v1/menuitems")]
    public async Task<ActionResult<MenuItemResponseItem>> Post([FromBody] MenutItemPostRequest request)
    {
        var menuItem = new MenuItem()
        {
            Id = Guid.NewGuid(),
            FranchiseId = request.FranchiseId,
            Name = request.Name,
            Description = request.Description,
            Calories = request.Calories,
            ProteinGrams = request.ProteinGrams
        };

        await _menuItemRepository.AddOrUpdateMenuItem(menuItem);

        return Created($"/api/v1/menuitems/{menuItem.Id}", Map(menuItem));
    }

    private MenuItemResponseItem Map(MenuItem menuItem)
    {
        return new MenuItemResponseItem()
        { 
            Id = menuItem.Id,
            FranchiseId = menuItem.FranchiseId,
            Name = menuItem.Name,
            Description = menuItem.Description,
            NutritionFacts = new MenutItemNutritionFacts()
            {
                Calories = menuItem.Calories,
                ProteinNutritionFacts = new ProteinNutritionFact()
                {
                    ProteinGrams = menuItem.ProteinGrams,
                    ProteinMilligrams = menuItem.ProteinGrams * 1000
                }
            }
        };
    }
}

/// <summary>
/// A full menu item response, as well as the franchise info
/// </summary>
public record FullMenuItemGetResponse
{
    /// <summary>
    /// The menu item 
    /// </summary>
    public MenuItemResponseItem Item { get; set; } = new();

    /// <summary>
    /// The Franchise Info
    /// </summary>
    public FranchiseInformation Franchise { get; set; } = new();

}

/// <summary>
/// An object containing franchise information
/// </summary>
public record FranchiseInformation
{
    /// <summary>
    /// The unique id of the franchise 
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the franchise 
    /// </summary>
    public string Name { get; set; } = String.Empty;
    /// <summary>
    /// The slogan of the franchise, in english.
    /// </summary>
    public string Slogan { get; set; } = String.Empty;
}

/// <summary>
/// A post request for menu items
/// </summary>
public record MenutItemPostRequest
{
    /// <summary>
    /// The franchise ID this menu item is offered by 
    /// </summary>
    public Guid FranchiseId { get; set; }
    /// <summary>
    /// The name of the menu item 
    /// </summary>
    public string Name { get; set; } = String.Empty;
    /// <summary>
    /// The menu item's description in english
    /// </summary>
    public string Description { get; set; } = String.Empty;

    /// <summary>
    /// How many calories is this menu item? 
    /// </summary>
    public int Calories { get; set; } = 0;

    /// <summary>
    /// How many grams of protein does the item contain?
    /// </summary>
    public int ProteinGrams { get; set; } = 0;
}

/// <summary>
/// A response containing multiple menu items
/// </summary>
public record MenuItemResponse
{
    /// <summary>
    /// The list of menu items
    /// </summary>
    public List<MenuItemResponseItem> Items { get; set; } = new();

    /// <summary>
    /// The total count of menu items
    /// </summary>
    public int TotalCount { get; set; } = 0;
}

/// <summary>
/// A single menu item
/// </summary>
public record MenuItemResponseItem
{
    /// <summary>
    /// The menu item's ID
    /// </summary>
    public Guid Id { get; set; } 
    /// <summary>
    /// The franchises unique ID
    /// </summary>
    public Guid FranchiseId { get; set; }
    /// <summary>
    /// The menu item name
    /// </summary>
    public string Name { get; set; }  = String.Empty;
    /// <summary>
    /// The menu item description (english)
    /// </summary>
    public string Description { get; set; } = String.Empty;

    /// <summary>
    /// Nutrition Facts about the menu item
    /// </summary>
    public MenutItemNutritionFacts NutritionFacts { get; set; } = new();
}

/// <summary>
/// A set of menu nutrition facts
/// </summary>
public record MenutItemNutritionFacts
{
    /// <summary>
    /// How many calories the menu item contains 
    /// </summary>
    public int Calories { get; set; } = 0;

    /// <summary>
    /// Specific nutrition info around protein 
    /// </summary>
    public ProteinNutritionFact ProteinNutritionFacts { get; set; } = new();
}

/// <summary>
/// Specific nutrition facts related to protein 
/// </summary>
public record ProteinNutritionFact
{
    /// <summary>
    ///  Grams of protein 
    /// </summary>
    public int ProteinGrams { get; set; } = 0;

    /// <summary>
    /// Milligrams of protein
    /// </summary>
    public int ProteinMilligrams { get; set; } = 0;
}