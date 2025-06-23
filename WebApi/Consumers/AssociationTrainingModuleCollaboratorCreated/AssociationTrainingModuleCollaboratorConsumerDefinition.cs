using MassTransit;

namespace InterfaceAdapters.Consumers.Definition;

public class AssociationTrainingModuleCollaboratorConsumerDefinition : ConsumerDefinition<AssociationTrainingModuleCollaboratorCreatedConsumer>
{
    public AssociationTrainingModuleCollaboratorConsumerDefinition()
    {
        var instanceId = Environment.MachineName; 
        EndpointName = $"query-associationTrainingModuleCollaborator-created-events-{instanceId}";
    }
}
