using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace c_sharp_dotnet_development_lab_3AI_project.utils;

/// <summary>
///     https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api
/// </summary>
public class ApiResponse
{
    protected ApiResponse(HttpStatusCode status, string? message = null)
    {
        Status = status;
        Title = message ?? GetHttpStatusDescription(status);
    }

    public HttpStatusCode Status { get; }
    public string Title { get; }

    public static ActionResult NotFound => Create(HttpStatusCode.NotFound);
    public static ActionResult Unauthorized => Create(HttpStatusCode.Unauthorized);
    public static ActionResult Forbidden => Create(HttpStatusCode.Forbidden);
    public static ActionResult BadRequest => Create(HttpStatusCode.BadRequest);

    public static ActionResult Create(HttpStatusCode status, string? message = null) =>
        new ObjectResult(new ApiResponse(status, message)) { StatusCode = (int)status };

    /// <summary>
    ///     Descriptions for Http Status Code
    ///     https://stackoverflow.com/a/63551354/13165967
    /// </summary>
    /// <param name="code">The http status code</param>
    /// <returns></returns>
    public static string GetHttpStatusDescription(HttpStatusCode code)
    {
        return (int)code switch
        {
            100 => "Continue",
            101 => "Switching Protocols",
            102 => "Processing",
            200 => "OK",
            201 => "Created",
            202 => "Accepted",
            203 => "Non-Authoritative Information",
            204 => "No Content",
            205 => "Reset Content",
            206 => "Partial Content",
            207 => "Multi-Status",
            300 => "Multiple Choices",
            301 => "Moved Permanently",
            302 => "Found",
            303 => "See Other",
            304 => "Not Modified",
            305 => "Use Proxy",
            307 => "Temporary Redirect",
            400 => "Bad Request",
            401 => "Unauthorized",
            402 => "Payment Required",
            403 => "Forbidden",
            404 => "Not Found",
            405 => "Method Not Allowed",
            406 => "Not Acceptable",
            407 => "Proxy Authentication Required",
            408 => "Request Timeout",
            409 => "Conflict",
            410 => "Gone",
            411 => "Length Required",
            412 => "Precondition Failed",
            413 => "Request Entity Too Large",
            414 => "Request-Uri Too Long",
            415 => "Unsupported Media Type",
            416 => "Requested Range Not Satisfiable",
            417 => "Expectation Failed",
            422 => "Unprocessable Entity",
            423 => "Locked",
            424 => "Failed Dependency",
            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            504 => "Gateway Timeout",
            505 => "Http Version Not Supported",
            507 => "Insufficient Storage",
            _ => string.Empty,
        };
    }
}