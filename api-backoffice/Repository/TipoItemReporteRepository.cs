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
    public interface ITipoItemReporteRepository : IRepository<TipoItemReporte>
    {
        Task<TipoItemReporte> GetTipoItemReporteById(TipoItemReporte TipoItemReporte);
        Task<IEnumerable<TipoItemReporte>> GetTipoItemReportes();
    }
    public class TipoItemReporteRepository : Repository<TipoItemReporte, Context>, ITipoItemReporteRepository
    {
        public TipoItemReporteRepository(Context context) : base(context) { }
        public async Task<TipoItemReporte> GetTipoItemReporteById(TipoItemReporte TipoItemReporte)
        {
            if (string.IsNullOrEmpty(TipoItemReporte.Id.ToString())) throw new ArgumentNullException("TipoItemReporteId");
            var retorno = await Context()
                            .TipoItemReportes
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoItemReporte.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoItemReporte>> GetTipoItemReportes()
        {
            var retorno = await  Context()
                            .TipoItemReportes
                            .AsNoTracking().Where(x => x.Activo.Value).OrderBy(x => x.Orden).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
