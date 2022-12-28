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
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        Task<Empresa> GetEmpresaByRutEmpresa(string rutempresa);

        Task<Empresa> GetEmpresaById(Empresa empresa);
        Task<IEnumerable<Empresa>> GetEmpresas();
        Task<IEnumerable<Empresa>> GetEmpresasByUsuarioId(Usuario usuario);
    }
    public class EmpresaRepository : Repository<Empresa, Context>, IEmpresaRepository
    {
        public EmpresaRepository(Context context) : base(context) { }
        public async Task<Empresa> GetEmpresaByRutEmpresa(string rutempresa)
        {
            if (string.IsNullOrEmpty(rutempresa)) throw new ArgumentNullException("rutempresa");
            var retorno = await Context()
                            .Empresas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.RutEmpresa == rutempresa);

            if (retorno == null) return null;
            return retorno;
        }


        public async Task<Empresa> GetEmpresaById(Empresa empresa)
        {
            if (string.IsNullOrEmpty(empresa.Id.ToString())) throw new ArgumentNullException("EmpresaId");
            var retorno = await Context()
                            .Empresas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == empresa.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Empresa>> GetEmpresas()
        {
            var retorno = await Context()
                            .Empresas.Include(x => x.EvaluacionEmpresas)
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            await maperEvaluacionArea(retorno);

            if (retorno == null) return null;
            return retorno;
        }

        private async Task maperEvaluacionArea(List<Empresa> retorno)
        {
            foreach (var re in retorno)
            {
                foreach (var ue in re.EvaluacionEmpresas)
                {
                    ue.Evaluacion = await Context().Evaluacions
                       // .Include(x => x.SegmentacionAreas)// <-
                        .FirstOrDefaultAsync(x => x.Id == ue.EvaluacionId);

                     //ue.Evaluacion.SegmentacionAreas = await Context().SegmentacionAreas.Where(x => x.EvaluacionId == ue.EvaluacionId).ToListAsync();
                }
            }
        }

        public async Task<IEnumerable<Empresa>> GetEmpresasByUsuarioId(Usuario usuario)
        {
            var retorno = await Context()
                            .Empresas.Include(i => i.UsuarioEmpresas.Where(y => y.UsuarioId == usuario.Id)).AsNoTracking().ToListAsync();
            retorno = retorno.Where(x => x.UsuarioEmpresas.Count() >=1 && x.Activo.Value).ToList();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
