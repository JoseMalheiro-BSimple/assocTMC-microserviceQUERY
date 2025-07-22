using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssociationTrainingModuleCollaboratorRepositoryEF : GenericRepositoryEF<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>, IAssociationTrainingModuleCollaboratorsRepository
    {
        private readonly IMapper _mapper;
        public AssociationTrainingModuleCollaboratorRepositoryEF(AssocTMCContext context, IMapper mapper) : base(context, mapper)
        {
            _mapper = mapper;
        }

        public override IAssociationTrainingModuleCollaborator? GetById(Guid id)
        {
            var trainingModuleCollabDM = _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                    .FirstOrDefault(t => t.Id == id);

            if (trainingModuleCollabDM == null)
                return null;

            return _mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(trainingModuleCollabDM);
        }

        public override async Task<IAssociationTrainingModuleCollaborator?> GetByIdAsync(Guid id)
        {
            var trainingModuleCollabDM = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                    .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingModuleCollabDM == null)
                return null;

            return _mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(trainingModuleCollabDM);
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollabAndTrainingModule(Guid collabId, Guid trainingModuleId)
        {
            var assocsDM = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                         .Where(a => a.CollaboratorId == collabId && a.TrainingModuleId == trainingModuleId)
                                         .ToListAsync();

            return assocsDM.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByTrainingModuleId(Guid id)
        {
            var tmCollabDMs = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                            .Where(tm => tm.TrainingModuleId == id)
                                            .ToListAsync();

            var tmCollabs = tmCollabDMs.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);

            return tmCollabs;
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByTrainingModuleIds(IEnumerable<Guid> trainingModuleIds)
        {
            var trainingModuleCollaboratorsDMs = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                                .Where(t => trainingModuleIds.Contains(t.TrainingModuleId))
                                                .ToListAsync();

            var trainingModuleCollaborators = trainingModuleCollaboratorsDMs.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);

            return trainingModuleCollaborators;
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByTrainingModuleAndFinishedInPeriod(Guid id, PeriodDate periodDate)
        {
            var tmCollabDMs = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                            .Where(tm => tm.TrainingModuleId == id && tm.PeriodDate.FinalDate >= periodDate.InitDate && tm.PeriodDate.FinalDate <= periodDate.FinalDate)
                                            .ToListAsync();
            
            var trainingModuleCollaborators = tmCollabDMs.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);

            return trainingModuleCollaborators;
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollaboratorId(Guid id)
        {
            var tmCollabDMs = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                            .Where(tm => tm.CollaboratorId == id)
                                            .ToListAsync();

            var tmCollabs = tmCollabDMs.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);

            return tmCollabs;
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollaboratorIds(IEnumerable<Guid> collabIds)
        {
            var trainingModuleCollaboratorsDMs = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                                .Where(t => collabIds.Contains(t.CollaboratorId))
                                                .ToListAsync();

            var trainingModuleCollaborators = trainingModuleCollaboratorsDMs.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);

            return trainingModuleCollaborators;
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollaboratorAndFinishedInPeriod(Guid id, PeriodDate periodDate)
        {
            var tmCollabDMs = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                            .Where(tm => tm.CollaboratorId == id && tm.PeriodDate.FinalDate >= periodDate.InitDate && tm.PeriodDate.FinalDate <= periodDate.FinalDate)
                                            .ToListAsync();

            var trainingModuleCollaborators = tmCollabDMs.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);

            return trainingModuleCollaborators;
        }

        public async Task RemoveWoTracked(IAssociationTrainingModuleCollaborator entity)
        {
            var trackedDataModel = _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                           .Local
                                           .FirstOrDefault(dm => dm.Id == entity.Id);

            if (trackedDataModel == null)
            {
                trackedDataModel = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                               .FirstOrDefaultAsync(dm => dm.Id == entity.Id);
                if (trackedDataModel == null)
                {
                    return;
                }
            }

            _context.Set<AssociationTrainingModuleCollaboratorDataModel>().Remove(trackedDataModel);

        }
    }
}
