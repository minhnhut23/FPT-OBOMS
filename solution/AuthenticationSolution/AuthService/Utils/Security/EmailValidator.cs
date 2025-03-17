using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AuthService.Utils.Security;

public class EmailValidator
{
    private static readonly Regex _emailRegex = new Regex(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return _emailRegex.IsMatch(email);
    }

    public static async Task<bool> IsEmailExists(string email)
    {
        var url = $"https://your-supabase-url.supabase.co/auth/v1/admin/users?email={email}";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNuYndud2J0YWZiYXJzZ21jYWJmIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTczMTQwMzc2MSwiZXhwIjoyMDQ2OTc5NzYxfQ.wmXv1VfLmGBVo-Mx_xLzVvov5NFgKZ-x3S3CoptK0O4");

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode) return false;

        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody.Contains(email);
    }
}
