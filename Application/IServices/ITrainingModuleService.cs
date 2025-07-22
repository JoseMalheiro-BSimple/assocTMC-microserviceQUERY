using Application.DTO;

namespace Application.IServices;

public interface ITrainingModuleService
{
    public Task SubmitAsync(CreateTrainingModuleDTO createDTO);
}
