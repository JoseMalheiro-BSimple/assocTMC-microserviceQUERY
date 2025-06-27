using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByTrainingModuleIdTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByTrainingModuleIdTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByTrainingModuleId_ReturnsMappedAssociations_WhenMatchesExist()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var dataModels = new List<AssociationTrainingModuleCollaboratorDataModel>
            {
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = Guid.NewGuid(),
                    CollaboratorId = Guid.NewGuid(),
                    TrainingModuleId = trainingModuleId
                },
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = Guid.NewGuid(),
                    CollaboratorId = Guid.NewGuid(),
                    TrainingModuleId = trainingModuleId
                },
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = Guid.NewGuid(),
                    CollaboratorId = Guid.NewGuid(),
                    TrainingModuleId = Guid.NewGuid() // unrelated
                }
            };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(dataModels);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
               .Returns((AssociationTrainingModuleCollaboratorDataModel dm) =>
               {
                   var mock = new Mock<IAssociationTrainingModuleCollaborator>();
                   mock.SetupGet(x => x.Id).Returns(dm.Id);
                   mock.SetupGet(x => x.TrainingModuleId).Returns(dm.TrainingModuleId);
                   mock.SetupGet(x => x.CollaboratorId).Returns(dm.CollaboratorId);
                   return mock.Object;
               });

        // Act
        var result = await _repository.GetByTrainingModuleId(trainingModuleId);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, item => Assert.Equal(trainingModuleId, item.TrainingModuleId));

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetByTrainingModuleId_ReturnsEmpty_WhenNoMatchExists()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();

        var unrelated = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = Guid.NewGuid() // does not match
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(unrelated);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByTrainingModuleId(trainingModuleId);

        // Assert
        Assert.Empty(result);
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
