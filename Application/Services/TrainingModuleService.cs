﻿using Application.DTO;
using Application.IServices;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;
public class TrainingModuleService : ITrainingModuleService
{
    public ITrainingModuleRepository _tmRepository { get; set; }
    public ITrainingModuleFactory _tmFactory { get; set; }

    public TrainingModuleService(ITrainingModuleRepository trainingModuleRepository, ITrainingModuleFactory trainingModuleFactory)
    {
        _tmRepository = trainingModuleRepository;
        _tmFactory = trainingModuleFactory;
    }

    public async Task SubmitAsync(CreateTrainingModuleDTO createDTO)
    {
        ITrainingModule trainingModule;

        trainingModule = _tmFactory.Create(createDTO.Id);
        trainingModule = await _tmRepository.AddAsync(trainingModule);

        if (trainingModule == null)
            throw new Exception("An error as occured!");
    }
}
