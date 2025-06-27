using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factory;
public interface ITrainingModuleFactory
{
    ITrainingModule Create(Guid id);
    ITrainingModule Create(ITrainingModuleVisitor visitor);
}
