using System.Net;
using desktopapplication.Models;
using Newtonsoft.Json;

namespace desktopapplication.services.api;

/// <summary>
///     https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/rest?view=net-maui-8.0#create-the-httpclient-object
/// </summary>
public class RestApiService : IRepository
{
    #region Fields

    private readonly HttpClient _client = new();

    private readonly JsonSerializerSettings _serializerOptions = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        PreserveReferencesHandling = PreserveReferencesHandling.All,
    };
#if DEBUG
    private string? _jsonWebToken =
        // ReSharper disable once StringLiteralTypo
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VybmFtZSIsImp0aSI6IjA4ZGJlZGIyLTdiNzEtNDc1My04MWU0LWU4YWM3OTI3Y2ZlOCIsImV4cCI6MTgwMTUzMjU0NCwiaXNzIjoibG9jYWxob3N0OjUyNzkiLCJhdWQiOiJsb2NhbGhvc3Q6NTI3OSJ9.EQl8E0Y5KkZr2XRPGJ2Sywr6ExKqRS9T8k6Q76EfA-E";
#else
    private string? _jsonWebToken = null;
#endif

    public bool IsAuthenticated => _jsonWebToken != null;

    #region Data

    private List<Group>? _groups;

    #endregion

    #endregion

    #region Getters

    public ICollection<Group>? Groups() => _groups;

    #endregion

    #region Fetchers

    public async Task<ICollection<Group>> FetchGroups()
    {
        HttpResponseMessage response = await MakeRequest("groups", MethodType.Get);

        string content = await response.Content.ReadAsStringAsync();
        _groups = JsonConvert.DeserializeObject<List<Group>>(content, _serializerOptions) ??
                  throw new Exception("No groups found");

        return _groups;
    }

    /// <summary>
    ///     Makes a request and checks if the response is successful.
    ///     If the response is successful, the response is returned.
    ///     Else an exception is thrown.
    /// </summary>
    /// <param name="path">
    ///     The path to the endpoint.
    /// </param>
    /// <param name="method">
    ///     The method to use.
    /// </param>
    /// <param name="body">
    ///     The body to send.
    ///     Only used for POST, PUT and PATCH.
    /// </param>
    /// <returns>
    ///     A <see cref="HttpResponseMessage" /> if the response is successful.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the method is not supported.
    /// </exception>
    /// <exception cref="Exception">
    ///     If the response is not successful.
    /// </exception>
    private async Task<HttpResponseMessage> MakeRequest(string path, MethodType method, string body = "")
    {
        _client.DefaultRequestHeaders.Remove("Authorization");
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jsonWebToken);

        Uri uri = new(Constants.RestApi.BaseUrl + path);

        HttpResponseMessage response = method switch
        {
            MethodType.Get => await _client.GetAsync(uri),
            MethodType.Post => await _client.PostAsync(uri, new StringContent(body)),
            MethodType.Put => await _client.PutAsync(uri, new StringContent(body)),
            MethodType.Patch => await _client.PatchAsync(uri, new StringContent(body)),
            MethodType.Delete => await _client.DeleteAsync(uri),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
        };

        if (response.IsSuccessStatusCode)
            return response;

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                _jsonWebToken = null;
                throw new ApiError("Unauthorized");
            default:
                throw new ApiError(method, path, response.StatusCode);
        }
    }

    #endregion
}

public enum MethodType
{
    Get = 1,
    Post = 2,
    Put = 3,
    Patch = 5,
    Delete = 4,
}

public class ApiError : Exception
{
    public ApiError(string message) : base(message)
    {
    }

    public ApiError(MethodType methodType, string path, HttpStatusCode status) : base(
        $"Got a {Enum.GetName(typeof(HttpStatusCode), status)}: {status} on {Enum.GetName(typeof(MethodType), methodType)} {path}")
    {
    }
}