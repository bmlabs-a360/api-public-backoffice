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
    public interface IReporteRepository : IRepository<Reporte>
    {
        Task<Reporte> GetReporteById(Reporte Reporte);
        Task<IEnumerable<Reporte>> GetReportes();
        Task<IEnumerable<Reporte>> GetReportesByEvaluacionId(Evaluacion evaluacion);
    }
    public class ReporteRepository : Repository<Reporte, Context>, IReporteRepository
    {
        public ReporteRepository(Context context) : base(context) { }
        public async Task<Reporte> GetReporteById(Reporte Reporte)
        {
            if (string.IsNullOrEmpty(Reporte.Id.ToString())) throw new ArgumentNullException("ReporteId");
            var retorno = await Context()
                            .Reportes
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Reporte.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Reporte>> GetReportes()
        {
            var retorno = await  Context()
                            .Reportes
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<Reporte>> GetReportesByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .Reportes
                            .Include(x => x.ReporteItemNivelBasicos.OrderByDescending(x => x.Orden))
                            .Where(y => y.EvaluacionId == evaluacion.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
