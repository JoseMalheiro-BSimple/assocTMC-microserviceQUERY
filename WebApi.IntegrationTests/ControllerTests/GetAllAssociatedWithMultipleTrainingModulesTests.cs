using Application.DTO;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class GetAllAssociatedWithMultipleTrainingModulesTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetAllAssociatedWithMultipleTrainingModulesTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllAssociatedWithMultipleTrainingModules_ReturnsAssociations()
    {
        // Arrange
        var trainingModuleId1 = Guid.NewGuid();
        var trainingModuleId2 = Guid.NewGuid();
        var collaboratorId1 = Guid.NewGuid();
        var collaboratorId2 = Guid.NewGuid();
        var associationId1 = Guid.NewGuid();
        var associationId2 = Guid.NewGuid();

        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3))
        );

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            context.TrainingModules.AddRange(new[]
            {
                new TrainingModuleDataModel { Id = trainingModuleId1 },
                new TrainingModuleDataModel { Id = trainingModuleId2 }
            });

            context.Collaborators.AddRange(new[]
            {
                new CollaboratorDataModel { Id = collaboratorId1 },
                new CollaboratorDataModel { Id = collaboratorId2 }
            });

            context.AssociationTrainingModuleCollaborators.Add(
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = associationId1,
                    CollaboratorId = collaboratorId1,
                    TrainingModuleId = trainingModuleId1,
                    PeriodDate = new PeriodDate(
                        DateOnly.FromDateTime(DateTime.UtcNow),
                        DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
                    )
                });

            context.AssociationTrainingModuleCollaborators.Add(
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = associationId2,
                    CollaboratorId = collaboratorId2,
                    TrainingModuleId = trainingModuleId2,
                    PeriodDate = new PeriodDate(
                        DateOnly.FromDateTime(DateTime.UtcNow),
                        DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
                    )
                });

            await context.SaveChangesAsync();
        }

        var query = $"api/associationsTMC/by-multiple-trainingModules?trainingModuleIds={trainingModuleId1}&trainingModuleIds={trainingModuleId2}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(query);

        // Assert
        Assert.NotNull(response);
        var list = response.ToList();
        Assert.Equal(2, list.Count);

        Assert.Contains(list, a => a.TrainingModuleId == trainingModuleId1 && a.CollaboratorId == collaboratorId1);
        Assert.Contains(list, a => a.TrainingModuleId == trainingModuleId2 && a.CollaboratorId == collaboratorId2);
    }

    [Fact]
    public async Task GetAllAssociatedWithMultipleTrainingModules_ReturnsEmpty_WhenNoAssociations()
    {
        var randomIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var query = $"api/associationsTMC/by-multiple-trainingModules?trainingModuleIds={randomIds[0]}&trainingModuleIds={randomIds[1]}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(query);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
