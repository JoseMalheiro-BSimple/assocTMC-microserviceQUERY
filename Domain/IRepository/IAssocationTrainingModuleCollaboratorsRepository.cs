using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;
public interface IAssociationTrainingModuleCollaboratorsRepository : IGenericRepositoryEF<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaborator, IAssociationTrainingModuleCollaboratorVisitor>
{
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollabAndTrainingModule(Guid collabId, Guid trainingModuleId);
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByTrainingModuleId(Guid id);
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByTrainingModuleIds(IEnumerable<Guid> trainingModuleIds);
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByTrainingModuleAndFinishedInPeriod(Guid id, PeriodDate periodDate);
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollaboratorId(Guid id);
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollaboratorIds(IEnumerable<Guid> collabIds);
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollaboratorAndFinishedInPeriod(Guid id, PeriodDate periodDate);
}
