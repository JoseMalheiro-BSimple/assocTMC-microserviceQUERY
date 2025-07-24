using Application.DTO;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByTrainingModuleTests
{
    [Fact]
    public async Task FindAllAssociationsByTrainingModule_WhenRepositoryReturnsData_ReturnsMappedDtos()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();

        var entityList = new List<AssociationTrainingModuleCollaborator>
            {
                new AssociationTrainingModuleCollaborator(
                    Guid.NewGuid(),
                    trainingModuleId,
                    Guid.NewGuid(),
                    It.IsAny<PeriodDate>()
                )
            };

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleId(trainingModuleId))
                .ReturnsAsync(entityList);

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); 

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByTrainingModule(new SearchByIdDTO(trainingModuleId));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

        var dto = result.Value.First();
        Assert.Equal(entityList[0].Id, dto.Id);
        Assert.Equal(entityList[0].CollaboratorId, dto.CollaboratorId);
        Assert.Equal(entityList[0].TrainingModuleId, dto.TrainingModuleId);
        Assert.Equal(entityList[0].PeriodDate, dto.PeriodDate);

        repoMock.Verify(r => r.GetByTrainingModuleId(trainingModuleId), Times.Once);
    }

    [Fact]
    public async Task FindAllAssociationsByTrainingModule_WhenRepositoryThrows_ReturnsFailureResult()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        repoMock.Setup(r => r.GetByTrainingModuleId(trainingModuleId))
                .ThrowsAsync(new Exception("Database failure"));

        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); // Not used here

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object);

        // Act
        var result = await service.FindAllAssociationsByTrainingModule(new SearchByIdDTO(trainingModuleId));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Database failure", result.Error!.Message);

        repoMock.Verify(r => r.GetByTrainingModuleId(trainingModuleId), Times.Once);
    }
}
