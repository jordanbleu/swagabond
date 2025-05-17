using SampleWebApp.Entities;
using SampleWebApp.Repositories;

public interface IRestaurantRepository
{
    Task AddOrUpdateRestaurant(Restaurant restaurant);
    Task<Optional<Restaurant>> GetRestaurantById(Guid id);
    Task<IEnumerable<Restaurant>> GetRestaurants();
    Task DeleteRestaurant(Guid id);
}

public class RestaurantRepository : IRestaurantRepository
{
    public Task AddOrUpdateRestaurant(Restaurant restaurant)
    {
        Database.Restaurants[restaurant.Id] = restaurant;
        return Task.CompletedTask;
    }

    public Task<Optional<Restaurant>> GetRestaurantById(Guid id)
    {
        if (Database.Restaurants.TryGetValue(id, out var restaurant))
        {
            return Task.FromResult(new Optional<Restaurant>(restaurant));
        }

        return Task.FromResult(new Optional<Restaurant>());
    }

    public Task<IEnumerable<Restaurant>> GetRestaurants()
    {
        var restaurants = Database.Restaurants.Values.ToList();
        return Task.FromResult<IEnumerable<Restaurant>>(restaurants);
    }

    public Task DeleteRestaurant(Guid id)
    {
        if (Database.Restaurants.ContainsKey(id))
        {
            Database.Restaurants.Remove(id);
        }

        return Task.CompletedTask;
    }
}