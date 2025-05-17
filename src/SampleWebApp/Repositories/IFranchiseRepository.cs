using SampleWebApp.Entities;

namespace SampleWebApp.Repositories;

public interface IFranchiseRepository
{
    Task AddOrUpdateFranchise(Franchise franchise);
    Task<Optional<Franchise>> GetFranchiseById(Guid id);
    Task<IEnumerable<Franchise>> GetFranchises();
}

public class FranchiseRepository : IFranchiseRepository
{
    public Task AddOrUpdateFranchise(Franchise franchise)
    {
        Database.Franchises[franchise.Id] = franchise;
        return Task.CompletedTask;
    }

    public Task<Optional<Franchise>> GetFranchiseById(Guid id)
    {
        if (Database.Franchises.TryGetValue(id, out var franchise))
        {
            return Task.FromResult(new Optional<Franchise>(franchise));
        }

        return Task.FromResult(new Optional<Franchise>());
    }

    public Task<IEnumerable<Franchise>> GetFranchises()
    {
        var franchises = Database.Franchises.Values.ToList();
        return Task.FromResult<IEnumerable<Franchise>>(franchises);
    }
}

