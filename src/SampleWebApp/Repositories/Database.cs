using SampleWebApp.Entities;

namespace SampleWebApp.Repositories;

public class Database
{
    public static Dictionary<Guid, Franchise> Franchises { get; set; } = new();
    public static Dictionary<Guid, Restaurant> Restaurants { get; set; } = new();
    public static Dictionary<Guid, MenuItem> MenuItems { get; set; } = new();
}