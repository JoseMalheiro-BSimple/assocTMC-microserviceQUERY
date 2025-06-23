using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByMultipleTrainingModulesTests
{
    [Fact]
    public async Task FindAllAssociationsByMultipleTrainingModules_WhenDataExists_ReturnsMappedDtos()
    {
        // Arrange
        var trainingModuleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var assocList = new List<AssociationTrainingModuleCollaborator>
            {
                new AssociationTrainingModuleCollaborator
                (
                    Guid.NewGuid(),
                    trainingModuleIds[0],
                    Guid.NewGuid(),
                    It.IsAny<PeriodDate>()
                )
            };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleIds(trainingModuleIds))
                .ReturnsAsync(assocList);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used here

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByMultipleTrainingModules(trainingModuleIds);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

        var dto = result.Value.First();
        Assert.Equal(assocList[0].Id, dto.Id);
        Assert.Equal(assocList[0].CollaboratorId, dto.CollaboratorId);
        Assert.Equal(assocList[0].TrainingModuleId, dto.TrainingModuleId);
        Assert.Equal(assocList[0].PeriodDate, dto.PeriodDate);

        repoMock.Verify(r => r.GetByTrainingModuleIds(trainingModuleIds), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByMultipleTrainingModules_WhenRepositoryThrows_ReturnsFailure()
    {
        // Arrange
        var trainingModuleIds = new List<Guid> { Guid.NewGuid() };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleIds(trainingModuleIds))
                .ThrowsAsync(new Exception("Something failed"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used here

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByMultipleTrainingModules(trainingModuleIds);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Something failed", result.Error!.Message);

        repoMock.Verify(r => r.GetByTrainingModuleIds(trainingModuleIds), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByMultipleTrainingModules_WhenInputIsEmpty_ReturnsEmptyList()
    {
        // Arrange
        var trainingModuleIds = Enumerable.Empty<Guid>();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleIds(trainingModuleIds))
                .ReturnsAsync(Enumerable.Empty<AssociationTrainingModuleCollaborator>());

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used here

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByMultipleTrainingModules(trainingModuleIds);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);

        repoMock.Verify(r => r.GetByTrainingModuleIds(trainingModuleIds), Times.Once);
    }
}
