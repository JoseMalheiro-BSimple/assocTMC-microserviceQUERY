using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;
public class TrainingModuleFactory : ITrainingModuleFactory
{
    public ITrainingModule Create(Guid id)
    {
        return new TrainingModule(id);
    }

    public TrainingModule Create(ITrainingModuleVisitor visitor)
    {
        return new TrainingModule(visitor.Id);
    }
}
