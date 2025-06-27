using Application.DTO;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using InterfaceAdapters.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString =
    Environment.GetEnvironmentVariable("CustomConnection") ??
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AssocTMCContext>(opt =>
    opt.UseNpgsql(connectionString));

//Services
builder.Services.AddTransient<IAssociationTrainingModuleCollaboratorService, AssociationTrainingModuleCollaboratorService>();
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<ITrainingModuleService, TrainingModuleService>();

//Repositories
builder.Services.AddTransient<IAssociationTrainingModuleCollaboratorsRepository, AssociationTrainingModuleCollaboratorRepositoryEF>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepositoryEF>();
builder.Services.AddTransient<ITrainingModuleRepository, TrainingModuleRepositoryEF>();

//Factories
builder.Services.AddTransient<IAssociationTrainingModuleCollaboratorFactory, AssociationTrainingModuleCollaboratorFactory>();
builder.Services.AddTransient<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddTransient<ITrainingModuleFactory, TrainingModuleFactory>();

//Mappers
builder.Services.AddTransient<AssociationTrainingModuleCollaboratorDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<TrainingModuleDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<AssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDTO>();
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AssociationTrainingModuleCollaboratorCreatedConsumer>();
    x.AddConsumer<CollaboratorCreatedConsumer>();
    x.AddConsumer<TrainingModuleCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        var random = Guid.NewGuid();
        cfg.ReceiveEndpoint($"{random}", e =>
        {
            e.ConfigureConsumer<AssociationTrainingModuleCollaboratorCreatedConsumer>(context);
            e.ConfigureConsumer<CollaboratorCreatedConsumer>(context);
            e.ConfigureConsumer<TrainingModuleCreatedConsumer>(context);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
