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
using System.Drawing;
using Elastic.Apm.Api.Kubernetes;
using System.Linq.Expressions;

namespace api_public_backOffice.Repository
{
    public interface IIndicadoresRepository : IRepository<Empresa>
    {

        Task<int> CantidadEmpresas();
        int CantidadEmpresasSql();
        int CantidadEmpresasSuscripcion();
        int CantidadGranEmpresa();
        int CantidaEmpresasEvaluacionProceso();
        int CantidadEmpresasEvaluacionFinalizada();
        List<PromedioIMTamanoEmpresaDto> PromedioIMTamanoEmpresa();
        List<PromedioIMRubroDto> PromedioIMRubro();
		List<PromedioIMRubroDto> PromedioIMRubroByEvaluacionId(Evaluacion evalaucion);
		List<PromedioIMTamanoEmpresaDto> PromedioIMTamanoEmpresaByEvaluacionId(Evaluacion evalaucion);
    }
    public class IndicadoresRepository : Repository<Empresa, Context>, IIndicadoresRepository
    {
        public IndicadoresRepository(Context context) : base(context) { }
       
        public async Task<int> CantidadEmpresas()
        {
            return  Context().Empresas.AsNoTracking().Count();
        }
        public int CantidadEmpresasSql()
        {

            //return  Context().Empresas.AsNoTracking().Count();

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@"select  count(id) as cantidad from public.empresa ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        if (result.Read())
                        return int.Parse(result["cantidad"].ToString());
                        else return 0;

                    }
                    else return 0;
                }
            }
        }

        public int CantidadEmpresasSuscripcion()
        {

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@" select  count(distinct ( u.empresa_id  )) as cantidad from 
											usuario_suscripcion us join
											usuario u on
											us.usuario_id =u.id  ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        if (result.Read())
                            return int.Parse(result["cantidad"].ToString());
                        else return 0;

                    }
                    else return 0;
                }
            }
        }

        public int CantidadGranEmpresa()
        {

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@" select count(distinct ( empresa_id)) as cantidad from 
									usuario u join
									perfil p on
									u.perfil_id = p.id
									where p.nombre ='Gran empresa' ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        if (result.Read())
                            return int.Parse(result["cantidad"].ToString());
                        else return 0;

                    }
                    else return 0;
                }
            }
        }

        public int CantidaEmpresasEvaluacionProceso()
        {

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@" select count(rr.respuestas) as cantidad from
										(
										select 
											(count(re.id) * 100) / count(pr.id) as respuestas
											from
												public.empresa em  join
												public.evaluacion_empresa ee on 
												 em.id = ee.empresa_id join 
												public.evaluacion ev on 
												ev.id =ee.evaluacion_id  join 
												public.pregunta pr on
												pr.evaluacion_id  = ee.evaluacion_id left join 
												public.respuesta re on
												re.pregunta_id  = pr.id and re.evaluacion_empresa_id =ee.id 
											group by ee.id, ev.nombre,em.razon_social, ev.tiempo_limite, ev.fecha_creacion, em.id, ev.id
											order by em.razon_social 
										) as rr where rr.respuestas < 100");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        if (result.Read())
                            return int.Parse(result["cantidad"].ToString());
                        else return 0;

                    }
                    else return 0;
                }
            }
        }

        public int CantidadEmpresasEvaluacionFinalizada()
        {

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = string.Format(@" select count(rr.respuestas) as cantidad from
								(
								select 
									(count(re.id) * 100) / count(pr.id) as respuestas
									from
										public.empresa em  join
										public.evaluacion_empresa ee on 
										 em.id = ee.empresa_id join 
										public.evaluacion ev on 
										ev.id =ee.evaluacion_id  join 
										public.pregunta pr on
										pr.evaluacion_id  = ee.evaluacion_id left join 
										public.respuesta re on
										re.pregunta_id  = pr.id and re.evaluacion_empresa_id =ee.id 
									group by ee.id, ev.nombre,em.razon_social, ev.tiempo_limite, ev.fecha_creacion, em.id, ev.id
									order by em.razon_social 
								) as rr where rr.respuestas = 100 ");

                Context().Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        if (result.Read())
                            return int.Parse(result["cantidad"].ToString());
                        else return 0;

                    }
                    else return 0;
                }
            }
        }

        public List<PromedioIMTamanoEmpresaDto> PromedioIMTamanoEmpresa()
        {
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                List<PromedioIMTamanoEmpresaDto> salida = new List<PromedioIMTamanoEmpresaDto>();
                command.CommandText = string.Format(@" select 
                                                        indi.tte_tamano_empresa,
                                                        avg(indi.IM) as PromedioIMTamanoEmpresaDto from (
                                                        select 
	                                                        rrr.pr_evaluacion_id,
	                                                        rrr.ee_empresa_id,
	                                                        rrr.ir_evaluacion_empresa_id,
	                                                        rrr.razon_social,
	                                                        rrr.tte_tamano_empresa,
	                                                        rrr.nombre_evaluacion,
	                                                        (SUM(rrr.peso_relativo_area_porc * rrr.IMA)/100) * 5/4 as IM
	                                                        from
                                                        (select 
	                                                        ttt.pr_evaluacion_id,
	                                                        ttt.ee_empresa_id,
	                                                        ttt.ir_evaluacion_empresa_id,
	                                                        ttt.pr_segmentacion_area_id,
	                                                        ttt.razon_social,
	                                                        ttt.tte_tamano_empresa,
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
	                                                        tte.nombre as tte_tamano_empresa,
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
	                                                        (al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id) join 
	                                                        public.tipo_tamano_empresa tte on
	                                                        (tte.id = e.tipo_tamano_empresa_id)
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
	                                                        tte.nombre,
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
	                                                        ttt.tte_tamano_empresa,
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
	                                                        rrr.tte_tamano_empresa,
	                                                        rrr.nombre_evaluacion 
                                                        )indi
                                                        group by 
	                                                        indi.tte_tamano_empresa ");
                Context().Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            PromedioIMTamanoEmpresaDto elemento = new PromedioIMTamanoEmpresaDto();
                            elemento.NombreTamano = result["tte_tamano_empresa"].ToString();
                            elemento.IMPromedio = decimal.Parse(result["PromedioIMTamanoEmpresaDto"].ToString());
                            salida.Add(elemento);
                        }
                    }
                    return salida;
                }
            }
        }

        public List<PromedioIMRubroDto> PromedioIMRubro()
        {

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                List<PromedioIMRubroDto> salida = new List<PromedioIMRubroDto>();
                command.CommandText = string.Format(@" select 
						indi.tr_tipo_rubro,
						avg(indi.IM) as  PromedioIMRubroDto from (
						select 
							rrr.pr_evaluacion_id,
							rrr.ee_empresa_id,
							rrr.ir_evaluacion_empresa_id,
							rrr.razon_social,
							rrr.tr_tipo_rubro,
							rrr.nombre_evaluacion,
							(SUM(rrr.peso_relativo_area_porc * rrr.IMA)/100) * 5/4 as IM
							from
						(select 
							ttt.pr_evaluacion_id,
							ttt.ee_empresa_id,
							ttt.ir_evaluacion_empresa_id,
							ttt.pr_segmentacion_area_id,
							ttt.razon_social,
							ttt.tr_tipo_rubro,
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
							tte.nombre as tr_tipo_rubro,
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
							(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id) join 
							public.tipo_rubro tte on
							(tte.id = e.tipo_rubro_id)
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
							tte.nombre,
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
							ttt.tr_tipo_rubro,
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
							rrr.tr_tipo_rubro,
							rrr.nombre_evaluacion 
						)indi
						group by 
							indi.tr_tipo_rubro ");
                Context().Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            PromedioIMRubroDto elemento = new PromedioIMRubroDto();
                            elemento.NombreRubro = result["tr_tipo_rubro"].ToString();
                            elemento.IMPromedio = decimal.Parse(result["PromedioIMRubroDto"].ToString());
                            salida.Add(elemento);
                        }
                    }
                    return salida;
                }
            }
        }

        public List<PromedioIMTamanoEmpresaDto> PromedioIMTamanoEmpresaByEvaluacionId(Evaluacion evalaucion)
        {
            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                List<PromedioIMTamanoEmpresaDto> salida = new List<PromedioIMTamanoEmpresaDto>();
                command.CommandText = string.Format(@" select 
                                                        indi.tte_tamano_empresa,
                                                        avg(indi.IM) as PromedioIMTamanoEmpresaDto from (
                                                        select 
	                                                        rrr.pr_evaluacion_id,
	                                                        rrr.ee_empresa_id,
	                                                        rrr.ir_evaluacion_empresa_id,
	                                                        rrr.razon_social,
	                                                        rrr.tte_tamano_empresa,
	                                                        rrr.nombre_evaluacion,
	                                                        (SUM(rrr.peso_relativo_area_porc * rrr.IMA)/100) * 5/4 as IM
	                                                        from
                                                        (select 
	                                                        ttt.pr_evaluacion_id,
	                                                        ttt.ee_empresa_id,
	                                                        ttt.ir_evaluacion_empresa_id,
	                                                        ttt.pr_segmentacion_area_id,
	                                                        ttt.razon_social,
	                                                        ttt.tte_tamano_empresa,
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
	                                                        tte.nombre as tte_tamano_empresa,
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
	                                                        (al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id) join 
	                                                        public.tipo_tamano_empresa tte on
	                                                        (tte.id = e.tipo_tamano_empresa_id)
                                                        where  cast (ti.detalle as  integer) > 0 and ev.id = '{0}'
                                                        group by 
	                                                        pr.evaluacion_id,
	                                                        ee.empresa_id,
	                                                        ir.id,
	                                                        ir.evaluacion_empresa_id,
	                                                        ie.id,
	                                                        pr.segmentacion_area_id,
	                                                        pr.segmentacion_sub_area_id,
	                                                        e.razon_social,
	                                                        tte.nombre,
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
	                                                        ttt.tte_tamano_empresa,
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
	                                                        rrr.tte_tamano_empresa,
	                                                        rrr.nombre_evaluacion 
                                                        )indi
                                                        group by 
	                                                        indi.tte_tamano_empresa ",evalaucion.Id);
                Context().Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            PromedioIMTamanoEmpresaDto elemento = new PromedioIMTamanoEmpresaDto();
                            elemento.NombreTamano = result["tte_tamano_empresa"].ToString();
                            elemento.IMPromedio = decimal.Parse(result["PromedioIMTamanoEmpresaDto"].ToString());
                            salida.Add(elemento);
                        }
                    }
                    return salida;
                }
            }
        }

        public List<PromedioIMRubroDto> PromedioIMRubroByEvaluacionId(Evaluacion evalaucion)
        {

            using (var command = Context().Database.GetDbConnection().CreateCommand())
            {
                List<PromedioIMRubroDto> salida = new List<PromedioIMRubroDto>();
                command.CommandText = string.Format(@" select 
						indi.tr_tipo_rubro,
						avg(indi.IM) as  PromedioIMRubroDto from (
						select 
							rrr.pr_evaluacion_id,
							rrr.ee_empresa_id,
							rrr.ir_evaluacion_empresa_id,
							rrr.razon_social,
							rrr.tr_tipo_rubro,
							rrr.nombre_evaluacion,
							(SUM(rrr.peso_relativo_area_porc * rrr.IMA)/100) * 5/4 as IM
							from
						(select 
							ttt.pr_evaluacion_id,
							ttt.ee_empresa_id,
							ttt.ir_evaluacion_empresa_id,
							ttt.pr_segmentacion_area_id,
							ttt.razon_social,
							ttt.tr_tipo_rubro,
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
							tte.nombre as tr_tipo_rubro,
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
							(al.id = re.alternativa_id and al.evaluacion_id = ee.evaluacion_id) join 
							public.tipo_rubro tte on
							(tte.id = e.tipo_rubro_id)
						where  cast (ti.detalle as  integer) > 0 and ev.id = '{0}'
						group by 
							pr.evaluacion_id,
							ee.empresa_id,
							ir.id,
							ir.evaluacion_empresa_id,
							ie.id,
							pr.segmentacion_area_id,
							pr.segmentacion_sub_area_id,
							e.razon_social,
							tte.nombre,
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
							ttt.tr_tipo_rubro,
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
							rrr.tr_tipo_rubro,
							rrr.nombre_evaluacion 
						)indi
						group by 
							indi.tr_tipo_rubro ",evalaucion.Id);
                Context().Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            PromedioIMRubroDto elemento = new PromedioIMRubroDto();
                            elemento.NombreRubro = result["tr_tipo_rubro"].ToString();
                            elemento.IMPromedio = decimal.Parse(result["PromedioIMRubroDto"].ToString());
                            salida.Add(elemento);
                        }
                    }
                    return salida;
                }
            }
        }
    }
}
