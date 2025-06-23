using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class CreateWithNoValidationTests
{
    [Fact]
    public async Task CreateWithNoValidations_WhenAssocDoesNotExist_CreatesAndAdds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var mockFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var mockRepository = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var mockAssoc = new Mock<IAssociationTrainingModuleCollaborator>();

        mockRepository
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((IAssociationTrainingModuleCollaborator?)null); // Not found

        mockFactory
            .Setup(f => f.Create(id, trainingModuleId, collaboratorId, It.IsAny<PeriodDate>()))
            .Returns(mockAssoc.Object);

        mockRepository
            .Setup(r => r.AddAsync(mockAssoc.Object))
            .ReturnsAsync(mockAssoc.Object); // Successfully added

        var service = new AssociationTrainingModuleCollaboratorService(mockRepository.Object, mockFactory.Object);

        // Act
        await service.CreateWithNoValidations(id, trainingModuleId, collaboratorId, It.IsAny<PeriodDate>());

        // Assert
        mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        mockFactory.Verify(f => f.Create(id, trainingModuleId, collaboratorId, It.IsAny<PeriodDate>()), Times.Once);
        mockRepository.Verify(r => r.AddAsync(mockAssoc.Object), Times.Once);
    }

    [Fact]
    public async Task CreateWithNoValidations_WhenAssocExists_DoesNothing()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var mockAssoc = new Mock<IAssociationTrainingModuleCollaborator>();
        var mockFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var mockRepository = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        mockRepository
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(mockAssoc.Object); // Simulate it already exists

        var service = new AssociationTrainingModuleCollaboratorService(mockRepository.Object, mockFactory.Object);

        // Act
        await service.CreateWithNoValidations(id, trainingModuleId, collaboratorId, It.IsAny<PeriodDate>());

        // Assert
        mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        mockFactory.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
        mockRepository.Verify(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Never);
    }

}
