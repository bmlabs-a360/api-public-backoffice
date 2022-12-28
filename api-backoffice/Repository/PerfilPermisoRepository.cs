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
    public interface IPerfilPermisoRepository : IRepository<PerfilPermiso>
    {
        Task<PerfilPermiso> GetPerfilPermisoById(PerfilPermiso PerfilPermiso);
        Task<IEnumerable<PerfilPermiso>> GetPerfilPermisos();
        Task<IEnumerable<PerfilPermiso>> GetGetPerfilPermisosByPerfilId(Perfil perfil);
    }
    public class PerfilPermisoRepository : Repository<PerfilPermiso, Context>, IPerfilPermisoRepository
    {
        public PerfilPermisoRepository(Context context) : base(context) { }
        public async Task<PerfilPermiso> GetPerfilPermisoById(PerfilPermiso PerfilPermiso)
        {
            if (string.IsNullOrEmpty(PerfilPermiso.Id.ToString())) throw new ArgumentNullException("PerfilPermisoId");
            var retorno = await Context()
                            .PerfilPermisos
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == PerfilPermiso.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<PerfilPermiso>> GetPerfilPermisos()
        {
            var retorno = await  Context()
                            .PerfilPermisos
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<PerfilPermiso>> GetGetPerfilPermisosByPerfilId(Perfil perfil)
        {
            var retorno = await Context()
                            .PerfilPermisos.Where(y => y.PerfilId == perfil.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }





    }
}
