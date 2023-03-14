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
using static System.Net.Mime.MediaTypeNames;

namespace api_public_backOffice.Repository
{
    public interface IMadurezRepository : IRepository<MadurezCapacidadSubAreaDto>
    {

        List<MadurezCapacidadSubAreaDto> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubArea);
		List<MadurezCapacidadSubAreaDto> GetAllCapacidadSubAreas();

        List<IMSADto> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubArea);
		List<IMSADto> GetAllIMSA();

        List<IMADto> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubArea);
        List<IMADto> GetIMAByAreasUsuarioBasico(Guid evaluacionId, Guid empresaId, List<Guid> areas);
        List<IMADto> GetAllIMA();

        List<IMDto> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubArea);
		List<IMDto> GetAllIM();
    }
    public class MadurezRepository : Repository<MadurezCapacidadSubAreaDto, Context>, IMadurezRepository
    {
        public MadurezRepository(Context context) : base(context) { }

        public List<MadurezCapacidadSubAreaDto> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            List<MadurezCapacidadSubAreaDto> lista = new List<MadurezCapacidadSubAreaDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
	                pr.evaluacion_id as pr_evaluacion_id,
	                ee.empresa_id  as ee_empresa_id,
	                re.pregunta_id as re_pregunta_id,
	                re.id as respuesta_id,
	                re.tipo_importancia_id as re_tipo_importancia_id,
	                re.tipo_diferencia_relacionada_id as re_tipo_diferencia_relacionada_id,
	                ir.id as importancia_relativa_id,
	                ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
	                ie.id as importancia_estrategica_id,
	                pr.segmentacion_area_id as pr_segmentacion_area_id,
	                pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
	                e.razon_social,
	                ev.nombre as nombre_evaluacion,
	                sa.nombre_area,
	                ssa.nombre_sub_area,
	                pr.capacidad as pr_capacidad,
	                pr.detalle as pregunta,
	                al.detalle as respuesta,
	                ir.valor as peso_relativo_area_porc,
	                ie.valor as peso_relativo_subarea_porc,
	                ti.detalle as peso_relativo_capacidad_valor,
	                public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
	                as peso_relativo_capacidad_porc,
	                re.valor as respuesta_valor
                from 
	                public.respuesta re join
	                public.pregunta pr on
	                (pr.id = re.pregunta_id)join 
	                public.evaluacion_empresa ee on
	                (ee.id = re.evaluacion_empresa_id) join 
	                public.importancia_relativa ir on
	                (ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
	                public.importancia_estrategica ie on 
	                (ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
	                public.tipo_importancia ti on
	                (ti.id =re.tipo_importancia_id ) join 	
	                public.segmentacion_area sa	on 
	                (sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
	                public.segmentacion_sub_area ssa on 
	                (ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
	                public.empresa e on 
	                (e.id = ee.empresa_id )join 
	                public.evaluacion ev on
	                (ev.id =ee.evaluacion_id )join 
	                public.alternativa al on
	                (al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
                where ee.empresa_id='{0}' and ee.evaluacion_id ='{1}'
					and cast (ti.detalle as  integer) > 0
                order by e.razon_social,
	                ev.nombre,
	                sa.nombre_area,
	                ssa.nombre_sub_area ", madurezCapacidadSubArea.EmpresaId.ToString(), madurezCapacidadSubArea.EvaluacionId.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            MadurezCapacidadSubAreaDto item = new MadurezCapacidadSubAreaDto();
							try
							{
                                item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                                item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                                item.PreguntaId = Guid.Parse(result["re_pregunta_id"].ToString());
                                item.RespuestaId = Guid.Parse(result["respuesta_id"].ToString());
                                item.TipoImportanciaId = Guid.Parse(result["re_tipo_importancia_id"].ToString());
                                item.TipoDiferenciaRelacionadaId = Guid.Parse(result["re_tipo_diferencia_relacionada_id"].ToString());
                                item.ImportanciaRelativaId = Guid.Parse(result["importancia_relativa_id"].ToString());
                                item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                                item.ImportanciaEstrategicaId = Guid.Parse(result["importancia_estrategica_id"].ToString());
                                item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
                                item.SegmentacionSubAreaId = Guid.Parse(result["pr_segmentacion_sub_area_id"].ToString());
                                item.RazonSocial = (result["razon_social"].ToString());
                                item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                                item.NombreArea = (result["nombre_area"].ToString());
                                item.NombreSubArea = (result["nombre_sub_area"].ToString());
                                item.Capacidad = (result["pr_capacidad"].ToString());
                                item.Pregunta = (result["pregunta"].ToString());
                                item.Respuesta = (result["respuesta"].ToString());
                                item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
                                item.PesoRelativoRubareaPorc = decimal.Parse(result["peso_relativo_subarea_porc"].ToString());
                                item.PesoRelativoCapacidadValor = decimal.Parse(result["peso_relativo_capacidad_valor"].ToString());
                                item.PesoRelativoCapacidadPorc = decimal.Parse(result["peso_relativo_capacidad_porc"].ToString());
                                item.RespuestaValor = decimal.Parse(result["respuesta_valor"].ToString());
                            }
							catch (Exception ex)
							{

								throw new Exception(ex.Message);
							}

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }
        public List<MadurezCapacidadSubAreaDto> GetAllCapacidadSubAreas()
        {
            List<MadurezCapacidadSubAreaDto> lista = new List<MadurezCapacidadSubAreaDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
	                pr.evaluacion_id as pr_evaluacion_id,
	                ee.empresa_id  as ee_empresa_id,
	                re.pregunta_id as re_pregunta_id,
	                re.id as respuesta_id,
	                re.tipo_importancia_id as re_tipo_importancia_id,
	                re.tipo_diferencia_relacionada_id as re_tipo_diferencia_relacionada_id,
	                ir.id as importancia_relativa_id,
	                ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
	                ie.id as importancia_estrategica_id,
	                pr.segmentacion_area_id as pr_segmentacion_area_id,
	                pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
	                e.razon_social,
	                ev.nombre as nombre_evaluacion,
	                sa.nombre_area,
	                ssa.nombre_sub_area,
	                pr.capacidad as pr_capacidad,
	                pr.detalle as pregunta,
	                al.detalle as respuesta,
	                ir.valor as peso_relativo_area_porc,
	                ie.valor as peso_relativo_subarea_porc,
	                ti.detalle as peso_relativo_capacidad_valor,
	                public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
	                as peso_relativo_capacidad_porc,
	                re.valor as respuesta_valor
                from 
	                public.respuesta re join
	                public.pregunta pr on
	                (pr.id = re.pregunta_id)join 
	                public.evaluacion_empresa ee on
	                (ee.id = re.evaluacion_empresa_id) join 
	                public.importancia_relativa ir on
	                (ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
	                public.importancia_estrategica ie on 
	                (ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
	                public.tipo_importancia ti on
	                (ti.id =re.tipo_importancia_id ) join 	
	                public.segmentacion_area sa	on 
	                (sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
	                public.segmentacion_sub_area ssa on 
	                (ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
	                public.empresa e on 
	                (e.id = ee.empresa_id )join 
	                public.evaluacion ev on
	                (ev.id =ee.evaluacion_id )join 
	                public.alternativa al on
	                (al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
                where cast (ti.detalle as  integer) > 0
                order by e.razon_social,
	                ev.nombre,
	                sa.nombre_area,
	                ssa.nombre_sub_area ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            MadurezCapacidadSubAreaDto item = new MadurezCapacidadSubAreaDto();
                            try
                            {
                                item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                                item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                                item.PreguntaId = Guid.Parse(result["re_pregunta_id"].ToString());
                                item.RespuestaId = Guid.Parse(result["respuesta_id"].ToString());
                                item.TipoImportanciaId = Guid.Parse(result["re_tipo_importancia_id"].ToString());
                                item.TipoDiferenciaRelacionadaId = Guid.Parse(result["re_tipo_diferencia_relacionada_id"].ToString());
                                item.ImportanciaRelativaId = Guid.Parse(result["importancia_relativa_id"].ToString());
                                item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                                item.ImportanciaEstrategicaId = Guid.Parse(result["importancia_estrategica_id"].ToString());
                                item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
                                item.SegmentacionSubAreaId = Guid.Parse(result["pr_segmentacion_sub_area_id"].ToString());
                                item.RazonSocial = (result["razon_social"].ToString());
                                item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                                item.NombreArea = (result["nombre_area"].ToString());
                                item.NombreSubArea = (result["nombre_sub_area"].ToString());
                                item.Capacidad = (result["pr_capacidad"].ToString());
                                item.Pregunta = (result["pregunta"].ToString());
                                item.Respuesta = (result["respuesta"].ToString());
                                item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
                                item.PesoRelativoRubareaPorc = decimal.Parse(result["peso_relativo_subarea_porc"].ToString());
                                item.PesoRelativoCapacidadValor = decimal.Parse(result["peso_relativo_capacidad_valor"].ToString());
                                item.PesoRelativoCapacidadPorc = decimal.Parse(result["peso_relativo_capacidad_porc"].ToString());
                                item.RespuestaValor = decimal.Parse(result["respuesta_valor"].ToString());
                            }
                            catch (Exception ex)
                            {

                                throw new Exception(ex.Message);
                            }

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<IMSADto> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            List<IMSADto> lista = new List<IMSADto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
				pr.evaluacion_id as pr_evaluacion_id,
				ee.empresa_id  as ee_empresa_id,
				ir.id as importancia_relativa_id,
				ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
				ie.id as importancia_estrategica_id,
				pr.segmentacion_area_id as pr_segmentacion_area_id,
				pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
				e.razon_social,
				ev.nombre as nombre_evaluacion,
				sa.nombre_area,
				ssa.nombre_sub_area,
				ir.valor as peso_relativo_area_porc,
				ie.valor as peso_relativo_subarea_porc,
				(SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
					* re.valor)/100) * 5 / 4 as IMSA
			from 
				public.respuesta re join
				public.pregunta pr on
				(pr.id = re.pregunta_id)join 
				public.evaluacion_empresa ee on
				(ee.id = re.evaluacion_empresa_id) join 
				public.importancia_relativa ir on
				(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
				public.importancia_estrategica ie on 
				(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
				public.tipo_importancia ti on
				(ti.id =re.tipo_importancia_id ) join 	
				public.segmentacion_area sa	on 
				(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
				public.segmentacion_sub_area ssa on 
				(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
				public.empresa e on 
				(e.id = ee.empresa_id )join 
				public.evaluacion ev on
				(ev.id =ee.evaluacion_id )join 
				public.alternativa al on
				(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
			where ee.empresa_id='{0}' and ee.evaluacion_id ='{1}'
			and cast (ti.detalle as  integer) > 0
			group by 
				pr.evaluacion_id,
				ee.empresa_id,
				ir.id,
				ir.evaluacion_empresa_id,
				ie.id,
				pr.segmentacion_area_id,
				pr.segmentacion_sub_area_id,
				e.razon_social,
				ev.nombre,
				sa.nombre_area,
				ssa.nombre_sub_area,
				ie.valor
			order by e.razon_social,
				ev.nombre,
				sa.nombre_area,
				ssa.nombre_sub_area ", madurezCapacidadSubArea.EmpresaId.ToString(), madurezCapacidadSubArea.EvaluacionId.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            IMSADto item = new IMSADto();
                            item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                            item.ImportanciaRelativaId = Guid.Parse(result["importancia_relativa_id"].ToString());
                            item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                            item.ImportanciaEstrategicaId = Guid.Parse(result["importancia_estrategica_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
                            item.SegmentacionSubAreaId = Guid.Parse(result["pr_segmentacion_sub_area_id"].ToString());
                            item.RazonSocial = (result["razon_social"].ToString());
                            item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.NombreSubArea = (result["nombre_sub_area"].ToString());
                            item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
                            item.PesoRelativoRubareaPorc = decimal.Parse(result["peso_relativo_subarea_porc"].ToString());
                            item.IMSAValor = decimal.Parse(result["IMSA"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }
        public List<IMSADto> GetAllIMSA( )
        {
            List<IMSADto> lista = new List<IMSADto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
				pr.evaluacion_id as pr_evaluacion_id,
				ee.empresa_id  as ee_empresa_id,
				ir.id as importancia_relativa_id,
				ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
				ie.id as importancia_estrategica_id,
				pr.segmentacion_area_id as pr_segmentacion_area_id,
				pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
				e.razon_social,
				ev.nombre as nombre_evaluacion,
				sa.nombre_area,
				ssa.nombre_sub_area,
				ir.valor as peso_relativo_area_porc,
				ie.valor as peso_relativo_subarea_porc,
				SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
					* re.valor)/100 as IMSA
			from 
				public.respuesta re join
				public.pregunta pr on
				(pr.id = re.pregunta_id)join 
				public.evaluacion_empresa ee on
				(ee.id = re.evaluacion_empresa_id) join 
				public.importancia_relativa ir on
				(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
				public.importancia_estrategica ie on 
				(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
				public.tipo_importancia ti on
				(ti.id =re.tipo_importancia_id ) join 	
				public.segmentacion_area sa	on 
				(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
				public.segmentacion_sub_area ssa on 
				(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
				public.empresa e on 
				(e.id = ee.empresa_id )join 
				public.evaluacion ev on
				(ev.id =ee.evaluacion_id )join 
				public.alternativa al on
				(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
			where  cast (ti.detalle as  integer) > 0
			group by 
				pr.evaluacion_id,
				ee.empresa_id,
				ir.id,
				ir.evaluacion_empresa_id,
				ie.id,
				pr.segmentacion_area_id,
				pr.segmentacion_sub_area_id,
				e.razon_social,
				ev.nombre,
				sa.nombre_area,
				ssa.nombre_sub_area,
				ie.valor
			order by e.razon_social,
				ev.nombre,
				sa.nombre_area,
				ssa.nombre_sub_area ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            IMSADto item = new IMSADto();
                            item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                            item.ImportanciaRelativaId = Guid.Parse(result["importancia_relativa_id"].ToString());
                            item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                            item.ImportanciaEstrategicaId = Guid.Parse(result["importancia_estrategica_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
                            item.SegmentacionSubAreaId = Guid.Parse(result["pr_segmentacion_sub_area_id"].ToString());
                            item.RazonSocial = (result["razon_social"].ToString());
                            item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.NombreSubArea = (result["nombre_sub_area"].ToString());
                            item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
                            item.PesoRelativoRubareaPorc = decimal.Parse(result["peso_relativo_subarea_porc"].ToString());
                            item.IMSAValor = decimal.Parse(result["IMSA"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<IMADto> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            List<IMADto> lista = new List<IMADto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
					ttt.pr_evaluacion_id,
					ttt.ee_empresa_id,
					ttt.ir_evaluacion_empresa_id,
					ttt.pr_segmentacion_area_id,
					ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area,
					ttt.peso_relativo_area_porc,
					SUM(ttt.peso_relativo_subarea_porc * ttt.IMSA)/100  as IMA
					from (
				select 
					pr.evaluacion_id as pr_evaluacion_id,
					ee.empresa_id  as ee_empresa_id,
					ir.id as importancia_relativa_id,
					ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
					ie.id as importancia_estrategica_id,
					pr.segmentacion_area_id as pr_segmentacion_area_id,
					pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
					e.razon_social,
					ev.nombre as nombre_evaluacion,
					sa.nombre_area,
					ssa.nombre_sub_area,
					ir.valor as peso_relativo_area_porc,
					ie.valor as peso_relativo_subarea_porc,
					(SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
						* re.valor)/100) * 5 / 4 as IMSA
				from 
					public.respuesta re join
					public.pregunta pr on
					(pr.id = re.pregunta_id)join 
					public.evaluacion_empresa ee on
					(ee.id = re.evaluacion_empresa_id) join 
					public.importancia_relativa ir on
					(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
					public.importancia_estrategica ie on 
					(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
					public.tipo_importancia ti on
					(ti.id =re.tipo_importancia_id ) join 	
					public.segmentacion_area sa	on 
					(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
					public.segmentacion_sub_area ssa on 
					(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
					public.empresa e on 
					(e.id = ee.empresa_id )join 
					public.evaluacion ev on
					(ev.id =ee.evaluacion_id )join 
					public.alternativa al on
					(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
				where ee.empresa_id='{0}' and ee.evaluacion_id ='{1}'
				and cast (ti.detalle as  integer) > 0
				group by 
					pr.evaluacion_id,
					ee.empresa_id,
					ir.id,
					ir.evaluacion_empresa_id,
					ie.id,
					pr.segmentacion_area_id,
					pr.segmentacion_sub_area_id,
					e.razon_social,
					ev.nombre,
					sa.nombre_area,
					ssa.nombre_sub_area,
					ie.valor
				order by e.razon_social,
					ev.nombre,
					sa.nombre_area,
					ssa.nombre_sub_area ) ttt
					group by 
					ttt.pr_evaluacion_id,
					ttt.ee_empresa_id,
					ttt.ir_evaluacion_empresa_id,
					ttt.pr_segmentacion_area_id,
					ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area,
					ttt.peso_relativo_area_porc
				order by ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area ", madurezCapacidadSubArea.EmpresaId.ToString(), madurezCapacidadSubArea.EvaluacionId.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            IMADto item = new IMADto();
                            item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                            item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
                            item.RazonSocial = (result["razon_social"].ToString());
                            item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
                            item.IMAValor = decimal.Parse(result["IMA"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

		public List<IMADto> GetIMAByAreasUsuarioBasico(Guid evaluacionId, Guid empresaId, List<Guid> areas)
		{
			List<IMADto> lista = new List<IMADto>();
			using (var command = Context().Database.GetDbConnection().CreateCommand())
			{
				command.CommandText = string.Format(@"select 
					ttt.pr_evaluacion_id,
					ttt.ee_empresa_id,
					ttt.ir_evaluacion_empresa_id,
					ttt.pr_segmentacion_area_id,
					ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area,
					ttt.peso_relativo_area_porc,
					SUM(ttt.peso_relativo_subarea_porc * ttt.IMSA)/100  as IMA
					from (
				select 
					pr.evaluacion_id as pr_evaluacion_id,
					ee.empresa_id  as ee_empresa_id,
					ir.id as importancia_relativa_id,
					ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
					ie.id as importancia_estrategica_id,
					pr.segmentacion_area_id as pr_segmentacion_area_id,
					pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
					e.razon_social,
					ev.nombre as nombre_evaluacion,
					sa.nombre_area,
					ssa.nombre_sub_area,
					ir.valor as peso_relativo_area_porc,
					ie.valor as peso_relativo_subarea_porc,
					(SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
						* re.valor)/100) * 5 / 4 as IMSA
				from 
					public.respuesta re join
					public.pregunta pr on
					(pr.id = re.pregunta_id)join 
					public.evaluacion_empresa ee on
					(ee.id = re.evaluacion_empresa_id) join 
					public.importancia_relativa ir on
					(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
					public.importancia_estrategica ie on 
					(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
					public.tipo_importancia ti on
					(ti.id =re.tipo_importancia_id ) join 	
					public.segmentacion_area sa	on 
					(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
					public.segmentacion_sub_area ssa on 
					(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
					public.empresa e on 
					(e.id = ee.empresa_id )join 
					public.evaluacion ev on
					(ev.id =ee.evaluacion_id )join 
					public.alternativa al on
					(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
				where ee.empresa_id='{0}' and ee.evaluacion_id ='{1}'
				and cast (ti.detalle as  integer) > 0
				group by 
					pr.evaluacion_id,
					ee.empresa_id,
					ir.id,
					ir.evaluacion_empresa_id,
					ie.id,
					pr.segmentacion_area_id,
					pr.segmentacion_sub_area_id,
					e.razon_social,
					ev.nombre,
					sa.nombre_area,
					ssa.nombre_sub_area,
					ie.valor
				order by e.razon_social,
					ev.nombre,
					sa.nombre_area,
					ssa.nombre_sub_area ) ttt
					group by 
					ttt.pr_evaluacion_id,
					ttt.ee_empresa_id,
					ttt.ir_evaluacion_empresa_id,
					ttt.pr_segmentacion_area_id,
					ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area,
					ttt.peso_relativo_area_porc
				order by ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area ", empresaId.ToString(), evaluacionId.ToString());

				Context().Database.OpenConnection();

				using (var result = command.ExecuteReader())
				{
					if (result.HasRows)
					{
						while (result.Read())
						{
							IMADto item = new IMADto();
							item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
							item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
							item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
							item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
							item.RazonSocial = (result["razon_social"].ToString());
							item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
							item.NombreArea = (result["nombre_area"].ToString());
							item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
							item.IMAValor = decimal.Parse(result["IMA"].ToString());

							lista.Add(item);
						}
					}
				}
			}

            List<IMADto> listaRetono = new List<IMADto>();
			foreach (var ar in areas) 
			{ 

				foreach (var li in lista)
				{
					if (li.SegmentacionAreaId == ar) {
						listaRetono.Add(li);
					}
				}
			}
            
        EvaluacionModel evaluacion = new EvaluacionModel{ Id = evaluacionId};
            // List<ReporteModel> reporteRetorno = await _reporteService.GetReportesByEvaluacionId(evaluacion);


            var reporteRetorno =  Context()
                          .Reportes
                           .Include(ri => ri.ReporteItems)
                              .ThenInclude(ti => ti.TipoItemReporte)
                          .Include(x => x.ReporteItemNivelBasicos.OrderByDescending(x => x.Orden))
                          .Include(a => a.ReporteAreas)
                          .Where(y => y.EvaluacionId == evaluacion.Id && y.Activo.Value).AsNoTracking();

            foreach (var rr in reporteRetorno)
				foreach (var ra in rr.ReporteAreas)
					listaRetono.ForEach(r => { 
						if (r.SegmentacionAreaId== ra.SegmentacionAreaId && ra.Activo == true) 
							r.ActivaArea = true;
						 });
               
 

            return listaRetono;
        }

        public List<IMADto> GetAllIMA()
        {
            List<IMADto> lista = new List<IMADto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
					ttt.pr_evaluacion_id,
					ttt.ee_empresa_id,
					ttt.ir_evaluacion_empresa_id,
					ttt.pr_segmentacion_area_id,
					ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area,
					ttt.peso_relativo_area_porc,
					SUM(ttt.peso_relativo_subarea_porc * ttt.IMSA)/100  as IMA
					from (
				select 
					pr.evaluacion_id as pr_evaluacion_id,
					ee.empresa_id  as ee_empresa_id,
					ir.id as importancia_relativa_id,
					ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
					ie.id as importancia_estrategica_id,
					pr.segmentacion_area_id as pr_segmentacion_area_id,
					pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
					e.razon_social,
					ev.nombre as nombre_evaluacion,
					sa.nombre_area,
					ssa.nombre_sub_area,
					ir.valor as peso_relativo_area_porc,
					ie.valor as peso_relativo_subarea_porc,
					SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
						* re.valor)/100 as IMSA
				from 
					public.respuesta re join
					public.pregunta pr on
					(pr.id = re.pregunta_id)join 
					public.evaluacion_empresa ee on
					(ee.id = re.evaluacion_empresa_id) join 
					public.importancia_relativa ir on
					(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
					public.importancia_estrategica ie on 
					(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
					public.tipo_importancia ti on
					(ti.id =re.tipo_importancia_id ) join 	
					public.segmentacion_area sa	on 
					(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
					public.segmentacion_sub_area ssa on 
					(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
					public.empresa e on 
					(e.id = ee.empresa_id )join 
					public.evaluacion ev on
					(ev.id =ee.evaluacion_id )join 
					public.alternativa al on
					(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
				where  cast (ti.detalle as  integer) > 0
				group by 
					pr.evaluacion_id,
					ee.empresa_id,
					ir.id,
					ir.evaluacion_empresa_id,
					ie.id,
					pr.segmentacion_area_id,
					pr.segmentacion_sub_area_id,
					e.razon_social,
					ev.nombre,
					sa.nombre_area,
					ssa.nombre_sub_area,
					ie.valor
				order by e.razon_social,
					ev.nombre,
					sa.nombre_area,
					ssa.nombre_sub_area ) ttt
					group by 
					ttt.pr_evaluacion_id,
					ttt.ee_empresa_id,
					ttt.ir_evaluacion_empresa_id,
					ttt.pr_segmentacion_area_id,
					ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area,
					ttt.peso_relativo_area_porc
				order by ttt.razon_social,
					ttt.nombre_evaluacion,
					ttt.nombre_area ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            IMADto item = new IMADto();
                            item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                            item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                            item.SegmentacionAreaId = Guid.Parse(result["pr_segmentacion_area_id"].ToString());
                            item.RazonSocial = (result["razon_social"].ToString());
                            item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                            item.NombreArea = (result["nombre_area"].ToString());
                            item.PesoRelativoAreaPorc = decimal.Parse(result["peso_relativo_area_porc"].ToString());
                            item.IMAValor = decimal.Parse(result["IMA"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<IMDto> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            List<IMDto> lista = new List<IMDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
						rrr.pr_evaluacion_id,
						rrr.ee_empresa_id,
						rrr.ir_evaluacion_empresa_id,
						rrr.razon_social,
						rrr.nombre_evaluacion,
						(SUM(rrr.peso_relativo_area_porc * rrr.IMA)/100) *5/4 as IM
						from
					(select 
						ttt.pr_evaluacion_id,
						ttt.ee_empresa_id,
						ttt.ir_evaluacion_empresa_id,
						ttt.pr_segmentacion_area_id,
						ttt.razon_social,
						ttt.nombre_evaluacion,
						ttt.nombre_area,
						ttt.peso_relativo_area_porc,
						SUM(ttt.peso_relativo_subarea_porc * ttt.IMSA)/100  as IMA
						from (
					select 
						pr.evaluacion_id as pr_evaluacion_id,
						ee.empresa_id  as ee_empresa_id,
						ir.id as importancia_relativa_id,
						ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
						ie.id as importancia_estrategica_id,
						pr.segmentacion_area_id as pr_segmentacion_area_id,
						pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
						e.razon_social,
						ev.nombre as nombre_evaluacion,
						sa.nombre_area,
						ssa.nombre_sub_area,
						ir.valor as peso_relativo_area_porc,
						ie.valor as peso_relativo_subarea_porc,
						SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
							* re.valor)/100 as IMSA
					from 
						public.respuesta re join
						public.pregunta pr on
						(pr.id = re.pregunta_id)join 
						public.evaluacion_empresa ee on
						(ee.id = re.evaluacion_empresa_id) join 
						public.importancia_relativa ir on
						(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
						public.importancia_estrategica ie on 
						(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
						public.tipo_importancia ti on
						(ti.id =re.tipo_importancia_id ) join 	
						public.segmentacion_area sa	on 
						(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
						public.segmentacion_sub_area ssa on 
						(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
						public.empresa e on 
						(e.id = ee.empresa_id )join 
						public.evaluacion ev on
						(ev.id =ee.evaluacion_id )join 
						public.alternativa al on
						(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
					where ee.empresa_id='{0}' and ee.evaluacion_id ='{1}'
					and cast (ti.detalle as  integer) > 0
					group by 
						pr.evaluacion_id,
						ee.empresa_id,
						ir.id,
						ir.evaluacion_empresa_id,
						ie.id,
						pr.segmentacion_area_id,
						pr.segmentacion_sub_area_id,
						e.razon_social,
						ev.nombre,
						sa.nombre_area,
						ssa.nombre_sub_area,
						ie.valor
					order by e.razon_social,
						ev.nombre,
						sa.nombre_area,
						ssa.nombre_sub_area ) ttt
						group by 
						ttt.pr_evaluacion_id,
						ttt.ee_empresa_id,
						ttt.ir_evaluacion_empresa_id,
						ttt.pr_segmentacion_area_id,
						ttt.razon_social,
						ttt.nombre_evaluacion,
						ttt.nombre_area,
						ttt.peso_relativo_area_porc
					order by ttt.razon_social,
						ttt.nombre_evaluacion,
						ttt.nombre_area)rrr
						group by 
						rrr.pr_evaluacion_id,
						rrr.ee_empresa_id,
						rrr.ir_evaluacion_empresa_id,
						rrr.razon_social,
						rrr.nombre_evaluacion ", madurezCapacidadSubArea.EmpresaId.ToString(), madurezCapacidadSubArea.EvaluacionId.ToString());

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            IMDto item = new IMDto();
                            item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                            item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                            item.RazonSocial = (result["razon_social"].ToString());
                            item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                            item.IMValor = decimal.Parse(result["IM"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }
        public List<IMDto> GetAllIM()
        {
            List<IMDto> lista = new List<IMDto>();
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select 
						rrr.pr_evaluacion_id,
						rrr.ee_empresa_id,
						rrr.ir_evaluacion_empresa_id,
						rrr.razon_social,
						rrr.nombre_evaluacion,
						(SUM(rrr.peso_relativo_area_porc * rrr.IMA)/100) * 5/4 as IM
						from
					(select 
						ttt.pr_evaluacion_id,
						ttt.ee_empresa_id,
						ttt.ir_evaluacion_empresa_id,
						ttt.pr_segmentacion_area_id,
						ttt.razon_social,
						ttt.nombre_evaluacion,
						ttt.nombre_area,
						ttt.peso_relativo_area_porc,
						SUM(ttt.peso_relativo_subarea_porc * ttt.IMSA)/100  as IMA
						from (
					select 
						pr.evaluacion_id as pr_evaluacion_id,
						ee.empresa_id  as ee_empresa_id,
						ir.id as importancia_relativa_id,
						ir.evaluacion_empresa_id as ir_evaluacion_empresa_id,
						ie.id as importancia_estrategica_id,
						pr.segmentacion_area_id as pr_segmentacion_area_id,
						pr.segmentacion_sub_area_id as pr_segmentacion_sub_area_id,
						e.razon_social,
						ev.nombre as nombre_evaluacion,
						sa.nombre_area,
						ssa.nombre_sub_area,
						ir.valor as peso_relativo_area_porc,
						ie.valor as peso_relativo_subarea_porc,
						SUM(public.func_capacidad(ee.empresa_id,ee.evaluacion_id,pr.segmentacion_area_id,pr.segmentacion_sub_area_id,cast (ti.detalle as  integer))
							* re.valor)/100 as IMSA
					from 
						public.respuesta re join
						public.pregunta pr on
						(pr.id = re.pregunta_id)join 
						public.evaluacion_empresa ee on
						(ee.id = re.evaluacion_empresa_id) join 
						public.importancia_relativa ir on
						(ir.evaluacion_empresa_id=ee.id and ir.segmentacion_area_id = pr.segmentacion_area_id)join
						public.importancia_estrategica ie on 
						(ie.importancia_relativa_id = ir.id and ie.segmentacion_sub_area_id = pr.segmentacion_sub_area_id) join 
						public.tipo_importancia ti on
						(ti.id =re.tipo_importancia_id ) join 	
						public.segmentacion_area sa	on 
						(sa.id = pr.segmentacion_area_id and sa.evaluacion_id =ee.evaluacion_id)join 
						public.segmentacion_sub_area ssa on 
						(ssa.segmentacion_area_id = pr.segmentacion_area_id and ssa.id = pr.segmentacion_sub_area_id) join 
						public.empresa e on 
						(e.id = ee.empresa_id )join 
						public.evaluacion ev on
						(ev.id =ee.evaluacion_id )join 
						public.alternativa al on
						(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id)
					where  cast (ti.detalle as  integer) > 0
					group by 
						pr.evaluacion_id,
						ee.empresa_id,
						ir.id,
						ir.evaluacion_empresa_id,
						ie.id,
						pr.segmentacion_area_id,
						pr.segmentacion_sub_area_id,
						e.razon_social,
						ev.nombre,
						sa.nombre_area,
						ssa.nombre_sub_area,
						ie.valor
					order by e.razon_social,
						ev.nombre,
						sa.nombre_area,
						ssa.nombre_sub_area ) ttt
						group by 
						ttt.pr_evaluacion_id,
						ttt.ee_empresa_id,
						ttt.ir_evaluacion_empresa_id,
						ttt.pr_segmentacion_area_id,
						ttt.razon_social,
						ttt.nombre_evaluacion,
						ttt.nombre_area,
						ttt.peso_relativo_area_porc
					order by ttt.razon_social,
						ttt.nombre_evaluacion,
						ttt.nombre_area)rrr
						group by 
						rrr.pr_evaluacion_id,
						rrr.ee_empresa_id,
						rrr.ir_evaluacion_empresa_id,
						rrr.razon_social,
						rrr.nombre_evaluacion ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            IMDto item = new IMDto();
                            item.EvaluacionId = Guid.Parse(result["pr_evaluacion_id"].ToString());
                            item.EmpresaId = Guid.Parse(result["ee_empresa_id"].ToString());
                            item.EvaluacionEmpresaId = Guid.Parse(result["ir_evaluacion_empresa_id"].ToString());
                            item.RazonSocial = (result["razon_social"].ToString());
                            item.NombreEvaluacion = (result["nombre_evaluacion"].ToString());
                            item.IMValor = decimal.Parse(result["IM"].ToString());

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }








    }
}
