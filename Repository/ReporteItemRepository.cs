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
    public interface IReporteItemRepository : IRepository<ReporteItem>
    {
        Task<ReporteItem> GetReporteItemById(ReporteItem ReporteItem);
        Task<IEnumerable<ReporteItem>> GetReporteItems();
        Task<IEnumerable<ReporteItem>> GetReporteItemsByReporteId(Reporte reporte);
    }
    public class ReporteItemRepository : Repository<ReporteItem, Context>, IReporteItemRepository
    {
        public ReporteItemRepository(Context context) : base(context) { }
        public async Task<ReporteItem> GetReporteItemById(ReporteItem ReporteItem)
        {
            if (string.IsNullOrEmpty(ReporteItem.Id.ToString())) throw new ArgumentNullException("ReporteItemId");
            var retorno = await Context()
                            .ReporteItems
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ReporteItem.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<ReporteItem>> GetReporteItems()
        {
            var retorno = await  Context()
                            .ReporteItems
                            .AsNoTracking().Where(x=>  x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ReporteItem>> GetReporteItemsByReporteId(Reporte reporte)
        {
            var retorno = await Context()
                            .ReporteItems.Where(y => y.ReporteId == reporte.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
