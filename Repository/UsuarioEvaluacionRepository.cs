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
    public interface IUsuarioEvaluacionRepository : IRepository<UsuarioEvaluacion>
    {
        Task<UsuarioEvaluacion> GetUsuarioEvaluacionById(UsuarioEvaluacion UsuarioEvaluacion);
        Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacions();
        Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacionsByUsuarioId(Usuario usuario);
        Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacionsByEmpresaId(Empresa empresa);
        Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacionsByEvaluacionId(Evaluacion evaluacion);
    }
    public class UsuarioEvaluacionRepository : Repository<UsuarioEvaluacion, Context>, IUsuarioEvaluacionRepository
    {
        public UsuarioEvaluacionRepository(Context context) : base(context) { }

        public async Task<UsuarioEvaluacion> GetUsuarioEvaluacionById(UsuarioEvaluacion UsuarioEvaluacion)
        {
            if (string.IsNullOrEmpty(UsuarioEvaluacion.Id.ToString())) throw new ArgumentNullException("UsuarioEvaluacionId");
            var retorno = await Context()
                            .UsuarioEvaluacions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == UsuarioEvaluacion.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacions()
        {
            var retorno = await  Context()
                            .UsuarioEvaluacions
                            .AsNoTracking().Where(x=> x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacionsByUsuarioId(Usuario usuario)
        {
            var retorno = await Context()
                            .UsuarioEvaluacions.Where(y => y.UsuarioId == usuario.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacionsByEmpresaId(Empresa empresa)
        {
            var retorno = await Context()
                            .UsuarioEvaluacions.Where(y => y.EmpresaId == empresa.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<UsuarioEvaluacion>> GetUsuarioEvaluacionsByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .UsuarioEvaluacions.Where(y => y.EvaluacionId == evaluacion.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

    }
}
