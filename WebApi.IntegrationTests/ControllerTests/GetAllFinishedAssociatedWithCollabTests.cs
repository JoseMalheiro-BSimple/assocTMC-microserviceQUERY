using Application.DTO;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;
public class GetAllFinishedAssociatedWithCollabTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetAllFinishedAssociatedWithCollabTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllFinishedAssociatedWithCollab_ReturnsAssociations()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var associationId = Guid.NewGuid();

        // Define a period that includes the finished date
        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
        );

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            // Seed collaborator
            context.Collaborators.Add(new CollaboratorDataModel { Id = collaboratorId });

            // Seed training module
            context.TrainingModules.Add(new TrainingModuleDataModel { Id = trainingModuleId });

            // Seed association with finished date within the period
            context.AssociationTrainingModuleCollaborators.Add(new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = associationId,
                CollaboratorId = collaboratorId,
                TrainingModuleId = trainingModuleId,
                PeriodDate = period
            });

            await context.SaveChangesAsync();
        }

        var url = $"api/associationsTMC/by-collaborator/{collaboratorId}/finished" +
                  $"?periodDate.StartDate={period.InitDate.ToString("yyyy-MM-dd")}" +
                  $"&periodDate.EndDate={period.FinalDate.ToString("yyyy-MM-dd")}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(url);

        // Assert
        Assert.NotNull(response);
        var list = response.ToList();
        Assert.Single(list);
        Assert.Equal(collaboratorId, list[0].CollaboratorId);
        Assert.Equal(trainingModuleId, list[0].TrainingModuleId);
        Assert.Equal(period, list[0].PeriodDate);
    }

    [Fact]
    public async Task GetAllFinishedAssociatedWithCollab_ReturnsEmpty_WhenNoMatch()
    {
        var randomId = Guid.NewGuid();
        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1))
        );

        var url = $"api/associationsTMC/by-collaborator/{randomId}/finished" +
                  $"?periodDate.StartDate={period.FinalDate:yyyy-MM-dd}" +
                  $"&periodDate.EndDate={period.InitDate:yyyy-MM-dd}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(url);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
