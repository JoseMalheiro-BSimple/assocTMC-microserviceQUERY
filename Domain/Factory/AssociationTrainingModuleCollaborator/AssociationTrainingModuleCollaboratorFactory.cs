using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;
public class AssociationTrainingModuleCollaboratorFactory : IAssociationTrainingModuleCollaboratorFactory
{

    public AssociationTrainingModuleCollaboratorFactory(){}

    public IAssociationTrainingModuleCollaborator Create(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        return new AssociationTrainingModuleCollaborator(id, trainingModuleId, collaboratorId, periodDate);
    }

    public IAssociationTrainingModuleCollaborator Create(IAssociationTrainingModuleCollaboratorVisitor visitor)
    {
        return new AssociationTrainingModuleCollaborator(visitor.Id, visitor.TrainingModuleId, visitor.CollaboratorId, visitor.PeriodDate);
    }
}
