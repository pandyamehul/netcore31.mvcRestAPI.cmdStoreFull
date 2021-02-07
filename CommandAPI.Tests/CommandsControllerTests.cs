using System;
using Xunit;

using Microsoft.EntityFrameworkCore;
using CommandAPI.Controllers;
using CommandAPI.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Tests
{
     public class CommandsControllerTests : IDisposable
     {
          DbContextOptionsBuilder<CommandContext> _optionsBuilder;
          CommandContext _dbContext;
          CommandsController _controller;

          public void Dispose()
          {
               _optionsBuilder = null;
               foreach (var cmditem in _dbContext.CommandItems)
               {
                   _dbContext.CommandItems.Remove(cmditem);
               }
               _dbContext.SaveChanges();
               _dbContext.Dispose();
               _controller = null;
          }

          public CommandsControllerTests()
          {
               //Arrange 
               //DBContext
               _optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
               _optionsBuilder.UseInMemoryDatabase("UnitTEstInMemoryDB");
               _dbContext = new CommandContext(_optionsBuilder.Options);
               //Controller
               _controller = new CommandsController(_dbContext);
          }

          [Fact]
          public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
          {
               //Action 1 Test : GET /api/commands
               //Test 1.1 : Condition = Request Object when none exists : Expected Result = Return nothing
               //TEST 1.1 REQUEST OBJECTS WHEN NONE EXIST – RETURN "NOTHING"
               //ACT
               var result = _controller.GetCommandItems();
               //Assert
               Assert.Empty(result.Value);
          }
          [Fact]
          public void GetCommandItemsReturnsOneItemWhenDBHasOneObject()
          {
               //Action 1 Test : GET /api/commands
               //Test# 1.2 : Condition = Request Objects when 1 exists : Expected Result = Return Single Object
               //Arrange
               var command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };

               _dbContext.CommandItems.Add(command);
               _dbContext.SaveChanges();

               //Act
               var result = _controller.GetCommandItems();
               //Assert
               Assert.Single(result.Value);
          }
          [Fact]
          public void GetCommandItemsReturnNItemsWhenDBHasNObjects()
          {
               //Action 1 Test : GET /api/commands
               //Test# 1.3 : Condition = Request Objects when N exists : Expected Result = Return Count of n Objects
               //Arrange
               var command1 = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };

               var command2 = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };

               _dbContext.CommandItems.Add(command1);
               _dbContext.CommandItems.Add(command2);
               _dbContext.SaveChanges();

               //Act
               var result = _controller.GetCommandItems();
               //Assert
               Assert.Equal(2, result.Value.Count());
          }
          [Fact]
          public void GetCommandItemsReturnsTheCorrectType()
          {
               //Action 1 Test : GET /api/commands
               //Test# 1.4 : Condition = Request Objects : Expected Result = Return the correct “type”               
               //Act
               var result = _controller.GetCommandItems();
               //Assert
               Assert.IsType<ActionResult<IEnumerable<Command>>>(result);
          }
     }
}