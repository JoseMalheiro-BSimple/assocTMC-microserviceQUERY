namespace Application.Interfaces;
public interface ICollaboratorService
{
    Task SubmitAsync(Guid id);
}
