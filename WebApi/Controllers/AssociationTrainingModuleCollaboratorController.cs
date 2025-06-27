using Application.DTO;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/associationsTMC")]
[ApiController]
public class AssociationTrainingModuleCollaboratorController : ControllerBase
{
    private readonly IAssociationTrainingModuleCollaboratorService _associationTrainingModuleCollaboratorService;

    public AssociationTrainingModuleCollaboratorController(AssociationTrainingModuleCollaboratorService associationTrainingModuleCollaboratorService)
    {
        _associationTrainingModuleCollaboratorService = associationTrainingModuleCollaboratorService;
    }

    // Get all associations of training module and collaborator
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllByCollabAndTrainingModule([FromQuery] Guid collabId, [FromQuery] Guid trainingModuleId)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByCollabAndTrainingModule(collabId, trainingModuleId);

        return assocs.ToActionResult();
    }

    // Get all associations with training module Id
    [HttpGet("by-trainingModule/{id}")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllAssociatedWithTrainingModule(Guid id)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByTrainingModule(id);

        return assocs.ToActionResult();
    }

    // Get all associations with training module Id
    [HttpGet("by-multiple-trainingModules")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllAssociatedWithMultipleTrainingModules([FromQuery] List<Guid> trainingModuleIds)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByMultipleTrainingModules(trainingModuleIds);

        return assocs.ToActionResult();
    }

    // Get all associations who have finished with a determined training module on a certain period
    [HttpGet("by-trainingModule/{id}/finished")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllWithFinishedTrainingModule(Guid id, [FromQuery] PeriodDate periodDate)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByTrainingModuleFinishedOnDate(id, periodDate);

        return assocs.ToActionResult();
    }

    // Get allassociations by collaborator Id
    [HttpGet("by-collaborator/{id}")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllAssociatedWithCollab(Guid id)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByCollab(id);

        return assocs.ToActionResult();
    }

    // Get all associations by multiple collaborator Id
    [HttpGet("by-multiple-collaborators")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllAssociatedWithMultipleCollabs([FromQuery] List<Guid> collabIds)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByMultipleCollabs(collabIds);

        return assocs.ToActionResult();
    }

    // Get all associations who have finished with a determined collaborator id on a certain period
    [HttpGet("by-collaborator/{id}/finished")]
    public async Task<ActionResult<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>> GetAllFinishedAssociatedWithCollab(Guid id, [FromQuery] PeriodDate periodDate)
    {
        var assocs = await _associationTrainingModuleCollaboratorService.FindAllAssociationsByCollabAndFinishedInPeriod(id, periodDate);

        return assocs.ToActionResult();
    }
}

