using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CajaMoreliaApi.Models;
using CajaMoreliaApi.Database;
using Microsoft.EntityFrameworkCore;

namespace CajaMoreliaApi.Middleware
{
    public class Credentials
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "Authorization";

        public Credentials(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ConnectorDatabase dbContext)
        {
            try
            {
                if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var authHeader))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API Key no proporcionada.");
                    return;
                }

                var token = authHeader.ToString().Replace("Bearer", "");

                var apiKey = await dbContext.ApiKeys
                    .Where(t => t.Token.ToString() == token.Trim())
                    .FirstAsync();

                if (apiKey == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API Key inválida o inactiva.");
                    return;
                }

                await _next(context);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key inválida o inactiva.");
                return;
            }
        }
    }
}