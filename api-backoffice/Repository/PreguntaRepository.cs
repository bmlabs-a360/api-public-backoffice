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
    public interface IPreguntaRepository : IRepository<Pregunta>
    {
        Task<Pregunta> GetPreguntaById(Pregunta Pregunta);
        Task<IEnumerable<Pregunta>> GetPreguntas();
        Task<IEnumerable<Pregunta>> GetPreguntasByEvaluacionId(Evaluacion evaluacion);
        Task<IEnumerable<Pregunta>> GetPreguntasBySegmentacionAreaId(SegmentacionArea segmentacionArea);
        Task<IEnumerable<Pregunta>> GetPreguntasBySegmentacionSubAreaId(SegmentacionSubArea segmentacionSubArea);
    }
    public class PreguntaRepository : Repository<Pregunta, Context>, IPreguntaRepository
    {
        public PreguntaRepository(Context context) : base(context) { }
        public async Task<Pregunta> GetPreguntaById(Pregunta Pregunta)
        {
            if (string.IsNullOrEmpty(Pregunta.Id.ToString())) throw new ArgumentNullException("PreguntaId");
            var retorno = await Context()
                            .Pregunta
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Pregunta.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Pregunta>> GetPreguntas()
        {
            var retorno = await  Context()
                            .Pregunta
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Pregunta>> GetPreguntasByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .Pregunta.Where(y => y.EvaluacionId == evaluacion.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Pregunta>> GetPreguntasBySegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .Pregunta.Where(y => y.SegmentacionAreaId == segmentacionArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Pregunta>> GetPreguntasBySegmentacionSubAreaId(SegmentacionSubArea segmentacionSubArea)
        {
            var retorno = await Context()
                            .Pregunta.Where(y => y.SegmentacionSubAreaId == segmentacionSubArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
