using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace c_sharp_dotnet_development_lab_3AI_project.utils;

public class ApiBadRequestResponse : ApiResponse
{
    internal ApiBadRequestResponse(ModelStateDictionary modelState)
        : base(HttpStatusCode.BadRequest)
    {
        if (modelState.IsValid)
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = modelState.SelectMany(x =>
                x.Value?.Errors ?? throw new InvalidOperationException(nameof(modelState) + " has null errors"))
            .Select(x => x.ErrorMessage).ToArray();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IEnumerable<string> Errors { get; }
}