using Application.DTO;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;
public class GetAllAssociatedWithCollabTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetAllAssociatedWithCollabTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllAssociatedWithCollab_ReturnsAssociations()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var associationId = Guid.NewGuid();

        var period = new PeriodDate(
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2))
        );

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            context.Collaborators.Add(new CollaboratorDataModel { Id = collaboratorId });
            context.TrainingModules.Add(new TrainingModuleDataModel { Id = trainingModuleId });

            context.AssociationTrainingModuleCollaborators.Add(new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = associationId,
                CollaboratorId = collaboratorId,
                TrainingModuleId = trainingModuleId,
                PeriodDate = period
            });

            await context.SaveChangesAsync();
        }

        var url = $"api/associationsTMC/by-collaborator/{collaboratorId}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(url);

        // Assert
        Assert.NotNull(response);
        var list = response.ToList();
        Assert.Single(list);

        var assoc = list[0];
        Assert.Equal(collaboratorId, assoc.CollaboratorId);
        Assert.Equal(trainingModuleId, assoc.TrainingModuleId);
        Assert.Equal(period.InitDate, assoc.PeriodDate.InitDate);
        Assert.Equal(period.FinalDate, assoc.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task GetAllAssociatedWithCollab_ReturnsEmpty_WhenNoAssociations()
    {
        var randomCollabId = Guid.NewGuid();

        var url = $"api/associationsTMC/by-collaborator/{randomCollabId}";

        // Act
        var response = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(url);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
