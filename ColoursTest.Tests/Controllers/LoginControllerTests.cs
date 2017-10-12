using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ColoursTest.Tests.Controllers
{
    public class LoginControllerTests
    {
        [Fact]
        public void Login_InvalidLoginRequestModel_ReturnsBadRequest()
        {
            // Arange
            var loginController = new LoginController();

            // Act
            var result = loginController.Login(new LoginRequest());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_InvalidUsername_ReturnsBadRequest()
        {
            // Arange
            var loginController = new LoginController();

            // Act
            var result = loginController.Login(new LoginRequest {Username = "wrongusername", Password = "password"});

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsBadRequest()
        {
            // Arange
            var loginController = new LoginController();

            // Act
            var result = loginController.Login(new LoginRequest { Username = "username", Password = "wrongpassword" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_ValidDetails_ReturnsOkWithLoginSuccessObject()
        {
            // Arange
            var loginController = new LoginController();

            // Act
            var result = loginController.Login(new LoginRequest {Username = "username", Password = "password"});

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult) result;
            Assert.IsType<LoginSuccess>(okObjectResult.Value);
        }
    }
}