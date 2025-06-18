using Application.Messaging;
using Domain.Messaging;
using MassTransit;

namespace Infrastructure.Messaging;
public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishOrderSubmittedAsync(Guid Id)
    {
        await _publishEndpoint.Publish(new AssociationTrainingModuleCollaboratorCreated(Id));
    }
}
