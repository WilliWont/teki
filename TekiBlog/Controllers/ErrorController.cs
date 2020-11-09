
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TekiBlog.ViewModels;

namespace TekiBlog.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{code:int}")]
        public IActionResult Index(int? code)
        {
            ErrorViewModel error = null;
            if (code != null)
            {
                error = new ErrorViewModel { 
                    StatusCode = (int) code,
                    StatusDescription = ReasonPhrases.GetReasonPhrase( (int) code)
                };
            }

            return View(error);
        }
    }
}
