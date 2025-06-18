using Domain.Interfaces;

namespace Domain.Models;
public class Collaborator : ICollaborator
{
    public Guid Id { get; set; }

    public Collaborator(Guid id)
    {
        Id = id;
    }
}
