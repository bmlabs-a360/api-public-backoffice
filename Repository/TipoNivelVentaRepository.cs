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
    public interface ITipoNivelVentaRepository : IRepository<TipoNivelVenta>
    {
        Task<TipoNivelVenta> GetTipoNivelVentaById(TipoNivelVenta TipoNivelVenta);
        Task<IEnumerable<TipoNivelVenta>> GetTipoNivelVentas();
    }
    public class TipoNivelVentaRepository : Repository<TipoNivelVenta, Context>, ITipoNivelVentaRepository
    {
        public TipoNivelVentaRepository(Context context) : base(context) { }
        public async Task<TipoNivelVenta> GetTipoNivelVentaById(TipoNivelVenta TipoNivelVenta)
        {
            if (string.IsNullOrEmpty(TipoNivelVenta.Id.ToString())) throw new ArgumentNullException("TipoNivelVentaId");
            var retorno = await Context()
                            .TipoNivelVenta
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoNivelVenta.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoNivelVenta>> GetTipoNivelVentas()
        {
            var retorno = await  Context()
                            .TipoNivelVenta
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
