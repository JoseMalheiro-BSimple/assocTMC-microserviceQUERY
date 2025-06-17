using Domain.Models;

namespace Domain.Visitor;
public interface IAssociationTrainingModuleCollaboratorVisitor
{
    Guid Id { get; }
    Guid TrainingModuleId { get; }
    Guid CollaboratorId { get; }
    PeriodDate PeriodDate { get; }
}

