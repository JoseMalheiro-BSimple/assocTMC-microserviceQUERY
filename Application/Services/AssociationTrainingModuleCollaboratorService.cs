using Application.DTO;
using Application.IServices;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;

public class AssociationTrainingModuleCollaboratorService : IAssociationTrainingModuleCollaboratorService
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
    public async Task CreateWithNoValidations(CreateConsumedAssociationTrainingModuleCollaboratorDTO createDTO)
    {
        // There is no data validation, but there is validation to no insert duplicate values on table
        IAssociationTrainingModuleCollaborator? assoc = await _assocTMCRepository.GetByIdAsync(createDTO.Id);

        if (assoc == null)
        {
            IAssociationTrainingModuleCollaborator tmc;

            tmc = _assocTMCFactory.Create(createDTO.Id, createDTO.TrainingModuleId, createDTO.CollaboratorId, createDTO.PeriodDate);
            tmc = await _assocTMCRepository.AddAsync(tmc);

            if (tmc == null)
                throw new Exception("An error has occured!");
        }
    }

    /**
     * Method gets all associations by collaborator and training module Ids
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndTrainingModule(SearchByCollabAndTrainingModuleDTO searchDTO)
    {
        IEnumerable<IAssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollabAndTrainingModule(searchDTO.CollaboratorId, searchDTO.TrainingModuleId);

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
     * Method gets associations that contain a certain training module 
     */
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModule(SearchByIdDTO searchDTO)
    {
        IEnumerable<IAssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByTrainingModuleId(searchDTO.Id);

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
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollaborator(SearchByIdDTO searchDTO)
    {
        IEnumerable<IAssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollaboratorId(searchDTO.Id);

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
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByTrainingModuleFinishedOnDate(SearchByIdAndPeriodDateDTO searchDTO)
    {
        IEnumerable<IAssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByTrainingModuleAndFinishedInPeriod(searchDTO.Id, searchDTO.PeriodDate);

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
    public async Task<Result<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> FindAllAssociationsByCollabAndFinishedInPeriod(SearchByIdAndPeriodDateDTO searchDTO)
    {
        IEnumerable<IAssociationTrainingModuleCollaborator> assocs;
        IEnumerable<AssociationTrainingModuleCollaboratorDTO> assocsResult;
        try
        {
            assocs = await _assocTMCRepository.GetByCollaboratorAndFinishedInPeriod(searchDTO.Id, searchDTO.PeriodDate);
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

    public async Task<Result> Remove(RemoveAssociationTrainingModuleCollaboratorDTO assocDTO)
    {
        IAssociationTrainingModuleCollaborator? associationToRemove;
        try
        {
            associationToRemove = await _assocTMCRepository.GetByIdAsync(assocDTO.Id);

            if (associationToRemove == null)
            {
                return Result.Failure(Error.NotFound("AssociationTrainingModuleCollaborator not found."));
            }

            await _assocTMCRepository.RemoveWoTracked(associationToRemove);
            await _assocTMCRepository.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.InternalServerError($"An unexpected error occurred: {ex.Message}"));
        }
    }
}