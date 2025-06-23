using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;
public interface IAssociationTrainingModuleCollaboratorsRepository : IGenericRepositoryEF<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaborator, IAssociationTrainingModuleCollaboratorVisitor>
{
    Task<IEnumerable<AssociationTrainingModuleCollaborator>> GetByTrainingModuleId(Guid id);
    Task<IEnumerable<AssociationTrainingModuleCollaborator>> GetByTrainingModuleIds(IEnumerable<Guid> trainingModuleIds);
    Task<IEnumerable<AssociationTrainingModuleCollaborator>> GetByTrainingModuleAndFinishedInPeriod(Guid id, PeriodDate periodDate);
    Task<IEnumerable<AssociationTrainingModuleCollaborator>> GetByCollaboratorId(Guid id);
    Task<IEnumerable<AssociationTrainingModuleCollaborator>> GetByCollaboratorIds(IEnumerable<Guid> collabIds);
    Task<IEnumerable<AssociationTrainingModuleCollaborator>> GetByCollaboratorAndFinishedInPeriod(Guid id, PeriodDate periodDate);
}
