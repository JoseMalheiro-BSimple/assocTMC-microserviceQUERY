using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;
public class GetByIdAsyncTests : RepositoryTestBase
{
    private readonly TrainingModuleRepositoryEF _repository;

    public GetByIdAsyncTests()
    {
        _repository = new TrainingModuleRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedTrainingModule_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tmDM = new TrainingModuleDataModel { Id = id };

        await context.Set<TrainingModuleDataModel>().AddAsync(tmDM);
        await context.SaveChangesAsync();

        var domainMock = new Mock<ITrainingModule>();
        domainMock.SetupGet(m => m.Id).Returns(id);

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, ITrainingModule>(tmDM))
               .Returns(domainMock.Object);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result?.Id);
        _mapper.Verify(m => m.Map<TrainingModuleDataModel, ITrainingModule>(tmDM), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
        _mapper.Verify(m => m.Map<TrainingModuleDataModel, ITrainingModule>(It.IsAny<TrainingModuleDataModel>()), Times.Never);
    }
}
