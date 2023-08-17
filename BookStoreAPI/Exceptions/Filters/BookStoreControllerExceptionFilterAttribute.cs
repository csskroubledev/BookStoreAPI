using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStoreAPI.Exceptions.Filters;

public class BookStoreControllerExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ApiException apiException)
            context.Result = new ObjectResult(apiException.Content)
            {
                StatusCode = apiException.StatusCode
            };
        else
            context.Result = new ObjectResult($"An error occured, message: {context.Exception.Message}")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

        context.ExceptionHandled = true;
    }
}