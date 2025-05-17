namespace SampleWebApp.Entities;

public record MenuItem
{
    public Guid Id { get; set; }
    public Guid FranchiseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    
    public int Calories { get; set; }
    public int ProteinGrams { get; set; }
}