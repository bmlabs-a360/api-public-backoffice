using api_public_backOffice.Models;
using neva.entities;
using neva.Repository.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api_public_backOffice.Repository
{
    public interface IPlanMejoraRepository : IRepository<PlanMejora>
    {
        Task<PlanMejora> GetPlanMejoraById(PlanMejora PlanMejora);
        Task<IEnumerable<PlanMejora>> GetPlanMejoras();
        Task<IEnumerable<PlanMejora>> GetPlanMejorasByPreguntaId(Pregunta pregunta);
    }
    public class PlanMejoraRepository : Repository<PlanMejora, Context>, IPlanMejoraRepository
    {
        public PlanMejoraRepository(Context context) : base(context) { }
        public async Task<PlanMejora> GetPlanMejoraById(PlanMejora PlanMejora)
        {
            if (string.IsNullOrEmpty(PlanMejora.Id.ToString())) throw new ArgumentNullException("PlanMejoraId");
            var retorno = await Context()
                            .PlanMejoras
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == PlanMejora.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<PlanMejora>> GetPlanMejoras()
        {
            var retorno = await  Context()
                            .PlanMejoras
                            .AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<PlanMejora>> GetPlanMejorasByPreguntaId(Pregunta pregunta)
        {
            var retorno = await Context()
                            .PlanMejoras.Where(y => y.PreguntaId == pregunta.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<PlanMejora>> GetPlanMejorasBySegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .PlanMejoras.Where(y => y.SegmentacionAreaId == segmentacionArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<PlanMejora>> GetPlanMejorasBySegmentacionSubAreaId(SegmentacionSubArea segmentacionSubArea)
        {
            var retorno = await Context()
                            .PlanMejoras.Where(y => y.SegmentacionSubAreaId == segmentacionSubArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<PlanMejora>> GetPlanMejorasBySegmentacionSubAreaId(Alternativa alternativa)
        {
            var retorno = await Context()
                            .PlanMejoras.Where(y => y.AlternativaId == alternativa.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
