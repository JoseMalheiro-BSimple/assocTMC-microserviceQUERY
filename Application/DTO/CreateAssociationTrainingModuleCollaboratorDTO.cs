using Domain.Models;

namespace Application.DTO;

public record CreateAssociationTrainingModuleCollaboratorDTO
{
    public Guid CollaboratorId { get; set; }
    public Guid TrainingModuleId { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public CreateAssociationTrainingModuleCollaboratorDTO()
    {

    }
}