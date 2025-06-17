using Application.Interfaces;
using Domain.Messaging;
using MassTransit;

namespace WebApi.Consumers;
public class CollaboratorCreatedConsumer : IConsumer<CollaboratorCreated>
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorCreatedConsumer(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public async Task Consume(ConsumeContext<CollaboratorCreated> context)
    {
        var msg = context.Message;
        await _collaboratorService.SubmitAsync(msg.id);
    }
}
