using BusinessObject.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ShopManagementService.Utils;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Supabase.Client _client;

    public RoleMiddleware(RequestDelegate next, Supabase.Client supabaseClient)
    {
        _next = next;
        _client = supabaseClient;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        string? userId = null;

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        if (!string.IsNullOrEmpty(userId))
        {
            var roleFromDB = await GetUserRole(Guid.Parse(userId));

            if (roleFromDB != null)
            {
                context.Items["UserRole"] = roleFromDB;
            }
        }

        await _next(context);
    }

    private async Task<Enum?> GetUserRole(Guid userId)
    {
        var response = await _client
            .From<Profile>()
            .Where(x => x.AccountId == userId)
            .Single();

        return response?.Role;
    }

}
