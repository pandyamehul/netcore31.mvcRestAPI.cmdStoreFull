using System;
using Xunit;

using Microsoft.EntityFrameworkCore;
using CommandAPI.Controllers;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
     public class CommandsControllerTests
     {
          [Fact]
          public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
          {
               //Test 1.1 : Condition = Request Object when none exists : Expected Result = Return nothing
               //Arrange 
               //DBContext
               var optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
               optionsBuilder.UseInMemoryDatabase("UnitTEstInMemoryDB");
               var dbContext = new CommandContext(optionsBuilder.Options);

               //Controller
               var controller = new CommandsController(dbContext);
               //ACT
               var result = controller.GetCommandItems();
               //Assert
               Assert.Empty(result.Value);
          }
     }
}