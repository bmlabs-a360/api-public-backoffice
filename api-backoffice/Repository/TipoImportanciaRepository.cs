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
    public interface ITipoImportanciaRepository : IRepository<TipoImportancia>
    {
        Task<TipoImportancia> GetTipoImportanciaById(TipoImportancia TipoImportancia);
        Task<IEnumerable<TipoImportancia>> GetTipoImportancias();
    }
    public class TipoImportanciaRepository : Repository<TipoImportancia, Context>, ITipoImportanciaRepository
    {
        public TipoImportanciaRepository(Context context) : base(context) { }
        public async Task<TipoImportancia> GetTipoImportanciaById(TipoImportancia TipoImportancia)
        {
            if (string.IsNullOrEmpty(TipoImportancia.Id.ToString())) throw new ArgumentNullException("TipoImportanciaId");
            var retorno = await Context()
                            .TipoImportancia
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoImportancia.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoImportancia>> GetTipoImportancias()
        {
            var retorno = await  Context()
                            .TipoImportancia
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
