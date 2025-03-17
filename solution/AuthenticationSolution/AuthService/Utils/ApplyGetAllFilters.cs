using BusinessObject.DTO;
using static Supabase.Postgrest.Constants;

namespace AuthService.Utils;

public class ApplyGetAllFilters
{
    public static dynamic ApplyProfileFilters(dynamic query, GetProfileRequestDTO request)
    {
        if (!string.IsNullOrEmpty(request.FullName))
            query = query.Filter("full_name", Operator.ILike, $"%{request.FullName}%");

        if (!string.IsNullOrEmpty(request.Role))
            query = query.Filter("role", Operator.Equals, request.Role);

        if (request.DateOfBirth.HasValue)
            query = query.Filter("date_of_birth", Operator.Equals, request.DateOfBirth.Value);

        return query;
    }
}
