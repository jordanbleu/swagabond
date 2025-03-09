namespace Swagabond.Core.ObjectModel;

public enum ApiContentType
{
    None,
    Json,
    Xml,
    PlainText,
    FormUrlEncoded,
    MultipartFormData,
    TextHtml,
    OctetStream
}

public static class ApiContentTypeMapper
{
    /// <summary>
    /// We do our best to map the content type to a known type.  If we can't, we default to plain text.
    /// </summary>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static ApiContentType FromString(string contentType)
    {
        return contentType.ToLower() switch
        {
            "" => ApiContentType.None,
            "application/json" => ApiContentType.Json,
            "application/xml" => ApiContentType.Xml,
            "text/plain" => ApiContentType.PlainText,
            "application/x-www-form-urlencoded" => ApiContentType.FormUrlEncoded,
            "multipart/form-data" => ApiContentType.MultipartFormData,
            "text/html" => ApiContentType.TextHtml,
            "application/octet-stream" => ApiContentType.OctetStream,
            _ => ApiContentType.PlainText
        };
    }
    
     
}