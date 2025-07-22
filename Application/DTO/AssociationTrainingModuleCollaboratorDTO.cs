using Domain.ValueObjects;

namespace Application.DTO;

public record AssociationTrainingModuleCollaboratorDTO
{
    public Guid Id { get; set; }
    public Guid TrainingModuleId { get; set;}
    public Guid CollaboratorId { get; set;}
    public PeriodDate PeriodDate { get; set;}

    public AssociationTrainingModuleCollaboratorDTO(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        Id = id;
        TrainingModuleId = trainingModuleId;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
    }

    public AssociationTrainingModuleCollaboratorDTO()
    {
    }
}