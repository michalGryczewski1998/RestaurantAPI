
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ForbidException fe)
            {
                context.Response.StatusCode = 403;
            }
            catch(BadRequestException br)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(br.Message);
            }
            catch (NotFoundException nfEx)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nfEx.Message);
            }
            catch (NotCreateAccountException ncEx)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ncEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Coś poszło nie tak !");
            }
        }
    }
}
