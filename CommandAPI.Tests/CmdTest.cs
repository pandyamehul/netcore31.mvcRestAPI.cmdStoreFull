using System;
using Xunit;
using CommandAPI.Models;
namespace CommandAPI.Tests
{
     public class CmdTest : IDisposable
     {
          //Arrange
          Command testCommand;

          public CmdTest()
          {
               testCommand = new Command {
                    HowTo = "Do automated unit testing",
                    Platform = "xUnit",
                    CommandLine = "dotnet test"
               };
          }          
          [Fact]
          public void CanChangeHowTo()
          {
               //Act
               testCommand.HowTo = "Execute automated unit test";
               //Assert
               Assert.Equal("Execute automated unit test", testCommand.HowTo);
          }
          [Fact]
          public void CanChangePlatform()
          {
               //Act
               testCommand.Platform = "xUnit";
               //Assert
               Assert.Equal("xUnit", testCommand.Platform);
          }
          [Fact]
          public void CanChangeCommandLine()
          {
               //Act
               testCommand.CommandLine = "dotnet test";
               //Assert
               Assert.Equal("dotnet test", testCommand.CommandLine);
          }
          public void Dispose()
          {
               testCommand = null;
          }
     }
}