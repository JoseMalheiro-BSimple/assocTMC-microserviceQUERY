using Application.DTO;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByCollabAndFinishedInPeriodTests
{
    [Fact]
    public async Task FindAllAssociationsByCollabAndFinishedInPeriod_ReturnsMappedDtos_OnSuccess()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var periodDate = new PeriodDate(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 1, 31)
        );

        var associations = new List<AssociationTrainingModuleCollaborator>
        {
            new AssociationTrainingModuleCollaborator
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                collaboratorId,
                periodDate
            )
        };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorAndFinishedInPeriod(collaboratorId, periodDate))
                .ReturnsAsync(associations);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByCollabAndFinishedInPeriod(new SearchByIdAndPeriodDateDTO(collaboratorId, periodDate));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

        var dto = result.Value.First();
        Assert.Equal(associations[0].Id, dto.Id);
        Assert.Equal(associations[0].CollaboratorId, dto.CollaboratorId);
        Assert.Equal(associations[0].TrainingModuleId, dto.TrainingModuleId);
        Assert.Equal(associations[0].PeriodDate, dto.PeriodDate);

        repoMock.Verify(r => r.GetByCollaboratorAndFinishedInPeriod(collaboratorId, periodDate), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByCollabAndFinishedInPeriod_ReturnsFailure_OnException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var periodDate = new PeriodDate(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 1, 31)
        );

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorAndFinishedInPeriod(collaboratorId, periodDate))
                .ThrowsAsync(new Exception("Repository error"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByCollabAndFinishedInPeriod(new SearchByIdAndPeriodDateDTO(collaboratorId, periodDate));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Repository error", result.Error!.Message);

        repoMock.Verify(r => r.GetByCollaboratorAndFinishedInPeriod(collaboratorId, periodDate), Times.Once);
    }
}
