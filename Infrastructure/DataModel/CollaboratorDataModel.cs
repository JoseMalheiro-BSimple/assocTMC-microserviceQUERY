using Domain.Interfaces;
using Domain.Visitor;

namespace Infrastructure.DataModel;
public class CollaboratorDataModel : ICollaboratorVisitor
{
    public Guid Id { get; set; }

    public CollaboratorDataModel() { }

    public CollaboratorDataModel(ICollaborator collaborator)
    {
        Id = collaborator.Id;
    }
}
