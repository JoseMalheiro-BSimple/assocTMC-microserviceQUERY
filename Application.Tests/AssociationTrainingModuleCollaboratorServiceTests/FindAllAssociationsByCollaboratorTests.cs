using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByCollaboratorTests
{
    [Fact]
    public async Task FindAllAssociationsByCollaborator_WhenDataExists_ReturnsMappedDtos()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();

        var assocList = new List<AssociationTrainingModuleCollaborator>
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
                .ReturnsAsync(assocList);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used here

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByCollaborator(collaboratorId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

        var dto = result.Value.First();
        Assert.Equal(assocList[0].Id, dto.Id);
        Assert.Equal(assocList[0].CollaboratorId, dto.CollaboratorId);
        Assert.Equal(assocList[0].TrainingModuleId, dto.TrainingModuleId);
        Assert.Equal(assocList[0].PeriodDate, dto.PeriodDate);

        repoMock.Verify(r => r.GetByCollaboratorId(collaboratorId), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByCollaborator_WhenRepositoryThrows_ReturnsFailure()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByCollaboratorId(collaboratorId))
                .ThrowsAsync(new Exception("Unexpected database error"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByCollaborator(collaboratorId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Unexpected database error", result.Error!.Message);

        repoMock.Verify(r => r.GetByCollaboratorId(collaboratorId), Times.Once);
    }
}
