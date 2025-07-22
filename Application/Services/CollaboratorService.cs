using Application.DTO;
using Application.IServices;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;
public class CollaboratorService : ICollaboratorService
{
    public ICollaboratorRepository _collaboratorRepository { get; set; }
    public ICollaboratorFactory _collaboratorFactory { get; set; }

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, ICollaboratorFactory collaboratorFactory)
    {
        _collaboratorRepository = collaboratorRepository;
        _collaboratorFactory = collaboratorFactory;
    }

    public async Task SubmitAsync(CreateCollaboratorDTO creaateDTO)
    {
        ICollaborator collaborator;

        collaborator =  _collaboratorFactory.Create(creaateDTO.Id);
        collaborator = await _collaboratorRepository.AddAsync(collaborator);

        if (collaborator == null)
            throw new Exception("An error as occured!");
    }
}
