using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAR.Presentation.Controllers;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class BaseControllerTests
{
    private readonly Mock<ILoggerAdapter<TestModel>> _mockLogger;

    private readonly Mock<IBaseService<TestModel>> _mockService;

    private readonly BaseController<TestModel> _controller;

    public BaseControllerTests()
    {
        _mockLogger = new Mock<ILoggerAdapter<TestModel>>();
        _mockService = new Mock<IBaseService<TestModel>>();
        _controller = new BaseController<TestModel>(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenDataExists()
    {
        // Arrange
        var testData = new List<TestModel> { new() { Id = 1, Name = "testuser1" }, new() { Id = 2, Name = "testuser2" } };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(testData);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<List<TestModel>>(actionResult.Value);
        Assert.Equal(2, returnedData.Count);
    }

    [Fact]
    public async Task GetAll_ReturnsNotFound_WhenTheresNoData()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<TestModel>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEntityExists()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "testuser" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(testEntity);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<TestModel>(actionResult.Value);
        Assert.Equal(1, returnedData.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenEntityDoesntExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(100)).ReturnsAsync((TestModel?)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetByField_ReturnsOk_WhenEntityExists()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "testuser" };
        _mockService.Setup(s => s.GetByFieldAsync("Name", "testuser")).ReturnsAsync(testEntity);

        // Act
        var result = await _controller.GetByField("Name", "testuser");

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<TestModel>(actionResult.Value);
        Assert.Equal("testuser", returnedData.Name);
    }

    [Fact]
    public async Task GetByField_ReturnsNotFound_WhenEntityDoesntExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByFieldAsync("Name", "nonexistent")).ReturnsAsync((TestModel?)null);

        // Act
        var result = await _controller.GetByField("Name", "nonexistent");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Add_ReturnsCreatedAction_WhenAddedSuccesfully()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "testuser" };
        _mockService.Setup(s => s.AddAsync(testEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Add(testEntity);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedData = Assert.IsType<TestModel>(actionResult.Value);
        Assert.Equal(testEntity.Id, returnedData.Id);
        Assert.Equal(testEntity.Name, returnedData.Name);
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenEntityIsNull()
    {
        // Arrange
        TestModel? testEntity = null;

        // Act
        var result = await _controller.Add(testEntity!);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Entity cannot be null.", actionResult.Value);
    }

    [Fact]
    public async Task Add_ReturnsStatusCode500_WhenCatchedException()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "testuser" };
        _mockService.Setup(s => s.AddAsync(testEntity)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Add(testEntity);

        // Assert
        var actionResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", actionResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdatedSuccesfully()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "updateduser" };
        _mockService.Setup(s => s.GetByIdAsync(testEntity.Id)).ReturnsAsync(new TestModel { Id = 1, Name = "testuser" });
        _mockService.Setup(s => s.UpdateAsync(testEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(testEntity);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenEnityIsNull()
    {
        // Arrange
        TestModel? testEntity = null;

        // Act
        var result = await _controller.Update(testEntity!);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Entity must not be null and have a valid Id.", actionResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEntityDoesntExist()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "nonexistentuser" };
        _mockService.Setup(s => s.GetByIdAsync(testEntity.Id)).ReturnsAsync((TestModel?)null);

        // Act
        var result = await _controller.Update(testEntity);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsStatusCode500_WhenCatchedException()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "testuser" };
        _mockService.Setup(s => s.GetByIdAsync(testEntity.Id)).ReturnsAsync(new TestModel { Id = 1, Name = "testuser" });
        _mockService.Setup(s => s.UpdateAsync(testEntity)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Update(testEntity);

        // Assert
        var actionResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", actionResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeletedSuccesfully()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "testuser" };
        _mockService.Setup(s => s.GetByIdAsync(testEntity.Id)).ReturnsAsync(testEntity);
        _mockService.Setup(s => s.DeleteAsync(testEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(testEntity);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenEntityIsNull()
    {
        // Arrange
        TestModel? testEntity = null;

        // Act
        var result = await _controller.Delete(testEntity!);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Entity must not be null and have a valid Id.", actionResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEntityDoesntExist()
    {
        // Arrange
        var testEntity = new TestModel { Id = 1, Name = "nonexistentuser" };
        _mockService.Setup(s => s.GetByIdAsync(testEntity.Id)).ReturnsAsync((TestModel?)null);

        // Act
        var result = await _controller.Delete(testEntity);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

