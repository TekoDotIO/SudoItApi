using Microsoft.AspNetCore.Mvc;

namespace SudoItApi.Controllers
{
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class PluginsController : Controller
    {
        public ActionResult<string> Get()
        {
            return "";
        }
    }
}
