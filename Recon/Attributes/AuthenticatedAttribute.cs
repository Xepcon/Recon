using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Recon.Models;
using System;

namespace Recon.Attribute
{
    public class AuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;

            if (userService == null)
            {
                throw new ApplicationException("IUserService not found in the service container.");
            }

            if (!userService.IsAuthenticated())
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}