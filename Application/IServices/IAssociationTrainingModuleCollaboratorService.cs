using Application.DTO;
using Domain.ValueObjects;

namespace Application.IServices;

public interface IAssociationTrainingModuleCollaboratorService
{
    public Task CreateWithNoValidations(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    Task<Result> Remove(RemoveAssociationTrainingModuleCollaboratorDTO assocDTO);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndTrainingModule(Guid collabId, Guid trainingModuleId);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModule(Guid id);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollaborator(Guid id);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModuleFinishedOnDate(Guid id, PeriodDate date);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndFinishedInPeriod(Guid id, PeriodDate date);
}

