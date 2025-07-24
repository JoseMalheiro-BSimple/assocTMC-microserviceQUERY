using Application.DTO;

namespace Application.IServices;

public interface IAssociationTrainingModuleCollaboratorService
{
    public Task CreateWithNoValidations(CreateConsumedAssociationTrainingModuleCollaboratorDTO createDTO);
    Task<Result> Remove(RemoveAssociationTrainingModuleCollaboratorDTO assocDTO);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndTrainingModule(SearchByCollabAndTrainingModuleDTO searchDTO);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModule(SearchByIdDTO searchDTO);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollaborator(SearchByIdDTO searchDTO);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModuleFinishedOnDate(SearchByIdAndPeriodDateDTO searchDTO);
    public Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndFinishedInPeriod(SearchByIdAndPeriodDateDTO searchDTO);
}

