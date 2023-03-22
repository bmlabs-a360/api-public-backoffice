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
using System.Runtime.Intrinsics.X86;

namespace api_public_backOffice.Repository
{
    public interface IEvaluacionRepository : IRepository<Evaluacion>
    {
        int GetCantidadEmpresas(Evaluacion Evaluacion);
        int GetCantidadSegmentacionAreas(Evaluacion Evaluacion);
        int GetCantidadSegmentacionSubAreas(Evaluacion Evaluacion);
        int GetCantidadPreguntas(Evaluacion Evaluacion);
        int GetCantidadAlternativas(Evaluacion Evaluacion);
        EvaluacionModel GetCantidades(EvaluacionModel Evaluacion);
        Task<Evaluacion> GetEvaluacionById(Evaluacion Evaluacion);
        Task<IEnumerable<Evaluacion>> GetEvaluacions();
        Task<IEnumerable<Evaluacion>> GetEvaluacionsByUsuarioId(Usuario usuario);
        Task<IEnumerable<Evaluacion>> GetEvaluacionsByEmpresaId(Empresa empresa);
        Task<Evaluacion> GetEvaluacionByDefecto();
        
    }
    public class EvaluacionRepository : Repository<Evaluacion, Context>, IEvaluacionRepository
    {
        public EvaluacionRepository(Context context) : base(context) { }

        public  int GetCantidadEmpresas(Evaluacion Evaluacion) {
            return Context()
                            .EvaluacionEmpresas
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );

        }
        public  int GetCantidadSegmentacionAreas(Evaluacion Evaluacion)
        {
            return Context()
                            .SegmentacionAreas
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
        }

        public  int GetCantidadSegmentacionSubAreas(Evaluacion Evaluacion)
        {

          //return Context().SegmentacionSubAreas.Include(x => x.SegmentacionArea)
          //      .Where(x => x.SegmentacionArea.EvaluacionId == Evaluacion.Id   && x.SegmentacionArea.Activo.Value)
          //      .Count();


            return (from ssa in Context().SegmentacionSubAreas
             join sa in Context().SegmentacionAreas.Where(x => x.EvaluacionId == Guid.Parse(Evaluacion.Id.ToString())) on
             ssa.SegmentacionAreaId equals sa.Id
             select ssa).Count();

        }

        public  int GetCantidadPreguntas(Evaluacion Evaluacion)
        {
            return Context()
                            .Pregunta
                            //.AsNoTracking()
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
            

        }
        public  int GetCantidadAlternativas(Evaluacion Evaluacion)
        {
            return Context().Alternativas
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
            

        }
        public EvaluacionModel GetCantidades(EvaluacionModel Evaluacion)
        {
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
                (select count(ee.id) as CantidadEmpresas  from evaluacion_empresa ee where ee.evaluacion_id = '{0}'),(
                select count(sa.id) as CantidadAreas from segmentacion_area sa where sa.evaluacion_id = '{0}'),(
                select count(p.id) as CantidadPreguntas from pregunta  p where p.evaluacion_id = '{0}'),(
                select count(a.id) as CantidadAlternativas from alternativa a where a.evaluacion_id = '{0}'),(
                select count(ssa) as CantidadSubAreas  from public.segmentacion_sub_area ssa where ssa.segmentacion_area_id  in (
                select sa.id from segmentacion_area sa where sa.evaluacion_id = '{0}'))",Evaluacion.Id.ToString());

                Context().Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            Evaluacion.CantidadEmpresas = int.Parse(result["CantidadEmpresas"].ToString());
                            Evaluacion.CantidadAreas = int.Parse(result["CantidadAreas"].ToString());
                            Evaluacion.CantidadPreguntas = int.Parse(result["CantidadPreguntas"].ToString());
                            Evaluacion.CantidadAlternativas = int.Parse(result["CantidadAlternativas"].ToString());
                            Evaluacion.CantidadSubAreas = int.Parse(result["CantidadSubAreas"].ToString());
                        }
                    }
                }
            }
            /*
            Evaluacion.CantidadSubAreas = (from ssa in Context().SegmentacionSubAreas
                                            join sa in Context().SegmentacionAreas.Where(x => x.EvaluacionId == Guid.Parse(Evaluacion.Id.ToString())) on
                                            ssa.SegmentacionAreaId equals sa.Id
                                            select ssa).Count();
            Evaluacion.CantidadAlternativas = Context().Alternativas
                            .AsNoTracking()
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
            Evaluacion.CantidadPreguntas = Context().Pregunta
                            .AsNoTracking()
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
            Evaluacion.CantidadAreas= Context()
                            .SegmentacionAreas
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
            Evaluacion.CantidadEmpresas= Context()
                            .EvaluacionEmpresas
                            .Count(x => x.EvaluacionId == Evaluacion.Id  );
            */

         return Evaluacion;
        }
        public async Task<Evaluacion> GetEvaluacionById(Evaluacion Evaluacion)
        {
            if (string.IsNullOrEmpty(Evaluacion.Id.ToString())) throw new ArgumentNullException("EvaluacionId");
            var retorno = await Context()
                            .Evaluacions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Evaluacion.Id   );

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<Evaluacion>> GetEvaluacions()
        {
            var retorno = await  Context()
                            .Evaluacions
                            .Include(inc => inc.SegmentacionAreas)
                            //.Include(x=>x.EvaluacionEmpresas)
                            
                            .AsNoTracking().ToListAsync();
           // foreach (var itemE in retorno)
           // {
               // itemE.EvaluacionEmpresas = Context().EvaluacionEmpresas.Where(x => x.EvaluacionId == itemE.Id).ToList();

               // foreach (var itemS in itemE.SegmentacionAreas)
               // {
               //     itemS.SegmentacionSubAreas = await Context().SegmentacionSubAreas.Where(x=>x.SegmentacionAreaId == itemS.Id).ToListAsync(); 
               // }
            //}

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<Evaluacion>> GetEvaluacionsByUsuarioId(Usuario usuario)
        {
            var retorno = await Context()
                            .Evaluacions.Include(i => i.UsuarioEvaluacions.Where(y => y.UsuarioId == usuario.Id)).AsNoTracking().ToListAsync();
            retorno = retorno.Where(x => x.UsuarioEvaluacions.Count >=1  ).ToList();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<Evaluacion>> GetEvaluacionsByEmpresaId(Empresa empresa)
        {
            var retorno = await Context()
                            .Evaluacions.Include(i => i.EvaluacionEmpresas.Where(y => y.EmpresaId == empresa.Id)).AsNoTracking().ToListAsync();
            retorno = retorno.Where(x => x.EvaluacionEmpresas.Count() >= 1  ).ToList();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<Evaluacion> GetEvaluacionByDefecto()
        {
            var retorno = await Context()
                            .Evaluacions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Default == true);

            if (retorno == null) return null;
            return retorno;
        }
        

    }
}
