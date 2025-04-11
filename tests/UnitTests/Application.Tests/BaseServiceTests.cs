using Xunit;
using Moq;
using MAR.Application.Services;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class BaseServiceTests
{
    private readonly Mock<IBaseRepository<TestModel>> _mockRepository;

    private readonly Mock<ILoggerAdapter<TestModel>> _mockLogger;

    private readonly BaseService<TestModel> _baseService;

    public BaseServiceTests()
    {
        _mockRepository = new Mock<IBaseRepository<TestModel>>();
        _mockLogger = new Mock<ILoggerAdapter<TestModel>>();
        _baseService = new BaseService<TestModel>(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEntities_WhenEntitiesExist()
    {
        // Arrange
        var expectedEntities = new List<TestModel>
        {
            new TestModel { Id = 1, Name = "testmodel1" },
            new TestModel { Id = 2, Name = "testmodel2" },
            new TestModel { Id = 3, Name = "testmodel3" }
        };

        _mockRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(expectedEntities);

        // Act
        var result = await _baseService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntities.Count(), result.Count());
        Assert.Equal(expectedEntities.First().Id, result.First().Id);
        Assert.Equal(expectedEntities.Last().Name, result.Last().Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyCollectionOfEntities_WhenTheresNoEntities()
    {
        // Arrange
        _mockRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(Enumerable.Empty<TestModel>());

        // Act
        var result = await _baseService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntity_WhenEntityExists()
    {
        //Arrange
        int entityId = 1;
        var expectedEntity = new TestModel { Id = entityId, Name = "testmodel"};
        _mockRepository.Setup(repository => repository.GetByIdAsync(entityId)).ReturnsAsync(expectedEntity);

        // Act
        var result = await _baseService.GetByIdAsync(entityId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenEntityDoesntExist()
    {
        // Arrange
        int entityId = 1;
        _mockRepository.Setup(repository => repository.GetByIdAsync(entityId)).ReturnsAsync((TestModel?)null);

        // Act
        var result = await _baseService.GetByIdAsync(entityId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByFieldAsync_ReturnsEntity_WhenEntityExists()
    {
        // Arrange
        var fieldName = "Name";
        var fieldValue = "testname";
        var expectedEntity = new TestModel { Id = 1, Name = "testname" };

        _mockRepository.Setup(repository => repository.GetByFieldAsync(fieldName, fieldValue)).ReturnsAsync(expectedEntity);

        // Act
        var result = await _baseService.GetByFieldAsync(fieldName, fieldValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Name, result.Name);
    }

    [Fact]
    public async Task GetByFieldAsync_ReturnsNull_WhenEnityDoesntExists()
    {
        // Arrange
        var fieldName = "Name";
        var fieldValue = "testname";
        var expectedEntity = new TestModel { Id = 1, Name = "testname" };

        _mockRepository.Setup(repository => repository.GetByFieldAsync(fieldName, fieldValue)).ReturnsAsync(expectedEntity);

        // Act
        var result = await _baseService.GetByFieldAsync(fieldName, "differentFieldValue");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByFieldAsync_ThrowsException_WhenFieldNameIsInvalid()
    {
        // Arrange
        var invalidFieldName = "InvalidField";
        var fieldValue = "testValue";
        
        _mockRepository.Setup(repository => repository.GetByFieldAsync(invalidFieldName, fieldValue)).ThrowsAsync(new ArgumentException("Invalid field name"));

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => 
                await _baseService.GetByFieldAsync(invalidFieldName, fieldValue)
            );

        // Assert
        Assert.Equal("Invalid field name", exception.Message);
    }

    [Fact]
    public async Task AddAsync_AddsEntity_WhenEntityIsValid()
    {
        // Arrange
        var newEntity = new TestModel { Id = 1, Name = "testname" };

        _mockRepository.Setup(repository => repository.AddAsync(newEntity)).Returns(Task.CompletedTask);

        // Act
        await _baseService.AddAsync(newEntity);

        // Assert
        _mockRepository.Verify(repository => repository.AddAsync(newEntity), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ThrowsException_WhenEntityIsNull()
    {
        // Arrange
        TestModel? nullEntity = null;

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _baseService.AddAsync(nullEntity!)
            );

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'entity')", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntity_WhenEntityIsValid()
    {
        // Arrange
        var existingEntity = new TestModel { Id = 1, Name = "existingName" };
        var updatedEntity = new TestModel { Id = 1, Name = "updatedName" };

        _mockRepository.Setup(repository => repository.UpdateAsync(updatedEntity)).Returns(Task.CompletedTask);

        // Act
        await _baseService.UpdateAsync(existingEntity);

        // Assert
        _mockRepository.Verify(repository => repository.UpdateAsync(existingEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenEntityIsNull()
    {
        // Arrange
        TestModel? nullEntity = null;

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _baseService.UpdateAsync(nullEntity!)
            );

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'entity')", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_DeletesEntity_WhenEntityIsValid()
    {
        // Arrange
        var entityToDelete = new TestModel { Id = 1, Name = "testname" };

        _mockRepository.Setup(repository => repository.DeleteAsync(entityToDelete)).Returns(Task.CompletedTask);

        // Act
        await _baseService.DeleteAsync(entityToDelete);

        // Assert
        _mockRepository.Verify(repository => repository.DeleteAsync(entityToDelete), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenEntityIsNull()
    {
        // Arrange
        TestModel? nullEntity = null;

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _baseService.DeleteAsync(nullEntity!)
            );

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'entity')", exception.Message);
    }
}