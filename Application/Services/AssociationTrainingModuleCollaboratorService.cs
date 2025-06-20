using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services;

public class AssociationTrainingModuleCollaboratorService
{
    public IAssociationTrainingModuleCollaboratorsRepository _assocTMCRepository;
    public IAssociationTrainingModuleCollaboratorFactory _assocTMCFactory;
    public AssociationTrainingModuleCollaboratorService(IAssociationTrainingModuleCollaboratorsRepository associationTrainingModuleCollaboratorsRepository, IAssociationTrainingModuleCollaboratorFactory associationTrainingModuleCollaboratorFactory)
    {
        _assocTMCRepository = associationTrainingModuleCollaboratorsRepository;
        _assocTMCFactory = associationTrainingModuleCollaboratorFactory;
    }

    public async Task CreateWithNoValidations(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        // There is no data validation, but there is validation to no insert duplicate values on table
        IAssociationTrainingModuleCollaborator? assoc = await _assocTMCRepository.GetByIdAsync(id);

        if (assoc == null)
        {
            IAssociationTrainingModuleCollaborator tmc;

            tmc = _assocTMCFactory.Create(id, trainingModuleId, collaboratorId, periodDate);
            await _assocTMCRepository.AddAsync(tmc);
        }
    }
}