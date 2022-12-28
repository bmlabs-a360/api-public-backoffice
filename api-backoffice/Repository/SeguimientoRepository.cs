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
    public interface ISeguimientoRepository : IRepository<Seguimiento>
    {
        Task<Seguimiento> GetSeguimientoById(Seguimiento Seguimiento);
        Task<IEnumerable<Seguimiento>> GetSeguimientos();
        Task<IEnumerable<Seguimiento>> GetSeguimientosByEmpresaId(Empresa empresa);
        Task<IEnumerable<Seguimiento>> GetSeguimientosByEvaluacionId(Evaluacion evaluacion);
        Task<IEnumerable<Seguimiento>> GetSeguimientosByPlanMejoraId(PlanMejora planMejora);
    }
    public class SeguimientoRepository : Repository<Seguimiento, Context>, ISeguimientoRepository
    {
        public SeguimientoRepository(Context context) : base(context) { }
        public async Task<Seguimiento> GetSeguimientoById(Seguimiento Seguimiento)
        {
            if (string.IsNullOrEmpty(Seguimiento.Id.ToString())) throw new ArgumentNullException("SeguimientoId");
            var retorno = await Context()
                            .Seguimientos
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Seguimiento.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Seguimiento>> GetSeguimientos()
        {
            var retorno = await  Context()
                            .Seguimientos
                            .AsNoTracking().Where(x=>x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Seguimiento>> GetSeguimientosByEmpresaId(Empresa empresa)
        {
            var retorno = await Context()
                            .Seguimientos.Where(y => y.EmpresaId == empresa.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Seguimiento>> GetSeguimientosByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .Seguimientos.Where(y => y.EvaluacionId == evaluacion.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Seguimiento>> GetSeguimientosByPlanMejoraId(PlanMejora planMejora)
        {
            var retorno = await Context()
                            .Seguimientos.Where(y => y.PlanMejoraId == planMejora.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
