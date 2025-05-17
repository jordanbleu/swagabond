namespace SampleWebApp.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public Guid FranchiseId { get; set; }
    public int StoreNumber { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public State State { get; set; }

}