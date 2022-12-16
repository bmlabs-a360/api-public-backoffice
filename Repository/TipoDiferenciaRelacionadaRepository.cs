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
    public interface ITipoDiferenciaRelacionadaRepository : IRepository<TipoDiferenciaRelacionada>
    {
        Task<TipoDiferenciaRelacionada> GetTipoDiferenciaRelacionadaById(TipoDiferenciaRelacionada TipoDiferenciaRelacionada);
        Task<IEnumerable<TipoDiferenciaRelacionada>> GetTipoDiferenciaRelacionadas();
    }
    public class TipoDiferenciaRelacionadaRepository : Repository<TipoDiferenciaRelacionada, Context>, ITipoDiferenciaRelacionadaRepository
    {
        public TipoDiferenciaRelacionadaRepository(Context context) : base(context) { }
        public async Task<TipoDiferenciaRelacionada> GetTipoDiferenciaRelacionadaById(TipoDiferenciaRelacionada TipoDiferenciaRelacionada)
        {
            if (string.IsNullOrEmpty(TipoDiferenciaRelacionada.Id.ToString())) throw new ArgumentNullException("TipoDiferenciaRelacionadaId");
            var retorno = await Context()
                            .TipoDiferenciaRelacionada
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoDiferenciaRelacionada.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoDiferenciaRelacionada>> GetTipoDiferenciaRelacionadas()
        {
            var retorno = await  Context()
                            .TipoDiferenciaRelacionada
                            .AsNoTracking().Where(x=>x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
