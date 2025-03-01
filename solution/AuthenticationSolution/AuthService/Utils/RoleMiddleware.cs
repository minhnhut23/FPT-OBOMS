using BusinessObject.Models;

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
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (userId != null)
        {
            var role = await GetUserRole(Guid.Parse(userId));
            if (role != null)
            {
                context.Items["UserRole"] = role;
            }
        }

        await _next(context);
    }

    private async Task<string?> GetUserRole(Guid userId)
    {
        var response = await _client
            .From<Profile>()
            .Where(x => x.AccountId == userId)
            .Single();

        return response?.Role;
    }

}
