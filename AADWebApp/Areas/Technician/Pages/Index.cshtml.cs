using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Technician.Pages
{
    [Authorize(Roles = "Technician, Admin")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}