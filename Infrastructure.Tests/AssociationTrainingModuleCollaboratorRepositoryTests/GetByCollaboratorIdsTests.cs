using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByCollaboratorIdsTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByCollaboratorIdsTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByCollaboratorIds_ReturnsMappedResults_WhenMatchesExist()
    {
        // Arrange
        var collabId1 = Guid.NewGuid();
        var collabId2 = Guid.NewGuid();
        var nonMatchCollabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        var data1 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collabId1,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow) }
        };

        var data2 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collabId2,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-4)), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)) }
        };

        var unrelated = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = nonMatchCollabId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-4)), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)) }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(data1, data2, unrelated);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
                .Returns((AssociationTrainingModuleCollaboratorDataModel dm) =>
                {
                    var mock = new Mock<IAssociationTrainingModuleCollaborator>();
                    mock.SetupGet(x => x.Id).Returns(dm.Id);
                    mock.SetupGet(x => x.CollaboratorId).Returns(dm.CollaboratorId);
                    mock.SetupGet(x => x.TrainingModuleId).Returns(dm.TrainingModuleId);
                    mock.SetupGet(x => x.PeriodDate).Returns(dm.PeriodDate);
                    return mock.Object;
                });

        var inputIds = new List<Guid> { collabId1, collabId2 };

        // Act
        var result = await _repository.GetByCollaboratorIds(inputIds);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, item => Assert.Contains(item.CollaboratorId, inputIds));

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetByCollaboratorIds_ReturnsEmpty_WhenNoMatches()
    {
        // Arrange
        var unmatchedId = Guid.NewGuid();

        var data = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = Guid.NewGuid(),
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow) }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(data);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCollaboratorIds(new List<Guid> { unmatchedId });

        // Assert
        Assert.Empty(result);

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
