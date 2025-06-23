using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByMultipleCollabsTests
{
    [Fact]
    public async Task FindAllAssociationsByMultipleCollabs_ReturnsMappedDtos_OnSuccess()
    {
        // Arrange
        var collaboratorIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var associations = new List<AssociationTrainingModuleCollaborator>
        {
            new AssociationTrainingModuleCollaborator
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                collaboratorIds[0],
                It.IsAny<PeriodDate>()
            ),
            new AssociationTrainingModuleCollaborator
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                collaboratorIds[1],
                It.IsAny<PeriodDate>()
            )
        };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorIds(collaboratorIds))
                .ReturnsAsync(associations);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByMultipleCollabs(collaboratorIds);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());

        foreach (var dto in result.Value)
        {
            var assoc = associations.First(a => a.Id == dto.Id);
            Assert.Equal(assoc.CollaboratorId, dto.CollaboratorId);
            Assert.Equal(assoc.TrainingModuleId, dto.TrainingModuleId);
            Assert.Equal(assoc.PeriodDate, dto.PeriodDate);
        }

        repoMock.Verify(r => r.GetByCollaboratorIds(collaboratorIds), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByMultipleCollabs_ReturnsFailure_OnException()
    {
        // Arrange
        var collaboratorIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorIds(collaboratorIds))
                .ThrowsAsync(new Exception("Repository failure"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByMultipleCollabs(collaboratorIds);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Repository failure", result.Error!.Message);

        repoMock.Verify(r => r.GetByCollaboratorIds(collaboratorIds), Times.Once);
    }
}
