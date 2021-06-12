using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommandAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CommandAPI.Controllers
{
    //Command API Controller Class
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
         private readonly CommandContext _context;
         public CommandsController(CommandContext commandContext) => _context = commandContext;

         //GET api/commands
         [Authorize]
         [HttpGet]
         public ActionResult<IEnumerable<Command>> GetCommandItems()
         {
              return _context.CommandItems;
         }
         //GET api/commands/{id}
         [Authorize]
         [HttpGet("{id}")]
         public ActionResult<Command> GetCommandItem(int id)
         {
             Command cmditem = _context.CommandItems.Find(id);
             if (cmditem == null)
             {
                 return NotFound();
             }
             return cmditem;
         }
         //POST: api/commands
         [Authorize]
         [HttpPost]
         public ActionResult<Command> PostCommandItem(Command cmd)
         {
             _context.CommandItems.Add(cmd);
             try
             {
                 _context.SaveChanges();
             }
             catch
             {
                 return BadRequest();
             }
             return CreatedAtAction("GetCommandItem", new Command { Id = cmd.Id}, cmd);
         }
         //PUT: api/commands/{Id}
         [Authorize]
         [HttpPut("{id}")]
         public ActionResult PutCommandItem(int id, Command _command)
         {
             if(id != _command.Id)
             {
                 return BadRequest();
             }
             _context.Entry(_command).State = EntityState.Modified;
             _context.SaveChanges();
             return NoContent();
         }
         //DELETE: api/commands/{Id}
         [Authorize]
         [HttpDelete("{id}")]
         public ActionResult<Command> DeleteCommandItem(int id)
         {
             var _commandItem  = _context.CommandItems.Find(id);
             if(_commandItem == null)
             {
                 return NotFound();
             }
             _context.CommandItems.Remove(_commandItem);
             _context.SaveChanges();
             return _commandItem;
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