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
    public interface ITipoTamanoEmpresaRepository : IRepository<TipoTamanoEmpresa>
    {
        Task<TipoTamanoEmpresa> GetTipoTamanoEmpresaById(TipoTamanoEmpresa TipoTamanoEmpresa);
        Task<IEnumerable<TipoTamanoEmpresa>> GetTipoTamanoEmpresas();
    }
    public class TipoTamanoEmpresaRepository : Repository<TipoTamanoEmpresa, Context>, ITipoTamanoEmpresaRepository
    {
        public TipoTamanoEmpresaRepository(Context context) : base(context) { }
        public async Task<TipoTamanoEmpresa> GetTipoTamanoEmpresaById(TipoTamanoEmpresa TipoTamanoEmpresa)
        {
            if (string.IsNullOrEmpty(TipoTamanoEmpresa.Id.ToString())) throw new ArgumentNullException("TipoTamanoEmpresaId");
            var retorno = await Context()
                            .TipoTamanoEmpresas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoTamanoEmpresa.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoTamanoEmpresa>> GetTipoTamanoEmpresas()
        {
            var retorno = await  Context()
                            .TipoTamanoEmpresas
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
