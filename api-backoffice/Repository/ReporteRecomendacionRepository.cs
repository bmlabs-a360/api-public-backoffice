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
    public interface IReporteRecomendacionAreaRepository : IRepository<ReporteRecomendacionArea>
    {
        Task<ReporteRecomendacionArea> GetReporteRecomendacionAreaById(ReporteRecomendacionArea ReporteRecomendacionArea);
        Task<IEnumerable<ReporteRecomendacionArea>> GetReporteRecomendacionAreas();
        Task<IEnumerable<ReporteRecomendacionArea>> GetReporteRecomendacionAreasByReporteId(Reporte reporte);
        Task<IEnumerable<ReporteRecomendacionArea>> GetReporteRecomendacionAreasBySegmentacionAreaId(SegmentacionArea segmentacionArea);
    }
    public class ReporteRecomendacionAreaRepository : Repository<ReporteRecomendacionArea, Context>, IReporteRecomendacionAreaRepository
    {
        public ReporteRecomendacionAreaRepository(Context context) : base(context) { }
        public async Task<ReporteRecomendacionArea> GetReporteRecomendacionAreaById(ReporteRecomendacionArea ReporteRecomendacionArea)
        {
            if (string.IsNullOrEmpty(ReporteRecomendacionArea.Id.ToString())) throw new ArgumentNullException("ReporteRecomendacionAreaId");
            var retorno = await Context()
                            .ReporteRecomendacionAreas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ReporteRecomendacionArea.Id);

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteRecomendacionArea>> GetReporteRecomendacionAreas()
        {
            var retorno = await Context()
                            .ReporteRecomendacionAreas
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteRecomendacionArea>> GetReporteRecomendacionAreasByReporteId(Reporte reporte)
        {
            var retorno = await Context()
                            .ReporteRecomendacionAreas.Where(y => y.ReporteId == reporte.Id).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteRecomendacionArea>> GetReporteRecomendacionAreasBySegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .ReporteRecomendacionAreas.Where(y => y.SegmentacionAreaId == segmentacionArea.Id).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
