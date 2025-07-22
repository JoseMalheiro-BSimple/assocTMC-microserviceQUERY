namespace Application.DTO;

public record CreateTrainingModuleDTO
{
    public Guid Id { get; set; }

    public CreateTrainingModuleDTO(Guid id)
    {
        Id = id;
    }
}
