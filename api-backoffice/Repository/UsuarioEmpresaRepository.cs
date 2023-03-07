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
    public interface IUsuarioEmpresaRepository : IRepository<UsuarioEmpresa>
    {
        Task<UsuarioEmpresa> GetUsuarioEmpresaById(UsuarioEmpresa usuarioEmpresa);
        Task<IEnumerable<UsuarioEmpresa>> GetUsuarioEmpresas();
        Task<IEnumerable<UsuarioEmpresa>> GetUsuarioEmpresasByUsuarioId(Usuario usuario);
        Task<int> DeleteByUsuarioId(Usuario usuario);

    }
    public class UsuarioEmpresaRepository : Repository<UsuarioEmpresa, Context>, IUsuarioEmpresaRepository
    {
        public UsuarioEmpresaRepository(Context context) : base(context) { }

        public async Task<UsuarioEmpresa> GetUsuarioEmpresaById(UsuarioEmpresa usuarioEmpresa)
        {
            if (string.IsNullOrEmpty(usuarioEmpresa.Id.ToString())) throw new ArgumentNullException("UsuarioEmpresaId");
            var retorno = await Context()
                            .UsuarioEmpresas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == usuarioEmpresa.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<UsuarioEmpresa>> GetUsuarioEmpresas()
        {
            var retorno = await Context()
                            .UsuarioEmpresas
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<UsuarioEmpresa>> GetUsuarioEmpresasByUsuarioId(Usuario usuario)
        {
            var retorno = await Context()
                            .UsuarioEmpresas.Where(x => x.UsuarioId == usuario.Id && x.Activo.Value)
                            .ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<int> DeleteByUsuarioId(Usuario usuario)
        {
            try
            {
                return await Context().UsuarioEmpresas.Where(x => x.UsuarioId == usuario.Id).DeleteFromQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
