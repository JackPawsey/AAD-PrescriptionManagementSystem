using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Patient.Pages
{
    [Authorize(Roles = "Patient, Authorised Carer, Admin")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
