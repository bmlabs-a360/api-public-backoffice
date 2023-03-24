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
using System.Collections;
using MongoDB.Bson;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text.RegularExpressions;
using System.Numerics;

namespace api_public_backOffice.Repository
{
    public interface IEvaluacionEmpresaRepository : IRepository<EvaluacionEmpresa>
    {
        Task<EvaluacionEmpresa> GetEvaluacionEmpresaById(EvaluacionEmpresa EvaluacionEmpresa);
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresas();
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEvaluacionId(Evaluacion evaluacion);
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEmpresaId(Empresa empresa);
        Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEvaluacionIdEmpresaId(Guid evaluacionEmpresaId, Guid empresaId);
        Task DeleteList(List<EvaluacionEmpresa> c);
        Task InsertOrUpdateList(List<EvaluacionEmpresa> c);
       // Task<IEnumerable<EvaluacionEmpresaModel>> GetSeguimiento();
        List<SeguimientoEvaluacionEmpresaDto> GetSeguimiento();
        List<SeguimientoPlanMejoraModelDto> GetPlanMejoras(EvaluacionEmpresa evaluacionEmpresa);
        List<SeguimientoPlanMejoraModelDto> GetFeedback(EvaluacionEmpresa evaluacionEmpresa, Guid evaluacionId);
        List<SeguimientoPlanMejoraModelDto> GetPlanMejorasReporteSubscripcionOBasico(Guid evaluacionId, List<Guid> areas);
        List<PorcentajeEvaluacionDto> GetPorcentajeEvaluacion(Guid evaluacionId, Guid empresaId);
        List<EnvioMailTiempoLimiteDto> GetCorreoTiempoLimite(Guid SegmentacionAreaId, Guid empresaId);



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
                            .EvaluacionEmpresas.Include(x => x.ImportanciaRelativas)
                                .ThenInclude(y => y.SegmentacionArea)
                                    .ThenInclude(e => e.SegmentacionSubAreas)
                                        .ThenInclude(a => a.ImportanciaEstrategicas)
                            .Include(x => x.Evaluacion)
                            .AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }


        public List<SeguimientoEvaluacionEmpresaDto> GetSeguimiento()
        {
            List<SeguimientoEvaluacionEmpresaDto> lista = new List<SeguimientoEvaluacionEmpresaDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select ee.id as evaluacion_empresa_id,
                                                    em.razon_social, 
                                                    em.id as empresa_id,
                                                    ev.nombre, 
                                                    ev.id as evaluacion_id,
                                                    's/m' as madurez,
                                                    concat((count(re.id) * 100) / count(pr.id),'%') as respuestas,
                                                    max(re.fecha_creacion) as re_fecha_creacion,
                                                    concat((count(pm.id) * 100) / count(pr.id),'%') as plan_mejoras,
                                                    max(pm.fecha_creacion) as pm_fecha_creacion,
                                                    ev.tiempo_limite,
                                                    ev.fecha_creacion as fecha_creacion_evaluacion,
                                                    now() -  ev.fecha_creacion as dias_transcurridos
                                                    from
                                                        public.empresa em  join
                                                        public.evaluacion_empresa ee on 
                                                         em.id = ee.empresa_id join 
                                                        public.evaluacion ev on 
                                                        ev.id =ee.evaluacion_id  join 
                                                        public.pregunta pr on
                                                        pr.evaluacion_id  = ee.evaluacion_id left join 
                                                        public.respuesta re on
                                                        re.pregunta_id  = pr.id and re.evaluacion_empresa_id =ee.id  left join 
                                                        public.plan_mejora pm on
                                                         pm.evaluacion_empresa_id = ee.id and pm.pregunta_id =pr.id and pm.segmentacion_area_id =pr.segmentacion_area_id 
                                                        and pm.segmentacion_sub_area_id =pr.segmentacion_sub_area_id 
                                                    group by ee.id, ev.nombre,em.razon_social, ev.tiempo_limite, ev.fecha_creacion, em.id, ev.id
                                                    order by em.razon_social ");

                Context().Database.OpenConnection();
                
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            SeguimientoEvaluacionEmpresaDto item = new SeguimientoEvaluacionEmpresaDto();
                            item.EvaluacionEmpresaId = Guid.Parse(result["evaluacion_empresa_id"].ToString());
                            item.EmpresaRazonSocial = (result["razon_social"].ToString());
                            item.EmpresaId = Guid.Parse(result["empresa_id"].ToString());
                            item.EvaluacionNombre = (result["nombre"].ToString());
                            item.EvaluacionId = Guid.Parse(result["evaluacion_id"].ToString());
                            item.Madurez = (result["madurez"].ToString());
                            item.Respuestas = (result["respuestas"].ToString());
                            item.RespuestaFechaMax = (result["re_fecha_creacion"].ToString())==string.Empty ? "s/a": (result["re_fecha_creacion"].ToString());
                            item.PlanMejoras = (result["plan_mejoras"].ToString());
                            item.PlanMejoraFechaMax = (result["pm_fecha_creacion"].ToString()) == string.Empty ? "s/a" : (result["pm_fecha_creacion"].ToString()); 
                            item.Estado = int.Parse(item.Respuestas.Replace("%", "")) > 0;
                            item.TiempoLimite = result["tiempo_limite"].ToString();
                            item.FechaCreacionEvaluacion = result["fecha_creacion_evaluacion"].ToString();
                            item.DiasTranscurridos = result["dias_transcurridos"].ToString();
                            lista.Add(item);
                        }
                    }
                }
            }


            // return (Task<IEnumerable<EvaluacionEmpresaModel>>)(IEnumerable<EvaluacionEmpresaModel>)lista;
            // return (Task<List<SeguimientoEvaluacionEmpresa>>)(IEnumerable<SeguimientoEvaluacionEmpresa>) lista;
            return lista;
        }

        public List<SeguimientoPlanMejoraModelDto> GetPlanMejoras(EvaluacionEmpresa evaluacionEmpresa)
        {
            List<SeguimientoPlanMejoraModelDto> lista = new List<SeguimientoPlanMejoraModelDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select  
	                                        ee.id as evaluacion_empresa_id,
	                                        ee.evaluacion_id ,
	                                        ee.empresa_id ,
	                                        ee.fecha_inicio_tiempo_limite  ,
	                                        pr.id  as pregunta_id,
	                                        pr.segmentacion_area_id,
	                                        pr.segmentacion_sub_area_id ,
	                                        pr.detalle as pr_detalle ,
	                                        pr.orden as pr_orden,
	                                        pr.capacidad as pr_capacidad,
	                                        al.id as alternativa_id ,
	                                        al.detalle as  al_detalle,
	                                        al.valor as al_valor,
	                                        re.tipo_importancia_id ,
	                                        re.tipo_diferencia_relacionada_id ,
	                                        re.valor as re_valor ,
	                                        re.realimentacion ,
	                                        pm.id as plan_mejora_id,
	                                        pm.mejora ,
	                                        sa.nombre_area ,
	                                        ssa.nombre_sub_area,
	                                        ti.nombre as tipo_importancia_nombre,
	                                        tdr.nombre as tipo_diferencia_relacionada_nombre,
                                            ti.detalle as importancia_detalle
                                        from
	                                        public.evaluacion_empresa ee join 
	                                        public.pregunta pr on
	                                        pr.evaluacion_id  = ee.evaluacion_id left join 
	                                        public.respuesta re on
	                                        re.pregunta_id  = pr.id and re.evaluacion_empresa_id =ee.id  left join 
	                                        public.alternativa al on
	                                        al.id =re.alternativa_id and al.pregunta_id =pr.id left join
	                                        public.plan_mejora pm on
	                                        pm.evaluacion_empresa_id = ee.id and pm.pregunta_id =pr.id and pm.segmentacion_area_id =pr.segmentacion_area_id 
	                                        and pm.segmentacion_sub_area_id =pr.segmentacion_sub_area_id and pm.alternativa_id =al.id  join 
	                                        public.segmentacion_area sa on
	                                        sa.id =pr.segmentacion_area_id and sa.evaluacion_id = ee.evaluacion_id join 
	                                        public.segmentacion_sub_area ssa on
	                                        ssa.id = pr.segmentacion_sub_area_id and ssa.segmentacion_area_id =sa.id left join
	                                        public.tipo_importancia ti on
	                                        ti.id = re.tipo_importancia_id left join 
	                                        public.tipo_diferencia_relacionada tdr on
	                                        tdr.id  = re.tipo_diferencia_relacionada_id 
                                        where ee.id = '{0}' ", evaluacionEmpresa.Id.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            SeguimientoPlanMejoraModelDto item = new SeguimientoPlanMejoraModelDto();
                            item.EvaluacionEmpresaId = Guid.Parse(result["evaluacion_empresa_id"].ToString());
                            item.EvaluacionId = Guid.Parse(result["evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["empresa_id"].ToString());
                            item.FechaInicioTiempoLimite = (result["fecha_inicio_tiempo_limite"].ToString());
                            item.PreguntaId = Guid.Parse(result["pregunta_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["segmentacion_area_id"].ToString());
                            item.SegmentacionSubAreaId = Guid.Parse(result["segmentacion_sub_area_id"].ToString());
                            item.PreguntaDetalle = (result["pr_detalle"].ToString());
                            item.PreguntaOrden = (result["pr_orden"].ToString());
                            item.PreguntaCapacidad = (result["pr_capacidad"].ToString());
                            if (result["alternativa_id"].ToString() != string.Empty)
                                item.AlternativaId =  Guid.Parse(result["alternativa_id"].ToString());
                            item.AlternativaDetalle = (result["al_detalle"].ToString()) == string.Empty ? "s/r" : (result["al_detalle"].ToString());
                            item.AlternativaValor = (result["al_valor"].ToString()) == string.Empty ? "s/r" : (result["al_valor"].ToString());
                            if (result["tipo_importancia_id"].ToString() != string.Empty)
                                item.TipoImportanciaId = Guid.Parse(result["tipo_importancia_id"].ToString());
                            if (result["tipo_diferencia_relacionada_id"].ToString() != string.Empty)
                                item.TipoDiferenciaRelacionadaId = Guid.Parse(result["tipo_diferencia_relacionada_id"].ToString());
                            item.RespuestaValor = (result["re_valor"].ToString()) == string.Empty ? "s/v" : (result["re_valor"].ToString());
                            item.RespuestaRealimentacion = (result["realimentacion"].ToString());
                            if (result["plan_mejora_id"].ToString() != string.Empty)
                                item.PlanMejoraId = Guid.Parse(result["plan_mejora_id"].ToString());  
                            item.Mejora = (result["mejora"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.NombreSubArea = (result["nombre_sub_area"].ToString());
                            item.Accion = result["re_valor"].ToString() != string.Empty;
                            item.Estado=result["plan_mejora_id"].ToString() != string.Empty;
                            item.TipoImportanciaNombre = (result["tipo_importancia_nombre"].ToString()) == string.Empty ? "s/d" : (result["tipo_importancia_nombre"].ToString());  
                            item.TipoDiferenciaRelacionadaNombre = (result["tipo_diferencia_relacionada_nombre"].ToString()) == string.Empty ? "s/d" : (result["tipo_diferencia_relacionada_nombre"].ToString()); 
                            item.ImportanciaDetalle = (result["importancia_detalle"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<SeguimientoPlanMejoraModelDto> GetFeedback(EvaluacionEmpresa evaluacionEmpresa, Guid evaluacionId)
        {
            List<SeguimientoPlanMejoraModelDto> lista = new List<SeguimientoPlanMejoraModelDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select  
	                                        ee.id as evaluacion_empresa_id,
	                                        ee.evaluacion_id ,
	                                        ee.empresa_id ,
	                                        ee.fecha_inicio_tiempo_limite  ,
	                                        pr.id  as pregunta_id,
	                                        pr.segmentacion_area_id,
	                                        pr.segmentacion_sub_area_id ,
	                                        pr.detalle as pr_detalle ,
	                                        pr.orden as pr_orden,
	                                        pr.capacidad as pr_capacidad,
	                                        al.id as alternativa_id ,
	                                        al.detalle as  al_detalle,
	                                        al.valor as al_valor,
	                                        re.tipo_importancia_id ,
	                                        re.tipo_diferencia_relacionada_id ,
	                                        re.valor as re_valor ,
	                                        re.realimentacion ,
	                                        pm.id as plan_mejora_id,
	                                        pm.mejora ,
	                                        sa.nombre_area ,
	                                        ssa.nombre_sub_area,
	                                        ti.nombre as tipo_importancia_nombre,
	                                        tdr.nombre as tipo_diferencia_relacionada_nombre,
                                            ti.detalle as importancia_detalle
                                        from
	                                        public.evaluacion_empresa ee join 
	                                        public.pregunta pr on
	                                        pr.evaluacion_id  = ee.evaluacion_id left join 
	                                        public.respuesta re on
	                                        re.pregunta_id  = pr.id and re.evaluacion_empresa_id =ee.id  left join 
	                                        public.alternativa al on
	                                        al.id =re.alternativa_id and al.pregunta_id =pr.id left join
	                                        public.plan_mejora pm on
	                                        pm.evaluacion_empresa_id = ee.id and pm.pregunta_id =pr.id and pm.segmentacion_area_id =pr.segmentacion_area_id 
	                                        and pm.segmentacion_sub_area_id =pr.segmentacion_sub_area_id and pm.alternativa_id =al.id  join 
	                                        public.segmentacion_area sa on
	                                        sa.id =pr.segmentacion_area_id and sa.evaluacion_id = ee.evaluacion_id join 
	                                        public.segmentacion_sub_area ssa on
	                                        ssa.id = pr.segmentacion_sub_area_id and ssa.segmentacion_area_id =sa.id left join
	                                        public.tipo_importancia ti on
	                                        ti.id = re.tipo_importancia_id left join 
	                                        public.tipo_diferencia_relacionada tdr on
	                                        tdr.id  = re.tipo_diferencia_relacionada_id 
                                        where ee.id = '{0}' ", evaluacionEmpresa.Id.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            SeguimientoPlanMejoraModelDto item = new SeguimientoPlanMejoraModelDto();
                            item.EvaluacionEmpresaId = Guid.Parse(result["evaluacion_empresa_id"].ToString());
                            item.EvaluacionId = Guid.Parse(result["evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["empresa_id"].ToString());
                            item.FechaInicioTiempoLimite = (result["fecha_inicio_tiempo_limite"].ToString());
                            item.PreguntaId = Guid.Parse(result["pregunta_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["segmentacion_area_id"].ToString());
                            item.SegmentacionSubAreaId = Guid.Parse(result["segmentacion_sub_area_id"].ToString());
                            item.PreguntaDetalle = (result["pr_detalle"].ToString());
                            item.PreguntaOrden = (result["pr_orden"].ToString());
                            item.PreguntaCapacidad = (result["pr_capacidad"].ToString());
                            if (result["alternativa_id"].ToString() != string.Empty)
                                item.AlternativaId = Guid.Parse(result["alternativa_id"].ToString());
                            item.AlternativaDetalle = (result["al_detalle"].ToString()) == string.Empty ? "s/r" : (result["al_detalle"].ToString());
                            item.AlternativaValor = (result["al_valor"].ToString()) == string.Empty ? "s/r" : (result["al_valor"].ToString());
                            if (result["tipo_importancia_id"].ToString() != string.Empty)
                                item.TipoImportanciaId = Guid.Parse(result["tipo_importancia_id"].ToString());
                            if (result["tipo_diferencia_relacionada_id"].ToString() != string.Empty)
                                item.TipoDiferenciaRelacionadaId = Guid.Parse(result["tipo_diferencia_relacionada_id"].ToString());
                            item.RespuestaValor = (result["re_valor"].ToString()) == string.Empty ? "s/v" : (result["re_valor"].ToString());
                            item.RespuestaRealimentacion = (result["realimentacion"].ToString());
                            if (result["plan_mejora_id"].ToString() != string.Empty)
                                item.PlanMejoraId = Guid.Parse(result["plan_mejora_id"].ToString());
                            item.Mejora = (result["mejora"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.NombreSubArea = (result["nombre_sub_area"].ToString());
                            item.Accion = result["re_valor"].ToString() != string.Empty;
                            item.Estado = result["plan_mejora_id"].ToString() != string.Empty;
                            item.TipoImportanciaNombre = (result["tipo_importancia_nombre"].ToString()) == string.Empty ? "s/d" : (result["tipo_importancia_nombre"].ToString());
                            item.TipoDiferenciaRelacionadaNombre = (result["tipo_diferencia_relacionada_nombre"].ToString()) == string.Empty ? "s/d" : (result["tipo_diferencia_relacionada_nombre"].ToString());
                            item.ImportanciaDetalle = (result["importancia_detalle"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<SeguimientoPlanMejoraModelDto> GetPlanMejorasReporteSubscripcionOBasico(Guid evaluacionEmpresaId, List<Guid> areas)
        {
            List<SeguimientoPlanMejoraModelDto> lista = new List<SeguimientoPlanMejoraModelDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select  
	                                        ee.id as evaluacion_empresa_id,
	                                        ee.evaluacion_id ,
	                                        ee.empresa_id ,
	                                        ee.fecha_inicio_tiempo_limite  ,
	                                        pr.id  as pregunta_id,
	                                        pr.segmentacion_area_id,
	                                        pr.segmentacion_sub_area_id ,
	                                        pr.detalle as pr_detalle ,
	                                        pr.orden as pr_orden,
	                                        pr.capacidad as pr_capacidad,
	                                        al.id as alternativa_id ,
	                                        al.detalle as  al_detalle,
	                                        al.valor as al_valor,
	                                        re.tipo_importancia_id ,
	                                        re.tipo_diferencia_relacionada_id ,
	                                        re.valor as re_valor ,
	                                        re.realimentacion ,
	                                        pm.id as plan_mejora_id,
	                                        pm.mejora ,
	                                        sa.nombre_area ,
	                                        ssa.nombre_sub_area,
	                                        ti.nombre as tipo_importancia_nombre,
	                                        tdr.nombre as tipo_diferencia_relacionada_nombre,
                                            ti.detalle as importancia_detalle
                                        from
	                                        public.evaluacion_empresa ee join 
	                                        public.pregunta pr on
	                                        pr.evaluacion_id  = ee.evaluacion_id left join 
	                                        public.respuesta re on
	                                        re.pregunta_id  = pr.id and re.evaluacion_empresa_id =ee.id  left join 
	                                        public.alternativa al on
	                                        al.id =re.alternativa_id and al.pregunta_id =pr.id left join
	                                        public.plan_mejora pm on
	                                        pm.evaluacion_empresa_id = ee.id and pm.pregunta_id =pr.id and pm.segmentacion_area_id =pr.segmentacion_area_id 
	                                        and pm.segmentacion_sub_area_id =pr.segmentacion_sub_area_id and pm.alternativa_id =al.id  join 
	                                        public.segmentacion_area sa on
	                                        sa.id =pr.segmentacion_area_id and sa.evaluacion_id = ee.evaluacion_id join 
	                                        public.segmentacion_sub_area ssa on
	                                        ssa.id = pr.segmentacion_sub_area_id and ssa.segmentacion_area_id =sa.id left join
	                                        public.tipo_importancia ti on
	                                        ti.id = re.tipo_importancia_id left join 
	                                        public.tipo_diferencia_relacionada tdr on
	                                        tdr.id  = re.tipo_diferencia_relacionada_id 
                                        where ee.id = '{0}' ", evaluacionEmpresaId.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            SeguimientoPlanMejoraModelDto item = new SeguimientoPlanMejoraModelDto();
                            item.EvaluacionEmpresaId = Guid.Parse(result["evaluacion_empresa_id"].ToString());
                            item.EvaluacionId = Guid.Parse(result["evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["empresa_id"].ToString());
                            item.FechaInicioTiempoLimite = (result["fecha_inicio_tiempo_limite"].ToString());
                            item.PreguntaId = Guid.Parse(result["pregunta_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["segmentacion_area_id"].ToString());
                            item.SegmentacionSubAreaId = Guid.Parse(result["segmentacion_sub_area_id"].ToString());
                            item.PreguntaDetalle = (result["pr_detalle"].ToString());
                            item.PreguntaOrden = (result["pr_orden"].ToString());
                            item.PreguntaCapacidad = (result["pr_capacidad"].ToString());
                            if (result["alternativa_id"].ToString() != string.Empty)
                                item.AlternativaId = Guid.Parse(result["alternativa_id"].ToString());
                            item.AlternativaDetalle = (result["al_detalle"].ToString()) == string.Empty ? "s/r" : (result["al_detalle"].ToString());
                            item.AlternativaValor = (result["al_valor"].ToString()) == string.Empty ? "s/r" : (result["al_valor"].ToString());
                            if (result["tipo_importancia_id"].ToString() != string.Empty)
                                item.TipoImportanciaId = Guid.Parse(result["tipo_importancia_id"].ToString());
                            if (result["tipo_diferencia_relacionada_id"].ToString() != string.Empty)
                                item.TipoDiferenciaRelacionadaId = Guid.Parse(result["tipo_diferencia_relacionada_id"].ToString());
                            item.RespuestaValor = (result["re_valor"].ToString()) == string.Empty ? "s/v" : (result["re_valor"].ToString());
                            item.RespuestaRealimentacion = (result["realimentacion"].ToString());
                            if (result["plan_mejora_id"].ToString() != string.Empty)
                                item.PlanMejoraId = Guid.Parse(result["plan_mejora_id"].ToString());
                            item.Mejora = (result["mejora"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.NombreSubArea = (result["nombre_sub_area"].ToString());
                            item.Accion = result["re_valor"].ToString() != string.Empty;
                            item.Estado = result["plan_mejora_id"].ToString() != string.Empty;
                            item.TipoImportanciaNombre = (result["tipo_importancia_nombre"].ToString()) == string.Empty ? "s/d" : (result["tipo_importancia_nombre"].ToString());
                            item.TipoDiferenciaRelacionadaNombre = (result["tipo_diferencia_relacionada_nombre"].ToString()) == string.Empty ? "s/d" : (result["tipo_diferencia_relacionada_nombre"].ToString());
                            item.ImportanciaDetalle = (result["importancia_detalle"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            List<SeguimientoPlanMejoraModelDto> listaRetono = new List<SeguimientoPlanMejoraModelDto>();
            foreach (var ar in areas)
            {
                foreach (var li in lista)
                {
                    if (li.SegmentacionAreaId == ar)
                    {
                        listaRetono.Add(li);
                    }
                }
            }

            return listaRetono;
        }


        public async Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEvaluacionId(Evaluacion evaluacion)
        {
            var retorno = await Context()
                            .EvaluacionEmpresas.Where(i => i.EvaluacionId  == evaluacion.Id).AsNoTracking().ToListAsync();

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
                            .EvaluacionEmpresas
                            .Include(x => x.ImportanciaRelativas)
                                .ThenInclude(y => y.SegmentacionArea)
                                    .ThenInclude(e => e.SegmentacionSubAreas)
                                        .ThenInclude(a => a.ImportanciaEstrategicas)
                            .Include(x => x.Evaluacion)
                            .Where(i => i.EmpresaId == empresa.Id).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public async Task<IEnumerable<EvaluacionEmpresa>> GetEvaluacionEmpresasByEvaluacionIdEmpresaId(Guid evaluacionId, Guid empresaId)
        {
            var retorno = await Context()
                            .EvaluacionEmpresas
                            .Include(x => x.ImportanciaRelativas)
                                .ThenInclude(y => y.SegmentacionArea)
                                    .ThenInclude(e => e.SegmentacionSubAreas)
                                        .ThenInclude(a => a.ImportanciaEstrategicas)
                            .Include(x => x.Evaluacion)
                            .Where(i => i.EmpresaId == empresaId && i.EvaluacionId == evaluacionId).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }

        public List<PorcentajeEvaluacionDto> GetPorcentajeEvaluacion(Guid evaluacionId, Guid empresaId)
        {
            /*var retorno = Context().Evaluacions.
                             Join(Context().EvaluacionEmpresas, e => e.Id, ee => ee.EvaluacionId, (e, ee) => new { EvaluacionEmpresas = ee, Evaluacions = e }).
                             Join(Context().SegmentacionAreas, ea => ea.Evaluacions.Id, sa => sa.Id, (ea, sa) => new { EvaluacionArea = ea, SegmentacionAreas = sa }).
                             Join(Context().Pregunta, ep => ep.EvaluacionArea.Evaluacions.Id, p => p.EvaluacionId, (ep, p) => new { EvaluacionPregunta = p, Pregunta = p }).
                             LeftJoin(Context().Respuesta, pr => pr.Pregunta.Id, r => r.PreguntaId, (ep, p) => new { EvaluacionPregunta = p, Pregunta = p }).
                             Select(pe => new PorcentajeEvaluacionDto {
                                 Nombre = ee,

                             }).
                             AsNoTracking().ToListAsync();*/


            //using (var repo = Context()) {
            /*var context = Context();
                var retorno = (from e in context.Evaluacions
                                   join ee in context.EvaluacionEmpresas on e.Id equals ee.EvaluacionId
                                   join sa in context.SegmentacionAreas on e.Id equals sa.EvaluacionId
                                   join p in context.Pregunta on e.Id equals p.EvaluacionId
                                   join r in context.Respuesta on new { x1 = ee.Id, x2 = p.Id} equals new { x1 = r.EvaluacionEmpresaId, x2 = r.PreguntaId}
                                where  ee.EmpresaId == empresaId && p.Activo == true && e.Id == evaluacionId
                                group e by new {
                                    e.Nombre,
                                    sa.NombreArea,
                                    sa.Id
                                } into groupBy
                               select new PorcentajeEvaluacionDto { 
                                   Nombre = groupBy.Key.Nombre,
                                   NombreArea = groupBy.Key.NombreArea,
                                   SegmentacionAreaId = groupBy.Key.Id,
                                   RespuestaPorcentaje = "0"
                               }).ToList();
            //}

            if (retorno == null) return null;
            return retorno;
            */

            List<PorcentajeEvaluacionDto> lista = new List<PorcentajeEvaluacionDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select
                                                    ev.nombre,
                                                    sa.nombre_area ,
                                                    sa.id as segmentacion_area_id,
                                                    concat((count(re.id) * 100) / count(pr.id)) as respuestas_porc
                                                from
                                                    public.evaluacion ev join
                                                    public.evaluacion_empresa ee 
                                                    on (ev.id = ee.evaluacion_id) join
                                                    public.segmentacion_area sa
                                                    on (sa.evaluacion_id = ev.id) join
                                                    public.pregunta pr
                                                    on (pr.segmentacion_area_id = sa.id) left join
                                                    public.respuesta re
                                                    on(re.pregunta_id = pr.id and re.evaluacion_empresa_id = ee.id)
                                                where ee.empresa_id = '{0}' and ee.evaluacion_id = '{1}' AND pr.activo is true
                                                group by
                                                    ev.nombre,
                                                    sa.nombre_area, 
                                                    sa.id ", empresaId, evaluacionId);

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            PorcentajeEvaluacionDto item = new PorcentajeEvaluacionDto();

                            item.Nombre = (result["nombre"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["segmentacion_area_id"].ToString());
                            item.RespuestaPorcentaje = (result["respuestas_porc"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<EnvioMailTiempoLimiteDto> GetCorreoTiempoLimite(Guid SegmentacionAreaId, Guid empresaId)
        {
            List<EnvioMailTiempoLimiteDto> lista = new List<EnvioMailTiempoLimiteDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select u.email, u.nombres , p.nombre as perfilnombre
                                                    from 
                                                     public.usuario u join 
                                                     public.usuario_evaluacion ue 
                                                     on (ue.usuario_id = u.id) join 
                                                     public.usuario_area ua
                                                     on (ua.usuario_evaluacion_id = ue.id) join 
                                                     public.perfil p 
                                                     on (p.id = u.perfil_id)
                                                     where 
                                                      ua.segmentacion_area_id = '{0}' and ue.empresa_id = '{1}'", SegmentacionAreaId, empresaId);

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            EnvioMailTiempoLimiteDto item = new EnvioMailTiempoLimiteDto();

                            item.Email = (result["email"].ToString());
                            item.Nombre = (result["nombres"].ToString());
                            item.NombrePerfil = (result["perfilnombre"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }


    }
}
