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
    public interface ISegmentacionAreaRepository : IRepository<SegmentacionArea>
    {
        Task<SegmentacionArea> GetSegmentacionAreaById(SegmentacionArea SegmentacionArea);
        Task<IEnumerable<SegmentacionArea>> GetSegmentacionAreas();
        Task<IEnumerable<SegmentacionArea>> GetSegmentacionAreasByEvaluacionId(Evaluacion evaluacion);
        Task<int> DeleteSegmentacionArea(SegmentacionArea segmentacionArea);
    }
    public class SegmentacionAreaRepository : Repository<SegmentacionArea, Context>, ISegmentacionAreaRepository
    {
        public SegmentacionAreaRepository(Context context) : base(context) { }

        public async Task<SegmentacionArea> GetSegmentacionAreaById(SegmentacionArea SegmentacionArea)
        {
            if (string.IsNullOrEmpty(SegmentacionArea.Id.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            var retorno = await Context()
                            .SegmentacionAreas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == SegmentacionArea.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<SegmentacionArea>> GetSegmentacionAreas()
        {
            var retorno = await  Context()
                            .SegmentacionAreas
                            .AsNoTracking().Where(x=>  x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<SegmentacionArea>> GetSegmentacionAreasByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .SegmentacionAreas.Where(y => y.EvaluacionId == evaluacion.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<int> DeleteSegmentacionArea(SegmentacionArea segmentacionArea)
        {

          return  await Context().SegmentacionAreas.Where(x => x.Id == segmentacionArea.Id).DeleteFromQueryAsync();
             }

    }
}
