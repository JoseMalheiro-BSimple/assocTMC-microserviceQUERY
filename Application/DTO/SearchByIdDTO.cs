using Domain.ValueObjects;

namespace Application.DTO;

public record SearchByIdDTO
{
    public Guid Id { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public SearchByIdDTO(Guid id, PeriodDate periodDate)
    {
        Id = id;
        PeriodDate = periodDate;
    }
}
