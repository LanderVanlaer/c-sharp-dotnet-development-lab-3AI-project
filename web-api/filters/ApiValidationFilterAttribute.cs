using c_sharp_dotnet_development_lab_3AI_project.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace c_sharp_dotnet_development_lab_3AI_project.filters;

/// <summary>
///     https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api#:~:text=same%20general%20structure.-,Centralising%20Validation%20Logic,-Given%20that%20validation
///     https://github.com/aspnet/Mvc/blob/master/src/Microsoft.AspNetCore.Mvc.Core/Infrastructure/ModelStateInvalidFilter.cs
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class ApiValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState));

        base.OnActionExecuting(context);
    }
}