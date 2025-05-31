using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// A hyperlink, containing text and a URL.
/// </summary>
public class HrefV1 : IObjectV1
{
    
    /// <summary>
    /// The text for the link
    /// </summary>
    public string Text { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The URL for the link 
    /// </summary>
    public string Url { get; internal set; } = string.Empty;
    
    public static readonly HrefV1 Empty = new ();
    
    /// <summary>
    /// If this is true, the link is empty.
    /// </summary>
    public bool IsEmpty { get; set; } = true;
}