using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class AssociationTrainingModuleCollaborator : IAssociationTrainingModuleCollaborator
{
    public Guid Id { get; }
    public Guid TrainingModuleId { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }

    public AssociationTrainingModuleCollaborator(Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        Id = Guid.NewGuid();
        TrainingModuleId = trainingModuleId;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
    }

    public AssociationTrainingModuleCollaborator(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        Id = id;
        TrainingModuleId = trainingModuleId;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
    }
}

