using Domain.Interfaces;

namespace Domain.Models;
public class TrainingModule: ITrainingModule
{
    public Guid Id { get; set; }

    public TrainingModule(Guid id)
    {
        Id = id;
    }
}
