namespace Swagabond.ObjectModelV1;

public class EnumOptionV1
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"EnumOptionV1 {Name} = {Value}";
    }
}