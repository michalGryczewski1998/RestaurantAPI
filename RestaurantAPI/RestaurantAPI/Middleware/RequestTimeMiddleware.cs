
using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private Stopwatch _stopwatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger, IConfiguration configuration)
        {
            _stopwatch = new();
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();

            await next.Invoke(context);

            _stopwatch.Stop();

            var time = _stopwatch.ElapsedMilliseconds;

            int timeOut = int.Parse(_configuration["TimeOutSettings:Default"]);

            if (time / 1000 > timeOut)
            {
                var msg = 
                    $"Request [{context.Request.Method}] za ścieżką [{context.Request.Path}] zajął {time} sekund";

                _logger.LogInformation(msg);
            }
        }
    }
}
