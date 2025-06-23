using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByTrainingModuleFinishedOnDateTests
{
    [Fact]
    public async Task FindAllAssociationsByTrainingModuleFinishedOnDate_ReturnsMappedDtos_OnSuccess()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var periodDate = new PeriodDate(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

        var associations = new List<AssociationTrainingModuleCollaborator>
        {
            new AssociationTrainingModuleCollaborator(
                Guid.NewGuid(),
                trainingModuleId,
                Guid.NewGuid(),
                periodDate
            )
        };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, periodDate))
                .ReturnsAsync(associations);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used here

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByTrainingModuleFinishedOnDate(trainingModuleId, periodDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

        var dto = result.Value.First();
        Assert.Equal(associations[0].Id, dto.Id);
        Assert.Equal(associations[0].CollaboratorId, dto.CollaboratorId);
        Assert.Equal(associations[0].TrainingModuleId, dto.TrainingModuleId);
        Assert.Equal(associations[0].PeriodDate, dto.PeriodDate);

        repoMock.Verify(r => r.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, periodDate), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByTrainingModuleFinishedOnDate_ReturnsFailure_OnException()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var periodDate = new PeriodDate(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, periodDate))
                .ThrowsAsync(new Exception("Repository failure"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByTrainingModuleFinishedOnDate(trainingModuleId, periodDate);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Repository failure", result.Error!.Message);

        repoMock.Verify(r => r.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, periodDate), Times.Once);
    }
}

