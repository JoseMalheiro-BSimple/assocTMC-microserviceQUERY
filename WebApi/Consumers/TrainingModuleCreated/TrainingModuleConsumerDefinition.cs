using MassTransit;

namespace InterfaceAdapters.Consumers;

public class TrainingModuleConsumerDefinition : ConsumerDefinition<TrainingModuleCreatedConsumer>
{
    public TrainingModuleConsumerDefinition()
    {
        var instanceId = Environment.MachineName;
        EndpointName = $"query-trainingModule-created-events-{instanceId}";
    }
}
