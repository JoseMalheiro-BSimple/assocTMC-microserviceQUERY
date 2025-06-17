using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;
public interface ITrainingModuleFactory
{
    ITrainingModule Create(Guid id);
    TrainingModule Create(ITrainingModuleVisitor visitor);
}
