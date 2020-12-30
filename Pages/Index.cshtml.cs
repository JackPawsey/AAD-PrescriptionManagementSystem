using AADWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public SendSMSService SendSMSService;

        public IndexModel(ILogger<IndexModel> logger, SendSMSService SendSMSService)
        {
            _logger = logger;
            this.SendSMSService = SendSMSService;
        }

        public void OnGet()
        {
            SendSMSService.SendSMS("+447873724880", "Hello from Jack WITH SUBJECT 2 at " + DateTime.Now.ToShortTimeString());
        }
    }
}
