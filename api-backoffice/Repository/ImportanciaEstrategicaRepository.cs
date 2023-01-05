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
    public interface IImportanciaEstrategicaRepository : IRepository<ImportanciaEstrategica>
    {
        Task<ImportanciaEstrategica> GetImportanciaEstrategicaById(ImportanciaEstrategica ImportanciaEstrategica);
        Task<IEnumerable<ImportanciaEstrategica>> GetImportanciaEstrategicas();
        Task<IEnumerable<ImportanciaEstrategica>> GetImportanciaEstrategicasByImportanciaRelativaId(ImportanciaRelativa importanciaRelativa);
        Task<IEnumerable<ImportanciaEstrategica>> GetImportanciaEstrategicasBySegmentacionSubAreaId(SegmentacionSubArea segmentacionSubArea);
    }
    public class ImportanciaEstrategicaRepository : Repository<ImportanciaEstrategica, Context>, IImportanciaEstrategicaRepository
    {
        public ImportanciaEstrategicaRepository(Context context) : base(context) { }
        public async Task<ImportanciaEstrategica> GetImportanciaEstrategicaById(ImportanciaEstrategica ImportanciaEstrategica)
        {
            if (string.IsNullOrEmpty(ImportanciaEstrategica.Id.ToString())) throw new ArgumentNullException("ImportanciaEstrategicaId");
            var retorno = await Context()
                            .ImportanciaEstrategicas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ImportanciaEstrategica.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<ImportanciaEstrategica>> GetImportanciaEstrategicas()
        {
            var retorno = await  Context()
                            .ImportanciaEstrategicas
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ImportanciaEstrategica>> GetImportanciaEstrategicasByImportanciaRelativaId(ImportanciaRelativa importanciaRelativa)
        {
            var retorno = await Context()
                            .ImportanciaEstrategicas.Where(y => y.ImportanciaRelativaId == importanciaRelativa.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ImportanciaEstrategica>> GetImportanciaEstrategicasBySegmentacionSubAreaId(SegmentacionSubArea segmentacionSubArea)
        {
            var retorno = await Context()
                            .ImportanciaEstrategicas.Where(y => y.SegmentacionSubAreaId == segmentacionSubArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
