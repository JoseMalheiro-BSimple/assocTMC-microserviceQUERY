using Application.DTO;
using Domain.Models;

namespace Application.Services;

public interface IAssociationTrainingModuleCollaboratorService
{
    public Task CreateWithNoValidations(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndTrainingModule(Guid collabId, Guid trainingModuleId);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModule(Guid id);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollaborator(Guid id);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByMultipleTrainingModules(IEnumerable<Guid> ids);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModuleFinishedOnDate(Guid id, PeriodDate date);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollab(Guid id);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByMultipleCollabs(IEnumerable<Guid> ids);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndFinishedInPeriod(Guid id, PeriodDate date);
}

