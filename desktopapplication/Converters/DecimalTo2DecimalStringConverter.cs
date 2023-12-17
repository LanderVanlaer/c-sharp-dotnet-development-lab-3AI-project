using System.Globalization;

namespace desktopapplication.Converters;

public class DecimalTo2DecimalStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            decimal d => d.ToString("0.00"),
            _ => throw new ArgumentException("Value is not a decimal", nameof(value)),
        };

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            string s => decimal.Parse(s),
            _ => throw new ArgumentException("Value is not a string", nameof(value)),
        };
}