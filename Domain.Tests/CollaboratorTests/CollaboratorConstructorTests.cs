using Domain.Models;
using Moq;

namespace Domain.Tests.CollaboratorTests;

public class CollaboratorConstructorTests
{
    [Fact]
    public void WhenPassingValidGUID_ThenReturnCollaborator()
    {
        // Act
        new Collaborator(It.IsAny<Guid>());
    }

}
