using SampleWebApp.Entities;

namespace SampleWebApp.Repositories;


public interface IMenuItemRepository
{
    Task AddOrUpdateMenuItem(MenuItem menuItem);
    Task<Optional<MenuItem>> GetMenuItemById(Guid id);
    Task<IEnumerable<MenuItem>> GetMenuItems();
}

public class MenuItemRepository : IMenuItemRepository
{
    public Task AddOrUpdateMenuItem(MenuItem menuItem)
    {
        Database.MenuItems[menuItem.Id] = menuItem;
        return Task.CompletedTask;
    }

    public Task<Optional<MenuItem>> GetMenuItemById(Guid id)
    {
        if (Database.MenuItems.TryGetValue(id, out var menuItem))
        {
            return Task.FromResult(new Optional<MenuItem>(menuItem));
        }

        return Task.FromResult(new Optional<MenuItem>());
    }

    public Task<IEnumerable<MenuItem>> GetMenuItems()
    {
        var menuItems = Database.MenuItems.Values.ToList();
        return Task.FromResult<IEnumerable<MenuItem>>(menuItems);
    }
}
