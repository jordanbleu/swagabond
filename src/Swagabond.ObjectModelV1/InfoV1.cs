namespace Swagabond.ObjectModelV1;

/// <summary>
/// A bunch of helpful information about the API, most of which is for documentation or support purposes.
/// </summary>
public class InfoV1
{
    /// <summary>
    /// Contact info for your API (name)
    /// </summary>
    public string ContactName { get; internal set; } = string.Empty;
    
    /// <summary>
    /// Contact info for your API (url)
    /// </summary>
    public string ContactUrl { get; internal set; } = string.Empty;
    
    /// <summary>
    /// Contact info for your API (email)
    /// </summary>
    public string ContactEmail { get; internal set; } = string.Empty;

    /// <summary>
    /// URL to the ToS for your API
    /// </summary>
    public string TermsOfServiceUrl { get; internal set; } = string.Empty;

    /// <summary>
    ///  URL to license for your API
    /// </summary>
    public string LicenseUrl { get; internal set; } = string.Empty;
    
    /// <summary>
    /// Name of the license for your API
    /// </summary>
    public string LicenseName {get; internal set; } = string.Empty;

    /// <summary>
    /// Returns true if the api has some sorta contact info available (not all fields need to be populated).
    /// </summary>
    public bool HasContactInfo => !string.IsNullOrEmpty(ContactName) || 
                                  !string.IsNullOrEmpty(ContactEmail) || 
                                  !string.IsNullOrEmpty(ContactUrl);

    /// <summary>
    /// Returns true if the api has some sorta license info available (not all fields need to be populated).
    /// </summary>
    public bool HasLicenseInfo => !string.IsNullOrEmpty(LicenseName) || !string.IsNullOrEmpty(LicenseUrl);
    
    public static readonly InfoV1 Empty = new();
}