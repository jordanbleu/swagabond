namespace Swagabond.ObjectModelV1.Interfaces;

public interface IObjectV1
{
    /// <summary>
    /// If true, the object is empty and so all fields will be their default.
    /// </summary>
    bool IsEmpty { get; }
}