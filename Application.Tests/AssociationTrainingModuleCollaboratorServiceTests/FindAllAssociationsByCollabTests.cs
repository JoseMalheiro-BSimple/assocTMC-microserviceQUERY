using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByCollabTests
{
    [Fact]
    public async Task FindAllAssociationsByCollab_ReturnsMappedDtos_OnSuccess()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var associations = new List<AssociationTrainingModuleCollaborator>
        {
            new AssociationTrainingModuleCollaborator(
                Guid.NewGuid(),
                Guid.NewGuid(),
                collaboratorId,
                It.IsAny<PeriodDate>()
                )
        };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorId(collaboratorId))
                .ReturnsAsync(associations);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByCollab(collaboratorId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

        var dto = result.Value.First();
        Assert.Equal(associations[0].Id, dto.Id);
        Assert.Equal(associations[0].CollaboratorId, dto.CollaboratorId);
        Assert.Equal(associations[0].TrainingModuleId, dto.TrainingModuleId);
        Assert.Equal(associations[0].PeriodDate, dto.PeriodDate);

        repoMock.Verify(r => r.GetByCollaboratorId(collaboratorId), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByCollab_ReturnsFailure_OnException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorId(collaboratorId))
                .ThrowsAsync(new Exception("Repo failed"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByCollab(collaboratorId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Repo failed", result.Error!.Message);

        repoMock.Verify(r => r.GetByCollaboratorId(collaboratorId), Times.Once);
    }
}
