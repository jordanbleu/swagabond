namespace SampleWebApp.Entities;

public class Optional<T> where T : class
{
    public Optional() { }
    
    public Optional(T? value)
    {
        if (value != null)
        {
            Value = value;
            HasValue = true;
        }
    }

    public T? Value { get; set; } = default;
    public bool HasValue { get; set; } = false;
    
}