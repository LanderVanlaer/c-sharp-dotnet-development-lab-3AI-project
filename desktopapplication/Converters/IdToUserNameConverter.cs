using System.Diagnostics;
using System.Globalization;
using desktopapplication.Models;
using desktopapplication.services;

namespace desktopapplication.Converters;

public class IdToUserNameConverter : IValueConverter
{
    private readonly IRepository _repository = DependencyService.Get<IRepository>();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ICollection<User>? users = _repository.Users;

        if (users == null)
        {
            Debug.WriteLine(nameof(IdToUserNameConverter) + " Please fetch users first, before using this converter");
            return value;
        }

        // ReSharper disable once InvertIf
        if (value is not Guid guid)
        {
            Debug.WriteLine(nameof(IdToUserNameConverter) + " Only use this converter with Guids");
            return value;
        }

        return users.FirstOrDefault(user => user.Id == guid)?.Username ?? value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Guid)
            return value;

        ICollection<User>? users = _repository.Users;

        if (users == null)
        {
            Debug.WriteLine(nameof(IdToUserNameConverter) + " Please fetch users first, before using this converter");
            return value;
        }

        if (value is not string str) return value;

        return users.FirstOrDefault(user => user.Username == str)?.Id ?? value;
    }
}