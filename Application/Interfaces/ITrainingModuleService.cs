namespace Application.Interfaces;
public interface ITrainingModuleService
{
    Task SubmitAsync(Guid id);
}
