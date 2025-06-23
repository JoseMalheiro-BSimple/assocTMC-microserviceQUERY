using MassTransit;

namespace InterfaceAdapters.Consumers;

public class CollaboratorConsumerDefinition : ConsumerDefinition<CollaboratorCreatedConsumer>
{
    public CollaboratorConsumerDefinition()
    {
        var instanceId = Environment.MachineName;
        EndpointName = $"query-collaborator-created-events-{instanceId}";
    }
}
