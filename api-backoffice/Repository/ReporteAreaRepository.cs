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
    public interface IReporteAreaRepository : IRepository<ReporteArea>
    {
        Task<ReporteArea> GetReporteAreaById(ReporteArea ReporteArea);
        Task<IEnumerable<ReporteArea>> GetReporteAreas();
        Task<IEnumerable<ReporteArea>> GetReporteAreasByReporteId(Reporte reporte);
        Task<IEnumerable<ReporteArea>> GetReporteAreasBySegmentacionAreaId(SegmentacionArea segmentacionArea);
    }
    public class ReporteAreaRepository : Repository<ReporteArea, Context>, IReporteAreaRepository
    {
        public ReporteAreaRepository(Context context) : base(context) { }
        public async Task<ReporteArea> GetReporteAreaById(ReporteArea ReporteArea)
        {
            if (string.IsNullOrEmpty(ReporteArea.Id.ToString())) throw new ArgumentNullException("ReporteAreaId");
            var retorno = await Context()
                            .ReporteAreas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ReporteArea.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<ReporteArea>> GetReporteAreas()
        {
            var retorno = await  Context()
                            .ReporteAreas
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteArea>> GetReporteAreasByReporteId(Reporte reporte)
        {
            var retorno = await Context()
                            .ReporteAreas.Where(y => y.ReporteId == reporte.Id).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteArea>> GetReporteAreasBySegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .ReporteAreas.Where(y => y.SegmentacionAreaId == segmentacionArea.Id).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
