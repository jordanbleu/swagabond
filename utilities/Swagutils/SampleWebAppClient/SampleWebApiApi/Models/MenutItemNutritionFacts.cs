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
/// A set of menu nutrition facts
/// </summary>
public record MenutItemNutritionFacts
{
    public int Calories { get; set; }
    public ProteinNutritionFact ProteinNutritionFacts { get; set; }
}
