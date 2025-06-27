using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;

public class GetByCollaboratorIdTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByCollaboratorIdTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByCollaboratorId_ReturnsMappedResults_WhenMatchesExist()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        var data1 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collaboratorId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow) }
        };

        var data2 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collaboratorId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-4)), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)) }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(data1, data2);
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

        // Act
        var result = await _repository.GetByCollaboratorId(collaboratorId);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, item => Assert.Equal(collaboratorId, item.CollaboratorId));

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetByCollaboratorId_ReturnsEmpty_WhenNoMatch()
    {
        // Arrange
        var otherCollaboratorId = Guid.NewGuid();

        var unrelated = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(), // does not match
            TrainingModuleId = Guid.NewGuid(),
            PeriodDate = new PeriodDate { InitDate = DateOnly.FromDateTime(DateTime.UtcNow), FinalDate = DateOnly.FromDateTime(DateTime.UtcNow) }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(unrelated);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCollaboratorId(otherCollaboratorId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
