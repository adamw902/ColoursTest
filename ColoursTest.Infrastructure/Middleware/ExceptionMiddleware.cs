using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ColoursTest.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        public ExceptionMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        private RequestDelegate Next { get; }

        public async Task Invoke(HttpContext context)
        {
            // no request stuff so do next.
            await this.Next.Invoke(context);

            // response stuff
            switch (context.Response.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    var header = $"{context.Response.Headers["WWW-Authenticate"]}";
                    context.Response.ContentType = "application/json";
                    string message;

                    if (header == "Bearer")
                    {
                        message = "Authorization header is missing.";
                    }
                    else
                    {
                        var results = header.Trim('{', '}')
                            .Split(',')
                            .Select(s => s.Trim().Split('='))
                            .ToDictionary(a => a[0], a => a[1]);

                        var messages = results.Select(r => r.Value);
                        message = string.Join(", ", messages).Replace('_', ' ').Replace("\"", "");
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { State = 401, Msg = message }));
                    break;

                case (int)HttpStatusCode.InternalServerError:
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { State = 500, Msg = "Server Error: The system administrator has been notified." }));
                    break;
            }
        }
    }
}