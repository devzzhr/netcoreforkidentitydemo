using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace DemoApp.Pages
{
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public string? AuthentMsg { get; set; }
        public string? statuscodeMsg { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string? code = null, int statuscode = 0)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            if (code != null)
            {
                if(code.StartsWith("M.C105_SN1.2."))
                AuthentMsg = "Une erreur a été observée lors de l'authentification";
                if (code.StartsWith("M.C105_BL2.2.") )
                {
                    AuthentMsg = "Une erreur a été observée lors de l'authentification auprés du service externe.";
                }

            }
            if (statuscode != 0)
            {
                statuscodeMsg = "Erreur "+statuscode.ToString();
               

            }
        }

    }
}