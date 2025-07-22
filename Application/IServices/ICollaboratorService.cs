using Application.DTO;

namespace Application.IServices;
public interface ICollaboratorService
{
    public Task SubmitAsync(CreateCollaboratorDTO createDTO);
}

