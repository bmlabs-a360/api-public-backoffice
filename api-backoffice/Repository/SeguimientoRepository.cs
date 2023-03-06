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
    public interface ISeguimientoRepository 
    {
    //    Task<Seguimiento> GetSeguimientoById(Seguimiento Seguimiento);
    //    Task<IEnumerable<Seguimiento>> GetSeguimientos();
    //    Task<IEnumerable<Seguimiento>> GetSeguimientosByEmpresaId(Empresa empresa);
    //    Task<IEnumerable<Seguimiento>> GetSeguimientosByEvaluacionId(Evaluacion evaluacion);
    //    Task<IEnumerable<Seguimiento>> GetSeguimientosByPlanMejoraId(PlanMejora planMejora);
    //}
    //public class SeguimientoRepository : Repository<Seguimiento, Context>, ISeguimientoRepository
    //{
    //    public SeguimientoRepository(Context context) : base(context) { }
    //    public async Task<Seguimiento> GetSeguimientoById(Seguimiento Seguimiento)
    //    {
    //        if (string.IsNullOrEmpty(Seguimiento.Id.ToString())) throw new ArgumentNullException("SeguimientoId");
    //        Seguimiento retorno = null;  //await Context()
    //        //                .Seguimientos
    //        //                .AsNoTracking()
    //        //                .FirstOrDefaultAsync(x => x.Id == Seguimiento.Id   );

    //        if (retorno == null) return null;
    //        return retorno; 
    //    }
    //    public async Task<IEnumerable<Seguimiento>> GetSeguimientos()
    //    {
    //        List<Seguimiento> retorno = null;  //await Context()
    //                        //.Seguimientos
    //                        //.AsNoTracking().ToListAsync();

    //        if (retorno == null) return null;
    //        return retorno;
    //    }
    //    public async Task<IEnumerable<Seguimiento>> GetSeguimientosByEmpresaId(Empresa empresa)
    //    {
    //        List<Seguimiento> retorno = null;  //await Context()
    //                        //.Seguimientos.Where(y => y.EmpresaId == empresa.Id ).AsNoTracking().ToListAsync();

    //        if (retorno == null) return null;
    //        return retorno;
    //    }
    //    public async Task<IEnumerable<Seguimiento>> GetSeguimientosByEvaluacionId(Evaluacion evaluacion)
    //    {
    //        List<Seguimiento> retorno = null;  //await Context()
    //                        //.Seguimientos.Where(y => y.EvaluacionId == evaluacion.Id).AsNoTracking().ToListAsync();

    //        if (retorno == null) return null;
    //        return retorno;
    //    }
    //    public async Task<IEnumerable<Seguimiento>> GetSeguimientosByPlanMejoraId(PlanMejora planMejora)
    //    {
    //        List<Seguimiento> retorno = null;  //await Context()
    //                        //.Seguimientos.Where(y => y.PlanMejoraId == planMejora.Id).AsNoTracking().ToListAsync();

    //        if (retorno == null) return null;
    //        return retorno;
    //    }
    }
}
