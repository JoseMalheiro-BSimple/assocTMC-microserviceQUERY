﻿using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.AssociationTrainingModuleCollaboratorTests;
public class AssociationTrainingModuleCollaboratorFactoryTests
{
    [Fact]
    public async Task WhenPassingValidData_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(collab.Object);

        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        tmRepo.Setup(tmr => tmr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(tm.Object);

        // Example valid dates
        DateOnly initDate = new DateOnly();
        DateOnly endDate = initDate.AddDays(1);

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object);

        // Act
        var assocTMC = await assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), initDate, endDate);

        // Assert
        Assert.NotNull(assocTMC);
    }

    [Fact]
    public async Task WhenPassingInvalidCollabId_ThenThrowsArgumentException()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();

        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        tmRepo.Setup(tmr => tmr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(tm.Object);

        // Example valid dates
        DateOnly initDate = new DateOnly();
        DateOnly endDate = initDate.AddDays(1);

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object);


        // Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), initDate, endDate)
        );

        Assert.Equal("Collaborator must exists", exception.Message);
    }

    [Fact]
    public async Task WhenPassingInvalidTrainingModuleId_ThenThrowsArgumentException()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(collab.Object);

        // Example valid dates
        DateOnly initDate = new DateOnly();
        DateOnly endDate = initDate.AddDays(1);

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object);

        // Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), initDate, endDate)
        );

        Assert.Equal("TrainingModule must exist", exception.Message);
    }

    [Fact]
    public void WhenPassingValidatedData_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object);

        // Act
        var result = assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange 
        Mock<IAssociationTrainingModuleCollaboratorVisitor> visitor = new Mock<IAssociationTrainingModuleCollaboratorVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.CollaboratorId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.TrainingModuleId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.PeriodDate).Returns(It.IsAny<PeriodDate>());

        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object);

        // Act
        var assocTMC = assocTMCFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(assocTMC);
    }

}
