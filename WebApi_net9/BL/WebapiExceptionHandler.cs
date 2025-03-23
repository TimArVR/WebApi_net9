using Microsoft.AspNetCore.Diagnostics;

namespace WebApi_net9.BL
{
    //Обработчик исключений, когда исключение происходит, мы его вызываем
    public class WebapiExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService problemDetailsService;
        public WebapiExceptionHandler(IProblemDetailsService problemDetailsService) 
        {
            this.problemDetailsService = problemDetailsService;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            if (exception is TicketException e)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var problemDetails = new ProblemDetailsContext()
                {
                    HttpContext = httpContext,
                    ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = e.Title,
                    }
                };
                return await problemDetailsService.TryWriteAsync(problemDetails);
            }

            if (exception is TicketNotFoundException nfe)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                var problemDetails = new ProblemDetailsContext()
                {
                    HttpContext = httpContext,
                    ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = nfe.Title,
                    }
                };
                return await problemDetailsService.TryWriteAsync(problemDetails);
            }

            return true;
        }
    }
}
