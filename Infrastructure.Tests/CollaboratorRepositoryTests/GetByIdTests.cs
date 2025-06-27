using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;
public class GetByIdTests : RepositoryTestBase
{
    private readonly CollaboratorRepositoryEF _repository;

    public GetByIdTests()
    {
        _repository = new CollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public void GetById_ReturnsMappedCollaborator_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var collabDM = new CollaboratorDataModel { Id = id };

        context.Set<CollaboratorDataModel>().Add(collabDM);
        context.SaveChanges();

        var mockCollab = new Mock<ICollaborator>();
        mockCollab.SetupGet(c => c.Id).Returns(id);

        _mapper.Setup(m => m.Map<CollaboratorDataModel, ICollaborator>(collabDM))
               .Returns(mockCollab.Object);

        // Act
        var result = _repository.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result?.Id);
        _mapper.Verify(m => m.Map<CollaboratorDataModel, ICollaborator>(collabDM), Times.Once);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = _repository.GetById(id);

        // Assert
        Assert.Null(result);
        _mapper.Verify(m => m.Map<CollaboratorDataModel, ICollaborator>(It.IsAny<CollaboratorDataModel>()), Times.Never);
    }
}
