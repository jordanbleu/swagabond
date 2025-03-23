using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

public class ApiInfo
{
    /// <summary>
    /// A title for your API
    /// </summary>
    public string Title { get; internal set; } = string.Empty;

    /// <summary>
    /// A description of your API
    /// </summary>
    public string Description { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The version of your API
    /// </summary>
    public string Version { get; internal set; } = string.Empty;

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
    /// URL to TOS for your API
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

    public static readonly ApiInfo Empty = new();

    /// <summary>
    /// Returns true if the api has some sorta contact info available.
    /// </summary>
    public bool HasContactInfo => !string.IsNullOrEmpty(ContactName) || !string.IsNullOrEmpty(ContactEmail) || !string.IsNullOrEmpty(ContactUrl);

    /// <summary>
    /// Returns true if the api has some sorta license info available.
    /// </summary>
    public bool HasLicenseInfo => !string.IsNullOrEmpty(LicenseName) || !string.IsNullOrEmpty(LicenseUrl);
    
    public static ApiInfo FromOpenApi(OpenApiInfo info)
    {
        if (info is null)
            return new();
        
        var apiInfo = new ApiInfo();

        apiInfo.Title = info.Title;
        apiInfo.Description = info.Description;
        apiInfo.Version = info.Version;
        apiInfo.TermsOfServiceUrl = info.TermsOfService?.ToString() ?? string.Empty;

        var contact = info.Contact;
        if (contact is not null)
        {
            apiInfo.ContactName = contact?.Name ?? string.Empty;
            apiInfo.ContactEmail = contact?.Email ?? string.Empty;
            apiInfo.ContactUrl = contact?.Url?.ToString() ?? string.Empty;
        }

        var license = info.License;
        if (license is not null)
        {
            apiInfo.LicenseName = license?.Name ?? string.Empty;
            apiInfo.LicenseUrl = license?.Url?.ToString() ?? string.Empty;
        }

        return apiInfo;
    }
}