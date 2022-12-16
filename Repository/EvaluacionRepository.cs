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
    public interface IEvaluacionRepository : IRepository<Evaluacion>
    {
        Task<Evaluacion> GetEvaluacionById(Evaluacion Evaluacion);
        Task<IEnumerable<Evaluacion>> GetEvaluacions();
        Task<IEnumerable<Evaluacion>> GetEvaluacionsByUsuarioId(Usuario usuario);
        Task<IEnumerable<Evaluacion>> GetEvaluacionsByEmpresaId(Empresa empresa);
    }
    public class EvaluacionRepository : Repository<Evaluacion, Context>, IEvaluacionRepository
    {
        public EvaluacionRepository(Context context) : base(context) { }

        public async Task<Evaluacion> GetEvaluacionById(Evaluacion Evaluacion)
        {
            if (string.IsNullOrEmpty(Evaluacion.Id.ToString())) throw new ArgumentNullException("EvaluacionId");
            var retorno = await Context()
                            .Evaluacions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Evaluacion.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Evaluacion>> GetEvaluacions()
        {
            var retorno = await  Context()
                            .Evaluacions
                            .AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<Evaluacion>> GetEvaluacionsByUsuarioId(Usuario usuario)
        {
            var retorno = await Context()
                            .Evaluacions.Include(i => i.UsuarioEvaluacions.Where(y => y.UsuarioId == usuario.Id)).AsNoTracking().ToListAsync();
            retorno = retorno.Where(x => x.UsuarioEvaluacions.Count >=1 && x.Activo.Value).ToList();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<Evaluacion>> GetEvaluacionsByEmpresaId(Empresa empresa)
        {
            var retorno = await Context()
                            .Evaluacions.Include(i => i.EvaluacionEmpresas.Where(y => y.EmpresaId == empresa.Id)).AsNoTracking().ToListAsync();
            retorno = retorno.Where(x => x.EvaluacionEmpresas.Count() >= 1 && x.Activo.Value).ToList();

            if (retorno == null) return null;
            return retorno;
        }

    }
}
