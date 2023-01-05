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
    public interface ITipoCantidadEmpleadoRepository : IRepository<TipoCantidadEmpleado>
    {
        Task<TipoCantidadEmpleado> GetTipoCantidadEmpleadoById(TipoCantidadEmpleado TipoCantidadEmpleado);
        Task<IEnumerable<TipoCantidadEmpleado>> GetTipoCantidadEmpleados();
    }
    public class TipoCantidadEmpleadoRepository : Repository<TipoCantidadEmpleado, Context>, ITipoCantidadEmpleadoRepository
    {
        public TipoCantidadEmpleadoRepository(Context context) : base(context) { }
        public async Task<TipoCantidadEmpleado> GetTipoCantidadEmpleadoById(TipoCantidadEmpleado TipoCantidadEmpleado)
        {
            if (string.IsNullOrEmpty(TipoCantidadEmpleado.Id.ToString())) throw new ArgumentNullException("TipoCantidadEmpleadoId");
            var retorno = await Context()
                            .TipoCantidadEmpleados
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == TipoCantidadEmpleado.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<TipoCantidadEmpleado>> GetTipoCantidadEmpleados()
        {
            var retorno = await  Context()
                            .TipoCantidadEmpleados
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    
    }
}
