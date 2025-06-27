using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;

public class GetByTrainingModuleIdsTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByTrainingModuleIdsTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByTrainingModuleIds_ReturnsMappedAssociations_WhenMatchesExist()
    {
        // Arrange
        var tmId1 = Guid.NewGuid();
        var tmId2 = Guid.NewGuid();
        var tmId3 = Guid.NewGuid(); // will not be in the input list

        var dataModels = new List<AssociationTrainingModuleCollaboratorDataModel>
            {
                new AssociationTrainingModuleCollaboratorDataModel { Id = Guid.NewGuid(), CollaboratorId = Guid.NewGuid(), TrainingModuleId = tmId1 },
                new AssociationTrainingModuleCollaboratorDataModel { Id = Guid.NewGuid(), CollaboratorId = Guid.NewGuid(), TrainingModuleId = tmId2 },
                new AssociationTrainingModuleCollaboratorDataModel { Id = Guid.NewGuid(), CollaboratorId = Guid.NewGuid(), TrainingModuleId = tmId3 }
            };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(dataModels);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
               .Returns((AssociationTrainingModuleCollaboratorDataModel dm) =>
               {
                   var mock = new Mock<IAssociationTrainingModuleCollaborator>();
                   mock.SetupGet(x => x.Id).Returns(dm.Id);
                   mock.SetupGet(x => x.CollaboratorId).Returns(dm.CollaboratorId);
                   mock.SetupGet(x => x.TrainingModuleId).Returns(dm.TrainingModuleId);
                   return mock.Object;
               });

        var inputIds = new List<Guid> { tmId1, tmId2 };

        // Act
        var result = await _repository.GetByTrainingModuleIds(inputIds);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, item => Assert.Contains(item.TrainingModuleId, inputIds));

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetByTrainingModuleIds_ReturnsEmpty_WhenNoMatches()
    {
        // Arrange
        var unrelatedId = Guid.NewGuid();

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = unrelatedId
        });

        await context.SaveChangesAsync();

        var inputIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }; // no matching IDs

        // Act
        var result = await _repository.GetByTrainingModuleIds(inputIds);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }

    [Fact]
    public async Task GetByTrainingModuleIds_ReturnsEmpty_WhenInputListIsEmpty()
    {
        // Arrange
        var emptyInput = Enumerable.Empty<Guid>();

        // Act
        var result = await _repository.GetByTrainingModuleIds(emptyInput);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}