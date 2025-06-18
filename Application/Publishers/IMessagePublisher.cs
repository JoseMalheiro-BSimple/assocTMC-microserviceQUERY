using Domain.Models;

namespace Application.Publishers;
public interface IMessagePublisher
{
    Task PublishOrderSubmittedAsync(Guid Id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate); 
}
