using System.Diagnostics;
using System.Globalization;

namespace desktopapplication.Converters;

public class InvertBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // ReSharper disable once InvertIf
        if (value is not bool boolean)
        {
            Debug.WriteLine("Please only use this converter with boolean values");
            return false;
        }

        return !boolean;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert(value, targetType, parameter, culture);
}