using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Pharmacist.Pages
{
    [Authorize(Roles = "Pharmacist, Admin")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}