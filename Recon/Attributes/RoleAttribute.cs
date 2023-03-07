using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Recon.Models;

public class CustomRoleAttribute : TypeFilterAttribute
{
    public CustomRoleAttribute(params string[] roles) : base(typeof(CustomRoleFilter))
    {
        Arguments = new object[] { roles };
    }
}

public class CustomRoleFilter : IAuthorizationFilter
{
    private readonly string[] _roles;
   

    public CustomRoleFilter(string[] roles )
    {
        _roles = roles;
        
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Get the current user from the database
        var userService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;

        if (userService == null)
        {
            throw new ApplicationException("IUserService not found in the service container.");
        }

        if (!userService.IsAuthenticated())
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }

        if (context.HttpContext.Session.GetString("UserId") != null)
        {
            var user = userService.GetById(int.Parse(context.HttpContext.Session.GetString("UserId")));

            var isInRole = false;
            List<Roles> UserRoles = userService.GetRolesForUser(user.Id);
            foreach (var role in _roles)
            {
                if (UserRoles.Any(r => r.Name == role))
                {
                    isInRole = true;
                    break;
                }
            }

            if (!isInRole)
            {

                context.Result = new ViewResult { ViewName = "AccessDenied" };
            }
        }
    }
}