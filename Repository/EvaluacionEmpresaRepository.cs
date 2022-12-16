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
    public interface IEvaluacionEmpresaRepository : IRepository<EvaluacionEmpresa>
    {
        Task<EvaluacionEmpresa> GetEvaluacionEmpresaById(EvaluacionEmpresa EvaluacionEmpresa);
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresas();
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEvaluacionId(Evaluacion evaluacion);
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEmpresaId(Empresa empresa);

    }
    public class EvaluacionEmpresaRepository : Repository<EvaluacionEmpresa, Context>, IEvaluacionEmpresaRepository
    {
        public EvaluacionEmpresaRepository(Context context) : base(context) { }

        public async Task<EvaluacionEmpresa> GetEvaluacionEmpresaById(EvaluacionEmpresa EvaluacionEmpresa)
        {
            if (string.IsNullOrEmpty(EvaluacionEmpresa.Id.ToString())) throw new ArgumentNullException("EvaluacionEmpresaId");
            var retorno = await Context()
                            .EvaluacionEmpresas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == EvaluacionEmpresa.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresas()
        {
            var retorno = await  Context()
                            .EvaluacionEmpresas
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .EvaluacionEmpresas.Where(i => i.EvaluacionId  == evaluacion.Id&& i.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEmpresaId(Empresa empresa)
        {
            var retorno = await Context()
                            .EvaluacionEmpresas.Where(i => i.EmpresaId == empresa.Id && i.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

    }
}
