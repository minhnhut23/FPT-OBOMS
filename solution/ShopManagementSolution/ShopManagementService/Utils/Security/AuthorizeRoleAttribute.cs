using BusinessObject.Enums;
using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopManagementService.Utils.Security;

public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserRole _requiredRole;

    public AuthorizeRoleAttribute(UserRole requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["UserRole"] is not UserRole userRole || userRole != _requiredRole)
        {
            context.Result = new ForbidResult();
        }
    }
}
