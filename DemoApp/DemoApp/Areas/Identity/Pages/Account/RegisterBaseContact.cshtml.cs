using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Even.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Even.Models;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace Even.Areas.Identity.Pages.Account
{
    public class InputModel
    {
        

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class RegisterBaseContactModel : PageModel
    {
        private readonly BaseContactDBContext _bccontext;
        public RegisterBaseContactModel(
           BaseContactDBContext context)
        {
           
            _bccontext = context;
        }
        [TempData]
        public string Message { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        //public string Message { get; private set; } = "PageModel in C#";

        public void OnGet()
        {
           // Message += $" Server time is {DateTime.Now}";
            if(TempData.Peek("MailForBaseContact") != null)
            {
                
                Input = new InputModel
                {
                    Email = TempData.Peek("MailForBaseContact").ToString()

            };
            }
        }
        public async Task<IActionResult> OnPostConfirmationAsync()
        {
            var bcontact = new BaseContact();
            bcontact.Email = Input.Email.ToString();
            bcontact.Creation =  DateTime.Now.ToUniversalTime();
            bcontact.Modification =  DateTime.Now.ToUniversalTime();
                  _bccontext.BaseContacts.Add(bcontact);
            await _bccontext.SaveChangesAsync();
            Message = "Votre adresse a bien été ajoutée à la base de contact.\n veuillez maintenant vous inscrire afin d'utiliser les services de cette application.";

             return  RedirectToPage("./ExternalLogin");
            // Request a redirect to the external login provider.


        }
    }
}
