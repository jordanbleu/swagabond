namespace Swagabond.Core.Constants;

/// <summary>
/// Consts related to how swagabond uses openapi extensions to add some proprietary functionality to generated output. 
/// </summary>
public static class ExtensionConstants
{
    /// <summary>
    /// This is a list of extension names we will check to find enum names.
    ///
    /// OpenAPI doesn't natively support enums as key-value pairs, so we have to use extensions.
    /// https://stackoverflow.com/questions/66465888/how-to-define-enum-mapping-in-openapi
    ///
    /// The only supported extensions we recognize need to be just a basic list of strings, and the list
    /// of strings needs to be the same length as the enum values since we match them index by index.
    /// </summary>
    public static readonly ICollection<string> EnumNamesExtension = new List<string> 
    {
        "x-enumNames",
        "x-enum-varnames",
        "x-speakeasy-enums"
    };
}