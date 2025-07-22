using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class FindAllAssociationsByCollabAndTrainingModuleTests
{
    [Fact]
    public async Task FindAllAssociationsByCollabAndTrainingModule_ReturnsSuccess_WithData()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _mockRepository = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _mockFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        var associations = new List<AssociationTrainingModuleCollaborator>
        {
            new AssociationTrainingModuleCollaborator(trainingModuleId, collabId, It.IsAny<PeriodDate>())
        };

        _mockRepository.Setup(r => r.GetByCollabAndTrainingModule(collabId, trainingModuleId))
                       .ReturnsAsync(associations);

        AssociationTrainingModuleCollaboratorService _service = new AssociationTrainingModuleCollaboratorService(_mockRepository.Object, _mockFactory.Object);
        
        // Act
        var result = await _service.FindAllAssociationsByCollabAndTrainingModule(collabId, trainingModuleId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        var dto = result.Value.First();
        Assert.Equal(collabId, dto.CollaboratorId);
        Assert.Equal(trainingModuleId, dto.TrainingModuleId);
    }

    [Fact]
    public async Task FindAllAssociationsByCollabAndTrainingModule_ReturnsSuccess_WithEmptyList()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _mockRepository = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _mockFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByCollabAndTrainingModule(collabId, trainingModuleId))
                       .ReturnsAsync(new List<AssociationTrainingModuleCollaborator>());

        AssociationTrainingModuleCollaboratorService _service = new AssociationTrainingModuleCollaboratorService(_mockRepository.Object, _mockFactory.Object);

        // Act
        var result = await _service.FindAllAssociationsByCollabAndTrainingModule(collabId, trainingModuleId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task FindAllAssociationsByCollabAndTrainingModule_ReturnsFailure_OnException()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _mockRepository = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _mockFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByCollabAndTrainingModule(collabId, trainingModuleId))
                       .ThrowsAsync(new Exception("DB failure"));

        AssociationTrainingModuleCollaboratorService _service = new AssociationTrainingModuleCollaboratorService(_mockRepository.Object, _mockFactory.Object);

        // Act
        var result = await _service.FindAllAssociationsByCollabAndTrainingModule(collabId, trainingModuleId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Contains("DB failure", result.Error.Message);
    }
}
