//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was generated by a tool. 
//
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------

using System;

namespace SampleWebApiApi.Models;

/// <summary>
/// Request for creating a new restaurant
/// </summary>
public record RestaurantPostRequest
{
    public Guid FranchiseId { get; set; }
    public int StoreNumber { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public State State { get; set; }
}
