﻿using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using desktopapplication.Models;
using Newtonsoft.Json;

namespace desktopapplication.services.api;

/// <summary>
///     https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/rest?view=net-maui-8.0#create-the-httpclient-object
/// </summary>
public class RestApiService : IRepository
{
    #region Getters

    public ICollection<Group>? Groups() => _groups;

    #endregion

    #region Fields

    private readonly HttpClient _client = new();

    private readonly JsonSerializerSettings _serializerOptions = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        PreserveReferencesHandling = PreserveReferencesHandling.All,
    };

    private string? _jsonWebToken;

    public string? JsonWebToken
    {
        get => _jsonWebToken;
        set
        {
            SetField(ref _jsonWebToken, value);
            OnPropertyChanged(nameof(IsAuthenticated));
        }
    }

    public bool IsAuthenticated => JsonWebToken != null;

    #region Data

    private List<Group>? _groups;

    #endregion

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
    ///     Logs in the user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <exception cref="WrongLoginCredentialsException">
    ///     If the username or password is wrong.
    /// </exception>
    /// <exception cref="ApiError">
    ///     If the response is not successful.
    /// </exception>
    public async Task Login(string username, string password)
    {
        JsonWebToken = null;

        string body = JsonConvert.SerializeObject(new
        {
            username,
            password,
        });

        string content;
        try
        {
            content = await (await MakeRequest("auth/login", MethodType.Post, body)).Content.ReadAsStringAsync();
        }
        catch (ApiError e)
        {
            if (e.Status == HttpStatusCode.Unauthorized)
                throw new WrongLoginCredentialsException();
            throw;
        }

        LoginResponse token = JsonConvert.DeserializeObject<LoginResponse>(content, _serializerOptions) ??
                              throw new Exception("No token found");

        JsonWebToken = token.Token;

        Debug.WriteLine($"Logged in as {username}, token: {JsonWebToken}");
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
    /// <exception cref="ApiError">
    ///     If the response is not successful.
    /// </exception>
    private async Task<HttpResponseMessage> MakeRequest(string path, MethodType method, string body = "")
    {
        Debug.WriteLine($"Making a {Enum.GetName(typeof(MethodType), method)} request to {path}");

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _client.DefaultRequestHeaders.Remove("Authorization");
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JsonWebToken);

        Uri uri = new(Constants.RestApi.BaseUrl + path);

        HttpResponseMessage response = method switch
        {
            MethodType.Get => await _client.GetAsync(uri),
            MethodType.Post => await _client.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")),
            MethodType.Put => await _client.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")),
            MethodType.Patch => await _client.PatchAsync(uri,
                new StringContent(body, Encoding.UTF8, "application/json")),
            MethodType.Delete => await _client.DeleteAsync(uri),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
        };

        //If the response is successful, return the response.
        if (response.IsSuccessStatusCode)
            return response;

        //Response is not successful, throw an exception.

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            JsonWebToken = null;

        throw new ApiError(method, path, response.StatusCode);
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    ///     Sets the field and calls <see cref="OnPropertyChanged" /> if the value has changed.
    /// </summary>
    /// <param name="field">
    ///     The field to set.
    /// </param>
    /// <param name="value">
    ///     The value to set.
    /// </param>
    /// <param name="propertyName">
    ///     The name of the property.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the field.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true" /> if the value has changed, else <see langword="false" />.
    /// </returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
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

public class ApiError(MethodType method, string path, HttpStatusCode status) : BaseException(
    $"Got a {Enum.GetName(typeof(HttpStatusCode), status)}: {status} on {Enum.GetName(typeof(MethodType), method)} {path}")
{
    public HttpStatusCode Status { get; } = status;
    public string Path { get; } = path;
    public MethodType Method { get; } = method;
}

public class WrongLoginCredentialsException() : BaseException("Wrong username or password");