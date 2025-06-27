using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class GetByIdAsynctTests : RepositoryTestBase
{
    private readonly CollaboratorRepositoryEF _repository;

    public GetByIdAsynctTests()
    {
        _repository = new CollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedCollaborator_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var collabDM = new CollaboratorDataModel { Id = id };

        await context.Set<CollaboratorDataModel>().AddAsync(collabDM);
        await context.SaveChangesAsync();

        var mockCollab = new Mock<ICollaborator>();
        mockCollab.SetupGet(c => c.Id).Returns(id);

        _mapper.Setup(m => m.Map<CollaboratorDataModel, ICollaborator>(collabDM))
               .Returns(mockCollab.Object);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result?.Id);
        _mapper.Verify(m => m.Map<CollaboratorDataModel, ICollaborator>(collabDM), Times.Once);
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
        _mapper.Verify(m => m.Map<CollaboratorDataModel, ICollaborator>(It.IsAny<CollaboratorDataModel>()), Times.Never);
    }
}

