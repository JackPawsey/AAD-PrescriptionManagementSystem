using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Patient.Pages
{
    [Authorize(Roles = "Patient, Authorised Carer, Admin")]
    public class SetCommPreferenceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}