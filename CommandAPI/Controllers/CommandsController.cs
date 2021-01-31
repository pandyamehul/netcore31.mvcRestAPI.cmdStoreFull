using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
             return new string[] {
                  "Hard",
                  "Coaded",
                  "Api",
                  "Response"
             };
        }

        //Alternate way to write above code as below
     //    [HttpGet]
     //    public IEnumerable Get() => new string[] {"this", "is", "hard", "coded", "response"};
    }
}