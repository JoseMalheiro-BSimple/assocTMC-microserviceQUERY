using Application.DTO;
using Domain.Models;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class GetByCollabAndTrainingModuleTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public GetByCollabAndTrainingModuleTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllByCollabAndTrainingModule_ReturnsMatchingAssociation()
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

            context.TrainingModules.Add(new TrainingModuleDataModel { Id = trainingModuleId });
            context.Collaborators.Add(new CollaboratorDataModel { Id = collaboratorId });
            context.AssociationTrainingModuleCollaborators.Add(new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = associationId,
                TrainingModuleId = trainingModuleId,
                CollaboratorId = collaboratorId,
                PeriodDate = period
            });

            await context.SaveChangesAsync();
        }

        // Act
        var result = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(
            $"/api/associationsTMC?collabId={collaboratorId}&trainingModuleId={trainingModuleId}");

        // Assert
        var list = result.ToList();
        Assert.Single(list);

        var assoc = list[0];
        Assert.Equal(trainingModuleId, assoc.TrainingModuleId);
        Assert.Equal(collaboratorId, assoc.CollaboratorId);
        Assert.Equal(period.InitDate, assoc.PeriodDate.InitDate);
        Assert.Equal(period.FinalDate, assoc.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task GetAllByCollabAndTrainingModule_ReturnsEmpty_WhenNoMatch()
    {
        // Act
        var result = await GetAndDeserializeAsync<IEnumerable<AssociationTrainingModuleCollaboratorDTO>>(
            $"api/associationsTMC?collabId={Guid.NewGuid()}&trainingModuleId={Guid.NewGuid()}");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
