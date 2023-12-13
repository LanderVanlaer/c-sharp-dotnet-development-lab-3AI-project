using System.ComponentModel;
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

    public ICollection<Group>? Groups
    {
        get => _groups;
        set => SetField(ref _groups, value);
    }

    public ICollection<Payment>? Payments
    {
        get => _payments;
        set => SetField(ref _payments, value);
    }

    public ICollection<User>? Users
    {
        get => _users;
        set => SetField(ref _users, value);
    }

    public User? User
    {
        get => _user;
        set => SetField(ref _user, value);
    }

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

    private User? _user;
    private ICollection<Group>? _groups;
    private ICollection<Payment>? _payments;
    private ICollection<User>? _users;

    #endregion

    #endregion

    #region Fetchers

    public async Task<ICollection<Group>> FetchGroups()
    {
        HttpResponseMessage response = await MakeRequest("groups", MethodType.Get);

        string content = await response.Content.ReadAsStringAsync();
        Groups = JsonConvert.DeserializeObject<List<Group>>(content, _serializerOptions) ??
                 throw new Exception("No groups found");

        return Groups;
    }

    public async Task<User> FetchUser()
    {
        HttpResponseMessage response = await MakeRequest("users/me", MethodType.Get);

        string content = await response.Content.ReadAsStringAsync();
        User = JsonConvert.DeserializeObject<User>(content, _serializerOptions) ??
               throw new Exception("No User found");

        return User;
    }

    public async Task<ICollection<User>> FetchUsers(Guid groupId)
    {
        HttpResponseMessage response = await MakeRequest($"groups/{groupId}/users", MethodType.Get);

        string content = await response.Content.ReadAsStringAsync();
        Users = JsonConvert.DeserializeObject<List<User>>(content, _serializerOptions) ??
                throw new Exception("No Users found");

        return Users;
    }

    public async Task<User> UpdateUser(string? username, string? password)
    {
        if (username == null && password == null)
            throw new InvalidArgumentsException("No username or password given");

        string body = JsonConvert.SerializeObject(new
        {
            username,
            password,
        });

        string content;
        try
        {
            content = await (await MakeRequest("users/me", MethodType.Patch, body)).Content.ReadAsStringAsync();
        }
        catch (ApiError e)
        {
            if (e.Status == HttpStatusCode.Conflict)
                throw new UserNameAlreadyExistsException();

            throw;
        }

        User = JsonConvert.DeserializeObject<User>(content, _serializerOptions) ??
               throw new Exception("No user found");

        Debug.WriteLine("User updated");

        return User;
    }


    public async Task<ICollection<Payment>> FetchPayments(Guid groupId)
    {
        HttpResponseMessage response = await MakeRequest($"groups/{groupId}/payments", MethodType.Get);

        string content = await response.Content.ReadAsStringAsync();
        Payments = JsonConvert.DeserializeObject<List<Payment>>(content, _serializerOptions) ??
                   throw new Exception("No payments found");

        return Payments;
    }

    public async Task<Payment> GetPayment(Guid groupId, Guid paymentId)
    {
        HttpResponseMessage response = await MakeRequest($"groups/{groupId}/payments/{paymentId}", MethodType.Get);

        string content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Payment>(content, _serializerOptions) ??
               throw new Exception("No payment found");
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
    ///     Registers the user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>The user.</returns>
    /// <exception cref="UserNameAlreadyExistsException">If the username already exists.</exception>
    /// <exception cref="ApiError">If the response is not successful.</exception>
    public async Task<User> Register(string username, string password)
    {
        string body = JsonConvert.SerializeObject(new
        {
            username,
            password,
        });

        string content;
        try
        {
            content = await (await MakeRequest("users", MethodType.Post, body)).Content.ReadAsStringAsync();
        }
        catch (ApiError e)
        {
            if (e.Status == HttpStatusCode.Conflict)
                throw new UserNameAlreadyExistsException();
            throw;
        }

        User user = JsonConvert.DeserializeObject<User>(content, _serializerOptions) ??
                    throw new Exception("No user found");

        Debug.WriteLine($"Registered as {username}");

        return user;
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


        throw new ApiError(method, path, response.StatusCode,
            JsonConvert.DeserializeObject<ApiErrorBody>(
                await response.Content.ReadAsStringAsync(),
                _serializerOptions
            )
        );
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

public class ApiError(MethodType method, string path, HttpStatusCode status, ApiErrorBody? body = null)
    : BaseException(body == null
        ? $"Got a {Enum.GetName(typeof(HttpStatusCode), status)}: {status} on {Enum.GetName(typeof(MethodType), method)} {path}"
        : body.Title)
{
    public readonly ApiErrorBody? Body = body;
    public readonly MethodType Method = method;
    public readonly string Path = path;
    public readonly HttpStatusCode Status = status;
}

public class WrongLoginCredentialsException() : BaseException("Wrong username or password");

public class UserNameAlreadyExistsException() : BaseException("Username already exists");

public class InvalidArgumentsException(string message) : BaseException(message);