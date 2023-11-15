using MySqlConnector;

namespace c_sharp_dotnet_development_lab_3AI_project.Services.MySql;

public static class CustomMySqlHelper
{
    public static bool IsMySqlException(Exception exception, MySqlExceptionType? exceptionType = null)
    {
        if (exception is MySqlException sqlException)
            return exceptionType == null || sqlException.Number == (int)exceptionType;

        return exception.InnerException != null && IsMySqlException(exception.InnerException, exceptionType);
    }
}