﻿using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;

public interface IAssociationTrainingModuleCollaboratorFactory
{
    IAssociationTrainingModuleCollaborator Create(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    IAssociationTrainingModuleCollaborator Create(IAssociationTrainingModuleCollaboratorVisitor visitor);
}
