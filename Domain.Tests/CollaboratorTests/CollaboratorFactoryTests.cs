using Domain.Factory;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.CollaboratorTests;
public class CollaboratorFactoryTests
{
    [Fact]
    public void WhenPassingValidId_ThenCreateCollaborator()
    {
        // Arrange
        var collaboratorFactory = new CollaboratorFactory();

        // Act
        var collab = collaboratorFactory.Create(It.IsAny<Guid>());

        // Assert
        Assert.NotNull(collab);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateCollaborator()
    {
        // Arrange
        Mock<ICollaboratorVisitor> visitor = new Mock<ICollaboratorVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());

        var collaboratorFactory = new CollaboratorFactory();

        // Act
        var collab = collaboratorFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(collab);
    }
}
