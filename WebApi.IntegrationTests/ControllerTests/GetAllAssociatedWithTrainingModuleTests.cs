using Application.DTO;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;
public class GetAllAssociatedWithTrainingModuleTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetAllAssociatedWithTrainingModuleTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllAssociatedWithTrainingModule_ReturnsAssociations()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var associationId = Guid.NewGuid();

        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3))
        );

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            context.TrainingModules.Add(new TrainingModuleDataModel
            {
                Id = trainingModuleId
            });

            context.Collaborators.Add(new CollaboratorDataModel
            {
                Id = collaboratorId
            });

            context.AssociationTrainingModuleCollaborators.Add(new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = associationId,
                CollaboratorId = collaboratorId,
                TrainingModuleId = trainingModuleId,
                PeriodDate = period
            });

            await context.SaveChangesAsync();
        }

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>($"api/associationsTMC/by-trainingModule/{trainingModuleId}");

        // Assert
        Assert.NotNull(response);
        var list = response.ToList();
        Assert.Single(list);
        Assert.Equal(trainingModuleId, list[0].TrainingModuleId);
        Assert.Equal(collaboratorId, list[0].CollaboratorId);
        Assert.Equal(period, list[0].PeriodDate);
    }

    [Fact]
    public async Task GetAllAssociatedWithTrainingModule_ReturnsEmpty_WhenNoAssociations()
    {
        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>($"api/associationsTMC/by-trainingModule/{Guid.NewGuid()}");

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
