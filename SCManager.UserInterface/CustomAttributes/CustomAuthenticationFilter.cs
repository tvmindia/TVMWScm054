using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;

namespace SCManager.UserInterface.CustomAttributes
{
    public class CustomAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            //SessionCheck
            if ((context.HttpContext.Session == null) || (context.HttpContext.Session["TvmValid"] == null))
            {
                context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "Index" } });
                return;
            }
            ////
            var authCookie = context.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                // Unauthorized
                context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "Index" } });
                return;
            }

            // Get the forms authentication ticket.
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //  object usercookie = JsonConvert.DeserializeObject(authTicket.UserData); // Up to you to write this Deserialize method -> it should be the reverse of what you did in your Login action
            if (authTicket == null)
            {
                context.Result = new HttpUnauthorizedResult(); // mark unauthorized
            }
            else
            {

                context.HttpContext.User = new System.Security.Principal.GenericPrincipal(
                new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms"), authTicket.UserData.Split(',').Select(t => t.Trim()).ToArray());
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null || context.Result is HttpUnauthorizedResult)
            {

                context.Result = new RedirectToRouteResult("Default",
                    new System.Web.Routing.RouteValueDictionary{
                        {"controller", "Account"},
                        {"action", "NotAuthorized"}
                        //{"returnUrl", context.HttpContext.Request.RawUrl}
                    });
            }
        }
    }

    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}