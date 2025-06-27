using Application.DTO;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;
public class GetAllWithFinishedTrainingModuleTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetAllWithFinishedTrainingModuleTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllWithFinishedTrainingModule_ReturnsAssociations()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var associationId = Guid.NewGuid();

        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10))
        );

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            context.TrainingModules.Add(new TrainingModuleDataModel { Id = trainingModuleId });
            context.Collaborators.Add(new CollaboratorDataModel { Id = collaboratorId });

            context.AssociationTrainingModuleCollaborators.Add(new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = associationId,
                CollaboratorId = collaboratorId,
                TrainingModuleId = trainingModuleId,
                PeriodDate = period
            });

            await context.SaveChangesAsync();
        }

        var query = $"api/associationsTMC/by-trainingModule/{trainingModuleId}/finished" +
                    $"?periodDate.StartDate={period.InitDate:yyyy-MM-dd}&periodDate.EndDate={period.FinalDate:yyyy-MM-dd}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(query);

        // Assert
        Assert.NotNull(response);
        var list = response.ToList();
        Assert.Single(list);

        var assoc = list[0];
        Assert.Equal(trainingModuleId, assoc.TrainingModuleId);
        Assert.Equal(collaboratorId, assoc.CollaboratorId);
        Assert.Equal(period, assoc.PeriodDate);
    }

    [Fact]
    public async Task GetAllWithFinishedTrainingModule_ReturnsEmpty_WhenNoMatch()
    {
        var randomTrainingModuleId = Guid.NewGuid();
        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
        );

        var query = $"api/associationsTMC/by-trainingModule/{randomTrainingModuleId}/finished" +
                    $"?periodDate.StartDate={period.FinalDate:yyyy-MM-dd}&periodDate.EndDate={period.FinalDate:yyyy-MM-dd}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(query);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}

