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
    public interface IImportanciaRelativaRepository : IRepository<ImportanciaRelativa>
    {
        Task<ImportanciaRelativa> GetImportanciaRelativaById(ImportanciaRelativa ImportanciaRelativa);
        Task<IEnumerable<ImportanciaRelativa>> GetImportanciaRelativas();
        Task<IEnumerable<ImportanciaRelativa>> GetImportanciaRelativasByEvaluacionEmpresaId(EvaluacionEmpresa evaluacionEmpresa);
        Task<IEnumerable<ImportanciaRelativa>> GetImportanciaRelativasBySegmentacionAreaId(SegmentacionArea segmentacionArea);
    }
    public class ImportanciaRelativaRepository : Repository<ImportanciaRelativa, Context>, IImportanciaRelativaRepository
    {
        public ImportanciaRelativaRepository(Context context) : base(context) { }
        public async Task<ImportanciaRelativa> GetImportanciaRelativaById(ImportanciaRelativa ImportanciaRelativa)
        {
            if (string.IsNullOrEmpty(ImportanciaRelativa.Id.ToString())) throw new ArgumentNullException("ImportanciaRelativaId");
            var retorno = await Context()
                            .ImportanciaRelativas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == ImportanciaRelativa.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<ImportanciaRelativa>> GetImportanciaRelativas()
        {
            var retorno = await  Context()
                            .ImportanciaRelativas
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ImportanciaRelativa>> GetImportanciaRelativasByEvaluacionEmpresaId(EvaluacionEmpresa evaluacionEmpresa)
        {
            var retorno = await Context()
                            .ImportanciaRelativas.Where(y => y.EvaluacionEmpresaId == evaluacionEmpresa.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<ImportanciaRelativa>> GetImportanciaRelativasBySegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .ImportanciaRelativas.Where(y => y.SegmentacionAreaId == segmentacionArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
