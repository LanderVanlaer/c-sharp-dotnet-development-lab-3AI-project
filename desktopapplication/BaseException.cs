using System.Diagnostics;

namespace desktopapplication;

public class BaseException : Exception
{
    public BaseException(string message, Exception? innerException = null) : base(message, innerException)
    {
        Debug.WriteLine("Exception " + GetType().Name + ": " + message);
    }
}