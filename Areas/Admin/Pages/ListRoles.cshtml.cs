using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Admin.Pages
{
    public class ListRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> RoleManager;

        public ListRolesModel(RoleManager<IdentityRole> roleManager)
        {
            RoleManager = roleManager;
        }

        public void OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }
        }
    }
}
