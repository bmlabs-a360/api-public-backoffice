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
    public interface IBitacoraRepository : IRepository<Bitacora>
    {
        Task<List<Bitacora>> GetBitacorasByUsuarioId(BitacoraModel UsuarioId);
        Task<Bitacora> GetBitacoraById(Bitacora bitacora);

    }
    public class BitacoraRepository : Repository<Bitacora, Context>, IBitacoraRepository
    {
        public BitacoraRepository(Context context) : base(context) { }

        public async Task<List<Bitacora>> GetBitacorasByUsuarioId(BitacoraModel bitacoraModel)
        {
            var bitacora = await Context().Bitacoras.Where(x => x.UsuarioId.Equals(bitacoraModel.UsuarioId)).ToListAsync();
            return bitacora;
        }

        public async Task<Bitacora> GetBitacoraById(Bitacora bitacora)
        {
            if (string.IsNullOrEmpty(bitacora.Id.ToString())) throw new ArgumentNullException("BitacoraId");
            var retorno = await Context()
                            .Bitacoras
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == bitacora.Id );

            if (retorno == null) return null;
            return retorno;
        }


    }
}
