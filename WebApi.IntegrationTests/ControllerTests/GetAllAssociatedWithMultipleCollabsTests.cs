using Application.DTO;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;
public class GetAllAssociatedWithMultipleCollabsTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetAllAssociatedWithMultipleCollabsTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllAssociatedWithMultipleCollabs_ReturnsAssociations()
    {
        // Arrange
        var collaboratorId1 = Guid.NewGuid();
        var collaboratorId2 = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        var associationId1 = Guid.NewGuid();
        var associationId2 = Guid.NewGuid();

        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
        );

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            // Seed collaborators
            context.Collaborators.AddRange(
                new CollaboratorDataModel { Id = collaboratorId1 },
                new CollaboratorDataModel { Id = collaboratorId2 }
            );

            // Seed training module
            context.TrainingModules.Add(new TrainingModuleDataModel { Id = trainingModuleId });

            // Seed associations
            context.AssociationTrainingModuleCollaborators.AddRange(
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = associationId1,
                    CollaboratorId = collaboratorId1,
                    TrainingModuleId = trainingModuleId,
                    PeriodDate = period
                },
                new AssociationTrainingModuleCollaboratorDataModel
                {
                    Id = associationId2,
                    CollaboratorId = collaboratorId2,
                    TrainingModuleId = trainingModuleId,
                    PeriodDate = period
                }
            );

            await context.SaveChangesAsync();
        }

        // Compose query string with multiple collaborator IDs
        var url = $"api/associationsTMC/by-multiple-collaborators?collabIds={collaboratorId1}&collabIds={collaboratorId2}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(url);

        // Assert
        Assert.NotNull(response);
        var list = response.ToList();
        Assert.Equal(2, list.Count);

        Assert.Contains(list, a => a.CollaboratorId == collaboratorId1);
        Assert.Contains(list, a => a.CollaboratorId == collaboratorId2);
    }

    [Fact]
    public async Task GetAllAssociatedWithMultipleCollabs_ReturnsEmpty_WhenNoMatch()
    {
        var randomIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var url = $"api/associationsTMC/by-multiple-collaborators?collabIds={randomIds[0]}&collabIds={randomIds[1]}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(url);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}

