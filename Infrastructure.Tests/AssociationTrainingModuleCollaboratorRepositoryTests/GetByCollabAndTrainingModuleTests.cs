using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByCollabAndTrainingModuleTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByCollabAndTrainingModuleTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByCollabAndTrainingModule_ReturnsMappedAssociations_WhenDataExists()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        var dataModels = new List<AssociationTrainingModuleCollaboratorDataModel>
        {
            new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = Guid.NewGuid(),
                CollaboratorId = collabId,
                TrainingModuleId = trainingModuleId,
                // fill other required fields if any
            },
            // add another unrelated record to test filtering
            new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = Guid.NewGuid(),
                CollaboratorId = Guid.NewGuid(),
                TrainingModuleId = Guid.NewGuid(),
            }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(dataModels);
        await context.SaveChangesAsync();

        // Setup mapper mock to map from DataModel to Domain model (IAssociationTrainingModuleCollaborator)
        _mapper
            .Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
                It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
            .Returns((AssociationTrainingModuleCollaboratorDataModel dm) =>
            {
                // Create a dummy IAssociationTrainingModuleCollaborator with the same IDs
                var mockDomainModel = new Mock<IAssociationTrainingModuleCollaborator>();
                mockDomainModel.SetupGet(x => x.Id).Returns(dm.Id);
                mockDomainModel.SetupGet(x => x.CollaboratorId).Returns(dm.CollaboratorId);
                mockDomainModel.SetupGet(x => x.TrainingModuleId).Returns(dm.TrainingModuleId);
                return mockDomainModel.Object;
            });

        // Act
        var result = await _repository.GetByCollabAndTrainingModule(collabId, trainingModuleId);

        // Assert
        Assert.NotNull(result);
        var list = result.ToList();
        Assert.Single(list); // Only one matching record
        Assert.Equal(collabId, list[0].CollaboratorId);
        Assert.Equal(trainingModuleId, list[0].TrainingModuleId);

        // Verify mapper was called for each DataModel returned by the query
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(list.Count));
    }

    [Fact]
    public async Task GetByCollabAndTrainingModule_ReturnsEmptyList_WhenNoDataMatches()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        // Seed unrelated data to context
        var unrelated = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = Guid.NewGuid()
        };
        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(unrelated);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCollabAndTrainingModule(collabId, trainingModuleId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Mapper should never be called since nothing matched
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
