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
    public interface IReporteItemNivelSubscripcionRepository : IRepository<ReporteItemNivelSubscripcion>
    {
        Task<ReporteItemNivelSubscripcion> GetReporteItemNivelSubscripcionById(ReporteItemNivelSubscripcion ReporteItemNivelSubscripcion);
        Task<IEnumerable<ReporteItemNivelSubscripcion>> GetReporteItemNivelSubscripcions();
        Task<IEnumerable<ReporteItemNivelSubscripcion>> GetReporteItemNivelSubscripcionsByReporteId(Reporte reporte);
    }
    public class ReporteItemNivelSubscripcionRepository : Repository<ReporteItemNivelSubscripcion, Context>, IReporteItemNivelSubscripcionRepository
    {
        public ReporteItemNivelSubscripcionRepository(Context context) : base(context) { }
        public async Task<ReporteItemNivelSubscripcion> GetReporteItemNivelSubscripcionById(ReporteItemNivelSubscripcion ReporteItemNivelSubscripcion)
        {
            if (string.IsNullOrEmpty(ReporteItemNivelSubscripcion.Id.ToString())) throw new ArgumentNullException("ReporteItemNivelSubscripcionId");
            var retorno = await Context()
                            .ReporteItemNivelSubscripcions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ReporteItemNivelSubscripcion.Id);

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteItemNivelSubscripcion>> GetReporteItemNivelSubscripcions()
        {
            var retorno = await Context()
                            .ReporteItemNivelSubscripcions
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteItemNivelSubscripcion>> GetReporteItemNivelSubscripcionsByReporteId(Reporte reporte)
        {
            var retorno = await Context()
                            .ReporteItemNivelSubscripcions.Where(y => y.ReporteId == reporte.Id).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
