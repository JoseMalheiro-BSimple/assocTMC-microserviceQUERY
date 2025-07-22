using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByCollaboratorAndFinishedInPeriodTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByCollaboratorAndFinishedInPeriodTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByCollaboratorAndFinishedInPeriod_ReturnsCorrectResults_WhenMatchesExist()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        var period = new PeriodDate
        {
            InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)),
            FinalDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var inRange1 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collaboratorId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-4)),
                FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            }
        };

        var inRange2 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collaboratorId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)),
                FinalDate = DateOnly.FromDateTime(DateTime.UtcNow)
            }
        };

        var outOfRange = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collaboratorId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
                FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6))
            }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(inRange1, inRange2, outOfRange);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
                .Returns((AssociationTrainingModuleCollaboratorDataModel dm) =>
                {
                    var mock = new Mock<IAssociationTrainingModuleCollaborator>();
                    mock.SetupGet(x => x.Id).Returns(dm.Id);
                    mock.SetupGet(x => x.CollaboratorId).Returns(dm.CollaboratorId);
                    mock.SetupGet(x => x.TrainingModuleId).Returns(dm.TrainingModuleId);
                    mock.SetupGet(x => x.PeriodDate).Returns(dm.PeriodDate);
                    return mock.Object;
                });

        // Act
        var result = await _repository.GetByCollaboratorAndFinishedInPeriod(collaboratorId, period);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, item =>
        {
            Assert.Equal(collaboratorId, item.CollaboratorId);
            Assert.InRange(item.PeriodDate.FinalDate, period.InitDate, period.FinalDate);
        });

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetByCollaboratorAndFinishedInPeriod_ReturnsEmpty_WhenNoMatch()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate
        {
            InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
            FinalDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var record = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collaboratorId,
            TrainingModuleId = Guid.NewGuid(),
            PeriodDate = new PeriodDate
            {
                InitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
                FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6))
            }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(record);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCollaboratorAndFinishedInPeriod(collaboratorId, period);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
