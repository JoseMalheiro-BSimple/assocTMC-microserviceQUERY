﻿using Application.DTO;
using Application.IServices;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/associationsTMC")]
[ApiController]
public class AssociationTrainingModuleCollaboratorController : ControllerBase
{
    private readonly IAssociationTrainingModuleCollaboratorService _associationTrainingModuleCollaboratorService;

    public AssociationTrainingModuleCollaboratorController(IAssociationTrainingModuleCollaboratorService associationTrainingModuleCollaboratorService)
    {
        _associationTrainingModuleCollaboratorService = associationTrainingModuleCollaboratorService;
    }

    // Get all associations of training module and collaborator
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllByCollabAndTrainingModule([FromQuery] Guid collabId, [FromQuery] Guid trainingModuleId)
    {
        var dto = new SearchByCollabAndTrainingModuleDTO(collabId, trainingModuleId);
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByCollabAndTrainingModule(dto);

        return assocs.ToActionResult();
    }

    // Get all associations with training module Id
    [HttpGet("by-trainingModule/{id}")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllAssociatedWithTrainingModule(Guid id)
    {
        var dto = new SearchByIdDTO(id);
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByTrainingModule(dto);

        return assocs.ToActionResult();
    }

    // Get all associations who have finished with a determined training module on a certain period
    [HttpGet("by-trainingModule/{id}/finished")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllWithFinishedTrainingModule(Guid id, [FromQuery] PeriodDate periodDate)
    {
        var dto = new SearchByIdAndPeriodDateDTO(id, periodDate);
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByTrainingModuleFinishedOnDate(dto);

        return assocs.ToActionResult();
    }

    // Get allassociations by collaborator Id
    [HttpGet("by-collaborator/{id}")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllAssociatedWithCollab(Guid id)
    {
        var dto = new SearchByIdDTO(id);
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByCollaborator(dto);

        return assocs.ToActionResult();
    }

    // Get all associations who have finished with a determined collaborator id on a certain period
    [HttpGet("by-collaborator/{id}/finished")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllFinishedAssociatedWithCollab(Guid id, [FromQuery] PeriodDate periodDate)
    {
        var dto = new SearchByIdAndPeriodDateDTO(id, periodDate);
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByCollabAndFinishedInPeriod(dto);

        return assocs.ToActionResult();
    }
}

