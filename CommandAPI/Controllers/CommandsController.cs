using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
         private readonly CommandContext _context;
         public CommandsController(CommandContext commandContext) => _context = commandContext;

         //GET api/commands
         [HttpGet]
         public ActionResult<IEnumerable<Command>> GetCommandItems()
         {
              return _context.CommandItems;
         }

         [HttpGet]
         public ActionResult<Command> GetCommandItem(int id)
         {
             Command cmditem = _context.CommandItems.Find(id);
             if (cmditem == null)
             {
                 return NotFound();
             }
             return cmditem;
         }

     //    [HttpGet]
     //    public ActionResult<IEnumerable<string>> Get()
     //    {
     //         return new string[] {
     //              "Hard",
     //              "Coaded",
     //              "Api",
     //              "Response"
     //         };
     //    }

        //Alternate way to write above code as below
     //    [HttpGet]
     //    public IEnumerable Get() => new string[] {"this", "is", "hard", "coded", "response"};
    }
}