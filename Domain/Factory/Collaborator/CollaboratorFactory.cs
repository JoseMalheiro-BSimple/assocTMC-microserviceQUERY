using Domain.Interfaces;
using Domain.Visitor;
using Domain.Models;

namespace Domain.Factory;
public class CollaboratorFactory : ICollaboratorFactory
{
    public ICollaborator Create(Guid id)
    {
        return new Collaborator(id);
    }

    public Collaborator Create(ICollaboratorVisitor visitor)
    {
        return new Collaborator(visitor.Id);
    }
}
