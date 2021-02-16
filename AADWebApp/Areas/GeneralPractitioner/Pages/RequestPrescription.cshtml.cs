using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.GeneralPractitioner.Pages
{
    [Authorize(Roles = "General Practitioner, Admin")]
    public class RequestPrescriptionModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}