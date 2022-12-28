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
    public interface ITipoRubroRepository : IRepository<TipoRubro>
    {
        Task<TipoRubro> GetTipoRubroById(TipoRubro TipoRubro);
        Task<IEnumerable<TipoRubro>> GetTipoRubros();
    }
    public class TipoRubroRepository : Repository<TipoRubro, Context>, ITipoRubroRepository
    {
        public TipoRubroRepository(Context context) : base(context) { }
        public async Task<TipoRubro> GetTipoRubroById(TipoRubro TipoRubro)
        {
            if (string.IsNullOrEmpty(TipoRubro.Id.ToString())) throw new ArgumentNullException("TipoRubroId");
            var retorno = await Context()
                            .TipoRubros
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoRubro.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoRubro>> GetTipoRubros()
        {
            var retorno = await  Context()
                            .TipoRubros
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
