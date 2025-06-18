using Domain.Models;
using Moq;

namespace Domain.Tests.TrainingModuleTests;

public class TrainingModuleFactoryTests
{
    [Fact]
    public void WhenPassingValidGUID_ThenReturnTrainingModule()
    {
        // Act
        new TrainingModule(It.IsAny<Guid>());
    }
}
