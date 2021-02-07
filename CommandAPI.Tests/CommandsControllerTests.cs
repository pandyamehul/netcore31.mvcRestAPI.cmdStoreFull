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

          [Fact]
          public void GetCommandItemReturnsNullResultWhenInvalidID()
          {
               //Action 2 Test : /api/commands/{Id} : Read : Read a single resource, (by Id)
               //Test# 2.1 : Condition = Resource ID is invalid (Does not exist in DB) : Result = Null Object Value Result

               //Arrange
               //DB should be empty, any ID will be invalid

               //Act
               var result = _controller.GetCommandItem(0);
               //Assert
               Assert.Null(result.Value);
          }

          [Fact]
          public void GetCommandItemReturns404NotFoundWhenInvalidID()
          {
               //Action 2 Test : /api/commands/{Id} : Read : Read a single resource, (by Id)
               //Test# 2.2 : Condition = Resource ID is invalid (Does not exist in DB) : Result = 404 Not Found Return Code

               //Arrange
               //DB should be empty, any ID will be invalid

               //Act
               var result = _controller.GetCommandItem(0);
               //Assert
               Assert.IsType<NotFoundResult>(result.Result);               
          }
          [Fact]
          public void GetCommandItemReturnsTheCorrectType()
          {
               //Action 2 Test : /api/commands/{Id} : Read : Read a single resource, (by Id)
               //Test# 2.3 : Condition = Resource ID is valid (Exists in the DB) : Result = Correct Return Type

               //Arrange
               var command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               _dbContext.CommandItems.Add(command);
               _dbContext.SaveChanges();
               int cmdId = command.Id;
               //Act
               var result = _controller.GetCommandItem(cmdId);
               //Assert
               Assert.IsType<ActionResult<Command>>(result);
          }
          [Fact]
          public void GetCommandItemReturnsTheCorrectResouce()
          {
               //Action 2 Test : /api/commands/{Id} : Read : Read a single resource, (by Id)
               //Test# 2.4 : Condition = Resource ID is valid (Exists in the DB) : Result = Correct Resource Returned

               //Arrange
               var command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               _dbContext.CommandItems.Add(command);
               _dbContext.SaveChanges();
               int cmdId = command.Id;
               //Act
               var result = _controller.GetCommandItem(cmdId);
               //Assert
               Assert.Equal(cmdId, result.Value.Id);               
          }
          [Fact]
          public void PostCommandItemObjectCountIncrementWhenValidObject()
          {
               //Action 3 Test : POST /api/commands : Create a new resource
               //Test# 3.1 : Condition = Valid Object Submitted for Creation : Result = Object count increments by 1

               //Arrange
               var command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               var oldCount = _dbContext.CommandItems.Count();
               //Act
               var result = _controller.PostCommandItem(command);
               //Assert
               Assert.Equal(oldCount+1, _dbContext.CommandItems.Count());
          }
          [Fact]
          public void PostCommandItemReturns201CreatedWhenValidObject()
          {
               //Action 3 Test : POST /api/commands : Create a new resource
               //Test# 3.2 : Condition = Valid Object Submitted for Creation 201 : Result = Created Return Code

               //Arrange
               var command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               var oldCount = _dbContext.CommandItems.Count();
               //Act
               var result = _controller.PostCommandItem(command);
               //Assert
               Assert.IsType<CreatedAtActionResult>(result.Result);
          }
          [Fact]
          public void PutCommandItem_AttributeUpdated_WhenValidObject()
          {
               //Action 4 Test : PUT /api/commands/{Id} : Update Update a single resource, (by Id)
               //Test# 4.1 : Condition = Valid Object Submitted for Update : Result = Attribute is updated

               //Arrange
               var _command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               _dbContext.CommandItems.Add(_command);
               _dbContext.SaveChanges();
               int cmdId = _command.Id;
               _command.HowTo = "dotnet test - updated";

               //Act
               _controller.PutCommandItem(cmdId, _command);
               var result = _dbContext.CommandItems.Find(cmdId);
               //Assert
               Assert.Equal(_command.HowTo, result.HowTo);
          }
          [Fact]
          public void PutCommandItem_Returns204_WhenValidObject()
          {
               //Action 4 Test : PUT /api/commands/{Id} : Update Update a single resource, (by Id)
               //Test# 4.2 : Condition = Valid Object Submitted for Update 204 : Result = No Content Return Code

               //Arrange
               var _command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               _dbContext.CommandItems.Add(_command);
               _dbContext.SaveChanges();
               int cmdId = _command.Id;
               _command.HowTo = "dotnet test - updated";
               //Act
               var result = _controller.PutCommandItem(cmdId, _command);
               //Assert
               Assert.IsType<NoContentResult>(result);
          }
          [Fact]
          public void PutCommandItem_Returns400_WhenInvalidObject()
          {
               //Action 4 Test : PUT /api/commands/{Id} : Update Update a single resource, (by Id)
               //Test# 4.3 : Condition = Invalid Object Submitted for Update 400 : Result = Bad Request Return Code

               //Arrange
               var _command = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               _dbContext.CommandItems.Add(_command);
               _dbContext.SaveChanges();
               int cmdId = _command.Id + 1;
               _command.HowTo = "dotnet test - updated";
               //Act
               var result = _controller.PutCommandItem(cmdId, _command);
               //Assert
               Assert.IsType<BadRequestResult>(result);
          }
          [Fact]
          public void PutCommandItem_AttributeUnchanged_WhenInvalidObject()
          {
               //Action 4 Test : PUT /api/commands/{Id} : Update Update a single resource, (by Id)
               //Test# 4.4 : Condition = Invalid Object Submitted for Update : Result = Object remains unchanged

               //Arrange
               var _command1 = new Command {
                    HowTo = "Do unit test",
                    Platform = "xUNit Platform",
                    CommandLine = "dotnet test"
               };
               var _command2 = new Command {
                    HowTo = "Do unit test - UPDATED",
                    Platform = "xUNit Platform - UPDATED",
                    CommandLine = "dotnet test - UPDATED"
               };               
               _dbContext.CommandItems.Add(_command1);
               _dbContext.SaveChanges();
               //Act
               _controller.PutCommandItem(_command1.Id + 1, _command2);
               var result = _dbContext.CommandItems.Find(_command1.Id);
               //Assert
               Assert.Equal(_command1.HowTo, result.HowTo);
          }                   
     }
}