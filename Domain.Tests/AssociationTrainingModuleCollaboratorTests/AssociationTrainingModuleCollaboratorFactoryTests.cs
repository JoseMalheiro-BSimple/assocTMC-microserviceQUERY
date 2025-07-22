using Domain.Factory;
using Domain.IRepository;
using Domain.ValueObjects;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.AssociationTrainingModuleCollaboratorTests;
public class AssociationTrainingModuleCollaboratorFactoryTests
{
    [Fact]
    public void WhenPassingValidDependencies_ThenInstatiateFactory()
    {
        // Arrange

        // Act
        var factory = new AssociationTrainingModuleCollaboratorFactory(); ;

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public void WhenPassingValidatedData_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange
        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory();

        // Act
        var result = assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange 
        Mock<IAssociationTrainingModuleCollaboratorVisitor> visitor = new Mock<IAssociationTrainingModuleCollaboratorVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.CollaboratorId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.TrainingModuleId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.PeriodDate).Returns(It.IsAny<PeriodDate>());

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory();

        // Act
        var assocTMC = assocTMCFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(assocTMC);
    }

}
