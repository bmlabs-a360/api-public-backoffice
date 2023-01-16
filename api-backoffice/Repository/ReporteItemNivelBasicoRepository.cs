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
    public interface IReporteItemNivelBasicoRepository : IRepository<ReporteItemNivelBasico>
    {
        Task<ReporteItemNivelBasico> GetReporteItemNivelBasicoById(ReporteItemNivelBasico ReporteItemNivelBasico);
        Task<IEnumerable<ReporteItemNivelBasico>> GetReporteItemNivelBasicos();
        Task<IEnumerable<ReporteItemNivelBasico>> GetReporteItemNivelBasicosByReporteId(Reporte reporte);
    }
    public class ReporteItemNivelBasicoRepository : Repository<ReporteItemNivelBasico, Context>, IReporteItemNivelBasicoRepository
    {
        public ReporteItemNivelBasicoRepository(Context context) : base(context) { }
        public async Task<ReporteItemNivelBasico> GetReporteItemNivelBasicoById(ReporteItemNivelBasico ReporteItemNivelBasico)
        {
            if (string.IsNullOrEmpty(ReporteItemNivelBasico.Id.ToString())) throw new ArgumentNullException("ReporteItemNivelBasicoId");
            var retorno = await Context()
                            .ReporteItemNivelBasicos
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ReporteItemNivelBasico.Id);

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteItemNivelBasico>> GetReporteItemNivelBasicos()
        {
            var retorno = await Context()
                            .ReporteItemNivelBasicos
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteItemNivelBasico>> GetReporteItemNivelBasicosByReporteId(Reporte reporte)
        {
            var retorno = await Context()
                            .ReporteItemNivelBasicos.Where(y => y.ReporteId == reporte.Id).OrderBy(y => y.Orden).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
