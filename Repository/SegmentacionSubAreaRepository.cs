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
    public interface ISegmentacionSubAreaRepository : IRepository<SegmentacionSubArea>
    {
        Task<SegmentacionSubArea> GetSegmentacionSubAreaById(SegmentacionSubArea SegmentacionSubArea);
        Task<IEnumerable<SegmentacionSubArea>> GetSegmentacionSubAreas();
        Task<IEnumerable<SegmentacionSubArea>> GetSegmentacionSubAreasBySegmentacionAreaId(SegmentacionArea segmentacionArea);
        Task<int> DeleteSegmentacionSubArea(SegmentacionSubArea segmentacionSubArea);
    }
    public class SegmentacionSubAreaRepository : Repository<SegmentacionSubArea, Context>, ISegmentacionSubAreaRepository
    {
        public SegmentacionSubAreaRepository(Context context) : base(context) { }

        public async Task<SegmentacionSubArea> GetSegmentacionSubAreaById(SegmentacionSubArea SegmentacionSubArea)
        {
            if (string.IsNullOrEmpty(SegmentacionSubArea.Id.ToString())) throw new ArgumentNullException("SegmentacionSubAreaId");
            var retorno = await Context()
                            .SegmentacionSubAreas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == SegmentacionSubArea.Id);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<SegmentacionSubArea>> GetSegmentacionSubAreas()
        {
            var retorno = await  Context()
                            .SegmentacionSubAreas
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<SegmentacionSubArea>> GetSegmentacionSubAreasBySegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .SegmentacionSubAreas.Where(y => y.SegmentacionAreaId == segmentacionArea.Id ).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<int> DeleteSegmentacionSubArea(SegmentacionSubArea segmentacionSubArea)
        {

            return await Context().SegmentacionSubAreas.Where(x => x.Id == segmentacionSubArea.Id).DeleteFromQueryAsync();
        }


    }
}
