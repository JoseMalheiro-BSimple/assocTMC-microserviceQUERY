using AutoMapper;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<AssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>();
        CreateMap<AssociationTrainingModuleCollaboratorDataModel, AssociationTrainingModuleCollaborator>()
            .ConvertUsing<AssociationTrainingModuleCollaboratorDataModelConverter>();
        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, Collaborator>()
            .ConvertUsing<CollaboratorDataModelConverter>();
        CreateMap<TrainingModule, TrainingModuleDataModel>();
        CreateMap<TrainingModuleDataModel, TrainingModule>()
            .ConvertUsing<TrainingModuleDataModelConverter>();
    }
}