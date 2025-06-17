using Domain.Interfaces;
using Domain.Visitor;

namespace Infrastructure.DataModel;
public class TrainingModuleDataModel : ITrainingModuleVisitor
{
    public Guid Id { get; set; }

    public TrainingModuleDataModel() { }

    public TrainingModuleDataModel(ITrainingModule trainingModule)
    {
        Id = trainingModule.Id;
    }
}
