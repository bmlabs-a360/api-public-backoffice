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
    public interface IAlternativaRepository : IRepository<Alternativa>
    {
        Task<Alternativa> GetAlternativaById(Alternativa Alternativa);
        Task<IEnumerable<Alternativa>> GetAlternativaByEvaluacionId(Alternativa alternativa);
        Task<IEnumerable<Alternativa>> GetAlternativaByPreguntaId(Pregunta pregunta);
    }
    public class AlternativaRepository : Repository<Alternativa, Context>, IAlternativaRepository
    {
        public AlternativaRepository(Context context) : base(context) { }
        public async Task<Alternativa> GetAlternativaById(Alternativa alternativa)
        {
            if (string.IsNullOrEmpty(alternativa.Id.ToString())) throw new ArgumentNullException("AlternativaId");
            var retorno = await Context()
                            .Alternativas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == alternativa.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Alternativa>> GetAlternativaByEvaluacionId(Alternativa alternativa)
        {
            if (string.IsNullOrEmpty(alternativa.EvaluacionId.ToString())) throw new ArgumentNullException("EvaluacionId");
            var retorno = await Context()
                            .Alternativas
                            .AsNoTracking()
                            .Where(x => x.EvaluacionId == alternativa.EvaluacionId  ).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Alternativa>> GetAlternativaByPreguntaId(Pregunta pregunta)
        {
            if (string.IsNullOrEmpty(pregunta.Id.ToString())) throw new ArgumentNullException("PreguntaId");
            var retorno = await Context()
                            .Alternativas
                            .AsNoTracking()
                            .Where(x => x.PreguntaId == pregunta.Id  ).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
