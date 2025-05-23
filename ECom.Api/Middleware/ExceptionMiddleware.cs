using ECom.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace ECom.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment env;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);


        public ExceptionMiddleware(RequestDelegate next , IHostingEnvironment env , IMemoryCache memoryCache)
        {
            _next = next;
            this.env = env;
            _memoryCache = memoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                ApplySecurity(context);
                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new ApiException((int)HttpStatusCode.TooManyRequests,
                        "Too many Request , Please Try again Later");
                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = env.IsDevelopment() ? new ApiException(500,ex.Message,ex.StackTrace) 
                    : new ApiException(500, ex.Message);
                var Options = new JsonSerializerOptions() {
                    PropertyNamingPolicy =  JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response , Options);
                await context.Response.WriteAsync(json);

            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress;
            var cachkey = $"Rare:{ip}";
            var dateNow = DateTime.Now;

            var (timesTamp, count) = _memoryCache.GetOrCreate(cachkey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timesTamp: dateNow, count : 0);
            });
            if (dateNow - timesTamp < _rateLimitWindow)
            {
                if (count>8)
                {
                    return false;
                }
                _memoryCache.Set(cachkey, (timesTamp, count += 1) , _rateLimitWindow);
            }
            _memoryCache.Set(cachkey, (timesTamp, count ), _rateLimitWindow);
            return true;

        }

        private void ApplySecurity (HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";


        }
    }
}
