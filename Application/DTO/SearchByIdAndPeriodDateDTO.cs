using Domain.ValueObjects;

namespace Application.DTO;

public record SearchByIdAndPeriodDateDTO(Guid Id, PeriodDate PeriodDate);
