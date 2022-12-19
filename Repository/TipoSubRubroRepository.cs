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
    public interface ITipoSubRubroRepository : IRepository<TipoSubRubro>
    {
        Task<List<TipoSubRubro>> GetTipoSubRubroByIdRubro(Guid TipoRubroId);
        Task<TipoSubRubro> GetTipoSubRubroById(TipoSubRubro TipoSubRubro);
        Task<IEnumerable<TipoSubRubro>> GetTipoSubRubros();
    }
    public class TipoSubRubroRepository : Repository<TipoSubRubro, Context>, ITipoSubRubroRepository
    {
        public TipoSubRubroRepository(Context context) : base(context) { }
        public async Task<List<TipoSubRubro>> GetTipoSubRubroByIdRubro(Guid TipoRubroId)
        {
            if (string.IsNullOrEmpty(TipoRubroId.ToString())) throw new ArgumentNullException("TipoRubroId");
            var retorno = await Context()
                            .TipoSubRubros
                            .AsNoTracking()
                            .Where(x => x.TipoRubroId == TipoRubroId && x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<TipoSubRubro> GetTipoSubRubroById(TipoSubRubro TipoSubRubro)
        {
            if (string.IsNullOrEmpty(TipoSubRubro.Id.ToString())) throw new ArgumentNullException("TipoSubRubroId");
            var retorno = await Context()
                            .TipoSubRubros
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoSubRubro.Id && x.Activo.Value);

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<TipoSubRubro>> GetTipoSubRubros()
        {
            var retorno = await Context()
                            .TipoSubRubros
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();
            if (retorno == null) return null;
            return retorno;
        }
    }
}
