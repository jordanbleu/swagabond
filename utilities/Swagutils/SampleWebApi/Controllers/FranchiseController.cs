using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Entities;
using SampleWebApi.Repositories;

namespace SampleWebApi.Controllers;

[ApiController]
public class FranchiseController : Controller
{
    private readonly IFranchiseRepository _franchiseRepository;

    public FranchiseController(IFranchiseRepository franchiseRepository)
    {
        _franchiseRepository = franchiseRepository;
    }

    /// <summary>
    /// Returns a list of each franchise in the system.  This endpoint doesn't use a wrapper class
    /// for the result items, and returns the items directly, but the generated code should still work
    /// just fine.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("api/v1/franchises")]
    [ProducesResponseType(200, Type = typeof(List<FranchiseGetResponseItem>))]
    public async Task<ActionResult<List<FranchiseGetResponseItem>>> GetFranchises()
    {
        var result = await _franchiseRepository.GetFranchises();
        
        var responseItems = result.Select(Map).ToList();

        return Ok(responseItems);
    }
    
    [HttpGet]
    [Route("api/v1/franchises/{id}")]
    [ProducesResponseType(200, Type = typeof(FranchiseGetResponseItem))]
    public async Task<ActionResult<FranchiseGetResponseItem>> GetFranchiseById(Guid id)
    {
        var franchise = await _franchiseRepository.GetFranchiseById(id);
        
        if (franchise.HasValue)
        {
            var resp = Map(franchise.Value!);
            return Ok(resp);
        }
        
        return NotFound();
    }

    private FranchiseGetResponseItem Map(Franchise arg)
    {
        return new FranchiseGetResponseItem()
        {
            Id = arg.Id,
            Name = arg.Name,
            Slogan = arg.Slogan
        };
    }
}

/// <summary>
/// A single franchise response item.
/// </summary>
public record FranchiseGetResponseItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slogan { get; set; }
}