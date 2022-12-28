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
    public interface IUsuarioSuscripcionRepository : IRepository<UsuarioSuscripcion>
    {
        Task<UsuarioSuscripcion> GetUsuarioSuscripcionById(UsuarioSuscripcion UsuarioSuscripcion);
        Task<IEnumerable<UsuarioSuscripcion>> GetUsuarioSuscripcions();
        Task<IEnumerable<UsuarioSuscripcion>> GetUsuarioSuscripcionsByUsuarioId(Usuario usuario);
    }
    public class UsuarioSuscripcionRepository : Repository<UsuarioSuscripcion, Context>, IUsuarioSuscripcionRepository
    {
        public UsuarioSuscripcionRepository(Context context) : base(context) { }
        public async Task<UsuarioSuscripcion> GetUsuarioSuscripcionById(UsuarioSuscripcion UsuarioSuscripcion)
        {
            if (string.IsNullOrEmpty(UsuarioSuscripcion.Id.ToString())) throw new ArgumentNullException("UsuarioSuscripcionId");
            var retorno = await Context()
                            .UsuarioSuscripcions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == UsuarioSuscripcion.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<UsuarioSuscripcion>> GetUsuarioSuscripcions()
        {
            var retorno = await  Context()
                            .UsuarioSuscripcions
                            .AsNoTracking().Where(x =>  x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<UsuarioSuscripcion>> GetUsuarioSuscripcionsByUsuarioId(Usuario usuario)
        {
            var retorno = await Context()
                            .UsuarioSuscripcions.Where(y => y.UsuarioId == usuario.Id &&  y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
