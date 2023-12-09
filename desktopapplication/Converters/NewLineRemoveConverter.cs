using System.Diagnostics;
using System.Globalization;

namespace desktopapplication.Converters;

public class NewLineRemoveConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value?.ToString()?.Replace("\n", " ");

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Debug.WriteLine("NewLineRemoveConverter.ConvertBack called but not implemented");
        return null;
    }
}