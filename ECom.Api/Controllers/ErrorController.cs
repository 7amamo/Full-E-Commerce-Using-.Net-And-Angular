using ECom.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.Api.Controllers
{
    [Route("errors/{statesCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error (int statesCode)
        {
            return new ObjectResult(new ResponseApi(statesCode));
        }
    }
}
