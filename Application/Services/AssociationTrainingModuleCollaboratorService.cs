using Application.DTO;
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

    /**
     * Method to create a new association when the microservice is consuming a message 
     * of the AssociationTrainingModuleCollaboratorCreated type
     * 
     * Because this is coming from another microservice -> no validation is needed
     */
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

    /**
     * Method gets associations that contain a certain training module 
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModule(Guid id)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByTrainingModuleId(id);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    /**
     * Method gets associations that contain a certain collaborator 
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollaborator(Guid id)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollaboratorId(id);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    /**
     * Method gets all associations with a training module from a list of training modules
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByMultipleTrainingModules(IEnumerable<Guid> ids)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByTrainingModuleIds(ids);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    /**
     * Method gets all associations with a training module who have finished
     * inside the period passed
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModuleFinishedOnDate(Guid id, PeriodDate date)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByTrainingModuleAndFinishedInPeriod(id, date);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    /**
     * Method gets all associations with a determined collaborator
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollab(Guid id)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollaboratorId(id);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    /**
     * Method gets all associations with collaborators from a list of collaborators
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByMultipleCollabs(IEnumerable<Guid> ids)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollaboratorIds(ids);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    /**
     * Method gets all associations with a collaborator,
     * who have finished in-between the period
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndFinishedInPeriod(Guid id, PeriodDate date)
    {
        IEnumerable<AssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollaboratorAndFinishedInPeriod(id, date);

            assocsResult = assocs.Select(a =>
            {
                var dto = new AssociationTrainingModuleCollaboratorDTO();
                dto.Id = a.Id;
                dto.CollaboratorId = a.CollaboratorId;
                dto.TrainingModuleId = a.TrainingModuleId;
                dto.PeriodDate = a.PeriodDate;

                return dto;
            });

            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Success(assocsResult);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>.Failure(Error.InternalServerError(ex.Message));
        }
    }
}