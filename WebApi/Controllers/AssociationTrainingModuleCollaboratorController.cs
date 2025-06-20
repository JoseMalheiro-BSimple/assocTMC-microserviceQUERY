using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/associationsTMC")]
[ApiController]
public class AssociationTrainingModuleCollaboratorController : ControllerBase
{
    private readonly AssociationTrainingModuleCollaboratorService _associationTrainingModuleCollaboratorService;

    public AssociationTrainingModuleCollaboratorController(AssociationTrainingModuleCollaboratorService associationTrainingModuleCollaboratorService)
    {
        _associationTrainingModuleCollaboratorService = associationTrainingModuleCollaboratorService;
    }

    
}

