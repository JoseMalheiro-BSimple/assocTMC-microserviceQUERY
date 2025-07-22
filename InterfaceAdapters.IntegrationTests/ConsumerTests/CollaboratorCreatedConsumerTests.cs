using Application.DTO;
using Application.IServices;
using Domain.Messages;
using Domain.Models;
using Domain.ValueObjects;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class CollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenCalled_CallsSubmitAsyncOnCollaboratorService()
    {
        // Arrange
        var mockService = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(mockService.Object);

        var message = new CollaboratorCreatedMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDateTime()
        );

        var mockContext = new Mock<ConsumeContext<CollaboratorCreatedMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(new CreateCollaboratorDTO(message.Id)), Times.Once);
    }
}
