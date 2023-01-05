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
    public interface IPerfilRepository : IRepository<Perfil>
    {
        Task<Perfil> GetPerfilByName(string nombre);

        Task<Perfil> GetPerfilById(Perfil Perfil);
        Task<IEnumerable<Perfil>> GetPerfils();
    }
    public class PerfilRepository : Repository<Perfil, Context>, IPerfilRepository
    {
        public PerfilRepository(Context context) : base(context) { }
        public async Task<Perfil> GetPerfilByName(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) throw new ArgumentNullException("Perfil");
            var retorno = await Context()
                            .Perfils
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Nombre == nombre);

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<Perfil>> GetPerfils()
        {
            var retorno = await Context()
                            .Perfils.Include(x => x.PerfilPermisos)
                            .AsNoTracking().ToListAsync();

           

            if (retorno == null) return null;
            return retorno;
        }


        public async Task<Perfil> GetPerfilById(Perfil Perfil)
        {
            if (string.IsNullOrEmpty(Perfil.Id.ToString())) throw new ArgumentNullException("PerfilId");
            var retorno = await Context()
                            .Perfils
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Perfil.Id   );

            if (retorno == null) return null;
            return retorno; 
        }

  



    }
}
