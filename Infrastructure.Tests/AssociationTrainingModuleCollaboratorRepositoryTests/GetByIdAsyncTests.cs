using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByIdAsyncTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByIdAsyncTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedObject_WhenRecordExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var data = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = id,
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = Guid.NewGuid(),
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow) }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(data);
        await context.SaveChangesAsync();

        var domainMock = new Mock<IAssociationTrainingModuleCollaborator>();
        domainMock.SetupGet(d => d.Id).Returns(data.Id);
        domainMock.SetupGet(d => d.CollaboratorId).Returns(data.CollaboratorId);
        domainMock.SetupGet(d => d.TrainingModuleId).Returns(data.TrainingModuleId);
        domainMock.SetupGet(d => d.PeriodDate).Returns(data.PeriodDate);

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(data))
                .Returns(domainMock.Object);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result?.Id);
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(data), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenRecordDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
