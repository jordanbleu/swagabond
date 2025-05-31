namespace SampleWebApi.Entities;

public record Franchise
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slogan { get; set; }
}