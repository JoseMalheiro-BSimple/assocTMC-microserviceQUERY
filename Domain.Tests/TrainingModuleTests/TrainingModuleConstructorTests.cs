using Domain.Factory;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.TrainingModuleTests;

public class TrainingModuleConstructorTests
{
    [Fact]
    public void WhenPassingValidGUID_ThenCreateTrainingModule()
    {
        // Arrange
        var tmFactory = new TrainingModuleFactory();

        // Act
        var tm = tmFactory.Create(It.IsAny<Guid>());

        // Assert
        Assert.NotNull(tm);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateTrainingModule()
    {
        // Arrange
        Mock<ITrainingModuleVisitor> visitor = new Mock<ITrainingModuleVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());

        var tmFactory = new TrainingModuleFactory();

        // Act
        var tm = tmFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(tm);
    }
}
