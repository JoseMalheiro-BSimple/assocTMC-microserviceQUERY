﻿using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;
public class AssociationTrainingModuleCollaboratorFactory : IAssociationTrainingModuleCollaboratorFactory
{
    private readonly ICollaboratorRepository _collabRepository;
    private readonly ITrainingModuleRepository _trainingModuleRepository;

    public AssociationTrainingModuleCollaboratorFactory(ICollaboratorRepository collaboratorRepository, ITrainingModuleRepository trainingModuleRepository)
    {
        _collabRepository = collaboratorRepository;
        _trainingModuleRepository = trainingModuleRepository;
    }

    public async Task<IAssociationTrainingModuleCollaborator> Create(Guid trainingModuleId, Guid collaboratorId, DateOnly initDate, DateOnly endDate)
    {
        ICollaborator? collab = await _collabRepository.GetByIdAsync(collaboratorId);
        ITrainingModule? trainingModule = await _trainingModuleRepository.GetByIdAsync(trainingModuleId);
        
        if(collab == null)
            throw new ArgumentException("Collaborator must exists");

        if (trainingModule == null)
            throw new ArgumentException("TrainingModule must exist");

        PeriodDate periodDate = new PeriodDate(initDate, endDate);

        return new AssociationTrainingModuleCollaborator(trainingModuleId, collaboratorId, periodDate);
    }

    public IAssociationTrainingModuleCollaborator Create(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        return new AssociationTrainingModuleCollaborator(id, trainingModuleId, collaboratorId, periodDate);

    }

    public AssociationTrainingModuleCollaborator Create(IAssociationTrainingModuleCollaboratorVisitor visitor)
    {
        return new AssociationTrainingModuleCollaborator(visitor.Id, visitor.TrainingModuleId, visitor.CollaboratorId, visitor.PeriodDate);
    }
}
