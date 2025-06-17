using Application.Interfaces;
using Domain.Messaging;
using MassTransit;

namespace WebApi.Consumers;
public class TrainingModuleCreatedConsumer : IConsumer<TrainingModuleCreated>
{
    private readonly ITrainingModuleService _trainingModuleService;

    public TrainingModuleCreatedConsumer(ITrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }

    public async Task Consume(ConsumeContext<TrainingModuleCreated> context)
    {
        var msg = context.Message;
        await _trainingModuleService.SubmitAsync(msg.id);
    }
}
