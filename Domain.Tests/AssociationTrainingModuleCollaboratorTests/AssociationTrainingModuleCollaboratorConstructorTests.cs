using Domain.Models;
using Moq;

namespace Domain.Tests.AssociationTrainingModuleCollaboratorTests;
public class AssociationTrainingModuleCollaboratorConstructorTests
{
    [Fact]
    public void WhenPassingValidDataMinusAssociationGUID_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Act
        new AssociationTrainingModuleCollaborator(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());
    }

    [Fact]
    public void WhenPassingValidDataAndAssociationGUID_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Act
        new AssociationTrainingModuleCollaborator(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());
    }
}
