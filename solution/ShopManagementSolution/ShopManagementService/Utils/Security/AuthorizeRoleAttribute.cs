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
        UserRole userRole = UserRole.Customer;

        if (context.HttpContext.Items["UserRole"] is UserRole role)
        {
            userRole = role;
        }

        if (userRole != _requiredRole)
        {
            context.Result = new ForbidResult();
        }
    }
}
