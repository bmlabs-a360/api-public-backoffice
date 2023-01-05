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
        Task DeleteList(List<EvaluacionEmpresa> c);
        Task InsertOrUpdateList(List<EvaluacionEmpresa> c);

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
                            .FirstOrDefaultAsync(x => x.Id == EvaluacionEmpresa.Id   );

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

        public async Task InsertOrUpdateList(List<EvaluacionEmpresa> c)
        {
            try
            {
                if (c.Count < 0) throw new Exception("Sin datos para registrar conciliacion");
                decimal rango = 1000;
                var iteracion = Math.Ceiling(((decimal)c.Count / rango));

                using (var ctx = Context())
                {
                    for (int i = 0; i < iteracion; i++)
                    {
                        List<EvaluacionEmpresa> toInsert = c.Take((int)rango).ToList();

                        ctx.ChangeTracker.AutoDetectChangesEnabled = false;
                        ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        ctx.EvaluacionEmpresas.AddRange(toInsert);

                        c.RemoveRange(0, (c.Count < rango) ? c.Count : (int)rango);
                    }
                    ctx.BulkSaveChanges();
                    ctx.ChangeTracker.Clear();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteList(List<EvaluacionEmpresa> existen)
        {
            try
            {
                /*List<EvaluacionEmpresa> existen = null;
                c.ForEach(async e =>
                {
                    EvaluacionEmpresa xxx = await Context().EvaluacionEmpresas.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == e.Id);
                    if (xxx != null) {
                        existen.Add(xxx);
                        }
                });*/
                if (existen == null) return;
                decimal rango = 1000;
                var iteracion = Math.Ceiling(((decimal)existen.Count / rango));

                using (var ctx = Context())
                {
                    for (int i = 0; i < iteracion; i++)
                    {
                        
                            List<EvaluacionEmpresa> toInsert = existen.Take((int)rango).ToList();

                        ctx.ChangeTracker.AutoDetectChangesEnabled = false;
                        ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        
                        ctx.EvaluacionEmpresas.RemoveRange(toInsert);

                        existen.RemoveRange(0, (existen.Count < rango) ? existen.Count : (int)rango);
                    }
                    ctx.BulkSaveChanges();
                    ctx.ChangeTracker.Clear();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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
