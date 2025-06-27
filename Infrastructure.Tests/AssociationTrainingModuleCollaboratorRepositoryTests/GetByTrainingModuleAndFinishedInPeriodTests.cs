using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByTrainingModuleAndFinishedInPeriodTests : RepositoryTestBase
{
    private readonly AssociationTrainingModuleCollaboratorRepositoryEF _repository;

    public GetByTrainingModuleAndFinishedInPeriodTests()
    {
        _repository = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);
    }

    [Fact]
    public async Task GetByTrainingModuleAndFinishedInPeriod_ReturnsMapped_WhenInDateRange()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var period = new PeriodDate
        {
            InitDate = new DateOnly(2024, 01, 01),
            FinalDate = new DateOnly(2024, 12, 31)
        };

        var matching1 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = new DateOnly(2024, 01, 01),
                FinalDate = new DateOnly(2024, 06, 01)
            }
        };

        var matching2 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = new DateOnly(2024, 03, 01),
                FinalDate = new DateOnly(2024, 11, 01)
            }
        };

        var outside = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = new DateOnly(2024, 01, 01),
                FinalDate = new DateOnly(2025, 01, 01)
            }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddRangeAsync(matching1, matching2, outside);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
            .Returns((AssociationTrainingModuleCollaboratorDataModel dm) =>
            {
                var mock = new Mock<IAssociationTrainingModuleCollaborator>();
                mock.SetupGet(x => x.Id).Returns(dm.Id);
                mock.SetupGet(x => x.TrainingModuleId).Returns(dm.TrainingModuleId);
                mock.SetupGet(x => x.CollaboratorId).Returns(dm.CollaboratorId);
                mock.SetupGet(x => x.PeriodDate).Returns(dm.PeriodDate);
                return mock.Object;
            });

        // Act
        var result = await _repository.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, period);

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, item =>
        {
            Assert.Equal(trainingModuleId, item.TrainingModuleId);
            Assert.InRange(item.PeriodDate.FinalDate, period.InitDate, period.FinalDate);
        });

        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetByTrainingModuleAndFinishedInPeriod_ReturnsEmpty_WhenNoneInPeriod()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var period = new PeriodDate
        {
            InitDate = new DateOnly(2024, 01, 01),
            FinalDate = new DateOnly(2024, 12, 31)
        };

        var outOfRange = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = trainingModuleId,
            PeriodDate = new PeriodDate
            {
                InitDate = new DateOnly(2023, 01, 01),
                FinalDate = new DateOnly(2023, 12, 31)
            }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(outOfRange);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, period);

        // Assert
        Assert.Empty(result);
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }

    [Fact]
    public async Task GetByTrainingModuleAndFinishedInPeriod_ReturnsEmpty_WhenTrainingModuleIdDoesNotMatch()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var unrelatedId = Guid.NewGuid();

        var period = new PeriodDate
        {
            InitDate = new DateOnly(2024, 01, 01),
            FinalDate = new DateOnly(2024, 12, 31)
        };

        var unrelated = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            TrainingModuleId = unrelatedId,
            PeriodDate = new PeriodDate
            {
                InitDate = new DateOnly(2024, 02, 01),
                FinalDate = new DateOnly(2024, 03, 01)
            }
        };

        await context.Set<AssociationTrainingModuleCollaboratorDataModel>().AddAsync(unrelated);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByTrainingModuleAndFinishedInPeriod(trainingModuleId, period);

        // Assert
        Assert.Empty(result);
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()), Times.Never);
    }
}
