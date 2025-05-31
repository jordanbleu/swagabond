using SampleWebApi.Entities;
using SampleWebApi.Repositories;

namespace SampleWebApi;

public static class TestDataPopulator
{
    public static void AddTestData()
    {
        var franchiseId1 = Guid.NewGuid();
        var franchiseId2 = Guid.NewGuid();
        var franchiseId3 = Guid.NewGuid();
        
        
        var franchises = new List<Franchise>()
        {
            new()
            {
                Id = franchiseId1,
                Name = "Paco's Tacos",
                Slogan = "Hard shells, hard arteries, soft bowel movements."
            },
            new() 
            {
                Id = franchiseId2,
                Name = "Bingus Burger",
                Slogan = "Don't be a dingus, buy burgers at Bingus."
            },
            new()
            {
                Id = franchiseId3,
                Name = "Salad Galaxy",
                Slogan = "Lettuce take you to the stars!"
            }
        };
        
        foreach (var franchise in franchises)
        {
            Database.Franchises.Add(franchise.Id, franchise);
        }
        
        // I would like to thank AI for these items
        var menuItems = new List<MenuItem>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId1,
                Name = "Taco Supreme",
                Description = "A taco with all the fixings.",
                Calories = 500,
                ProteinGrams = 20
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId1,
                Name = "Burrito Grande",
                Description = "A burrito that's bigger than your dreams.",
                Calories = 800,
                ProteinGrams = 30
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId2,
                Name = "Big Bingus Burger",
                Description = "A burger so big, you'll need three hands.",
                Calories = 1000,
                ProteinGrams = 50
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId3,
                Name = "Galactic Greens Salad",
                Description = "A salad that's out of this world.",
                Calories = 200,
                ProteinGrams = 5
            }
        };
        
        foreach (var menuItem in menuItems)
        {
            Database.MenuItems.Add(menuItem.Id, menuItem);
        }

        var stores = new List<Restaurant>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId1,
                State = State.Wisconsin,
                City = "Marshfield",
                Address = "1213 S Central Ave",
                Zip = "54449",
                StoreNumber = 12345,
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId1,
                State = State.Illinois,
                City = "Chicago",
                Address = "555 Main St",
                Zip = "60601",
                StoreNumber = 94
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId2,
                State = State.Wisconsin,
                City = "Madison",
                Address = "1234 State St",
                Zip = "53703",
                StoreNumber = 99
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId3,
                State = State.Illinois,
                City = "Chicago",
                Address = "456 Elm St",
                Zip = "60602",
                StoreNumber = 6457
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId3,
                State = State.Washington,
                City = "Seattle",
                Address = "456 Salad St",
                Zip = "98039",
                StoreNumber = 2453
            },
            new()
            {
                Id = Guid.NewGuid(),
                FranchiseId = franchiseId3,
                State = State.Hawaii,
                City = "Honolulu",
                Address = "456 Palm Tree Drive",
                Zip = "96804",
                StoreNumber = 465
            },
        };
        
        foreach (var store in stores)
        {
            Database.Restaurants.Add(store.Id, store);
        }


    }
}