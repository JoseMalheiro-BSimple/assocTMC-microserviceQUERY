namespace Application.DTO;

public record CreateCollaboratorDTO
{
    public Guid Id { get; set; }

    public CreateCollaboratorDTO(Guid id)
    {
        Id = id;
    }
}
