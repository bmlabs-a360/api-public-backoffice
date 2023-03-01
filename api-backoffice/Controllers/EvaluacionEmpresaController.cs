﻿using api_public_backOffice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using api_public_backOffice.Service;
using Microsoft.AspNetCore.Authorization;
using api_public_backOffice.Clients;
using api_public_backOffice.Helpers;
using Microsoft.Extensions.Configuration;
using api_public_backOffice.Interceptors;

using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;
using System.Web;
using neva.entities;
using System.Text.Json;

namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class EvaluacionEmpresaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEvaluacionEmpresaService _EvaluacionEmpresaService;
        private readonly IUsuarioSuscripcionService _usuarioSubscripcionService;
        private readonly IReporteService _reporteService;
        private readonly IMailService _mailService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public EvaluacionEmpresaController(
            EvaluacionEmpresaService EvaluacionEmpresaService,
            UsuarioSuscripcionService usuarioSuscripcionService,
            ReporteService reporteService,
            MailService mailService,
            ILogger<EvaluacionEmpresaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _EvaluacionEmpresaService = EvaluacionEmpresaService;
            _usuarioSubscripcionService = usuarioSuscripcionService;
            _reporteService = reporteService;
            _mailService = mailService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionEmpresaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> GetEvaluacionEmpresaById([FromBody] EvaluacionEmpresaModel EvaluacionEmpresaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.Id.ToString())) return BadRequest("Debe indicar EvaluacionEmpresaModel.Id");
                EvaluacionEmpresaModel retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresaById(EvaluacionEmpresaModel);
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("EvaluacionEmpresaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> EvaluacionEmpresaInsertOrUpdate([FromBody] EvaluacionEmpresaModel EvaluacionEmpresaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.EmpresaId.ToString())) return BadRequest("Debe indicar EmpresaId");
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.EvaluacionId.ToString())) return BadRequest("Debe indicar EvaluacionId");
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.FechaInicioTiempoLimite.ToString())) return BadRequest("Debe indicar FechaInicioTiempoLimite");
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                EvaluacionEmpresaModel retorno = await _EvaluacionEmpresaService.InsertOrUpdate(EvaluacionEmpresaModel);
                if (retorno == null) return NotFound();

                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionEmpresaModel>>> GetEvaluacionEmpresas()
        {
            try
            {
                List<EvaluacionEmpresaModel> retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresas();
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        [HttpPost("GetSeguimiento")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoEvaluacionEmpresaDto>>> GetSeguimiento()
        {
            try
            {
                List<SeguimientoEvaluacionEmpresaDto> retorno =  _EvaluacionEmpresaService.GetSeguimiento();
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


        [HttpPost("GetPlanMejoras")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoPlanMejoraModelDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoPlanMejoraModelDto>>> GetPlanMejoras(EvaluacionEmpresa evaluacionEmpresa)
        {
            try
            {
                List<SeguimientoPlanMejoraModelDto> retorno = _EvaluacionEmpresaService.GetPlanMejoras(evaluacionEmpresa);
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetPlanMejorasReporteSubscripcionOBasico")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoPlanMejoraModelDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoPlanMejoraModelDto>>> GetPlanMejorasReporteSubscripcionOBasico(Guid evaluacionEmpresaId, Guid usuarioId, Guid evaluacionId)
        {
            try
            {

                List<SeguimientoPlanMejoraModelDto> retorno = null;
                UsuarioModel usuario = new UsuarioModel
                {
                    Id = usuarioId
                };
                UsuarioSuscripcionModel usuarioRetorno = await _usuarioSubscripcionService.GetUsuarioSuscripcionsByUsuarioId(usuario);

                if (usuarioRetorno == null)
                {
                    EvaluacionModel evaluacion = new EvaluacionModel
                    {
                        Id = evaluacionId
                    };
                    List<ReporteModel> reporteRetorno = await _reporteService.GetReportesByEvaluacionId(evaluacion);

                    List<Guid> areas = new();
                    foreach (var rr in reporteRetorno)
                    {
                        foreach (var ra in rr.ReporteAreas)
                        {
                            if (ra.Activo == true)
                            {
                                areas.Add(ra.SegmentacionAreaId);
                            }
                        }
                    }
                    retorno = _EvaluacionEmpresaService.GetPlanMejorasReporteSubscripcionOBasico(evaluacionId, areas);
                }
                else
                {
                    EvaluacionEmpresa seguimientoplanmejora = new EvaluacionEmpresa
                    {
                        Id = evaluacionEmpresaId
                    };
                    retorno = _EvaluacionEmpresaService.GetPlanMejoras(seguimientoplanmejora);
                }
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }



        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionEmpresasByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> GetEvaluacionEmpresasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                EvaluacionEmpresaModel retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresasByEvaluacionId(evaluacionModel);
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetEvaluacionEmpresasByEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionEmpresaModel>>> GetEvaluacionEmpresasByEmpresaId(EmpresaModel EmpresaMode)
        {
            try
            {
                List<EvaluacionEmpresaModel> retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresasByEmpresaId(EmpresaMode);
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("DeleteList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> DeleteList(List<EvaluacionEmpresaModel> c)
        {
            try
            {


                await _EvaluacionEmpresaService.DeleteList(c);

                return Ok(true);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
            }
        }

        [HttpPost("InsertOrUpdateList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> InsertOrUpdateList(List<EvaluacionEmpresaModel> c)
        {
            try
            {


                await _EvaluacionEmpresaService.InsertOrUpdateList(c);

                return Ok(true);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
            }
        }

        [HttpGet("GetEvaluacionEmpresasByEvaluacionIdEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionEmpresaModel>>> GetEvaluacionEmpresasByEvaluacionIdEmpresaId(Guid evaluacionId, Guid empresaId)
        {
            try
            {
                List<EvaluacionEmpresaModel> retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresasByEvaluacionIdEmpresaId(evaluacionId, empresaId);
                if (retorno == null) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpGet("EnvioMailTiempoLimite")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> EnvioMailTiempoLimite(Guid evaluacionId, Guid empresaId, string respuestas)
        {
            List<PorcentajeEvaluacionDto> porcentajes = _EvaluacionEmpresaService.GetPorcentajeEvaluacion(evaluacionId, empresaId);

            List<string> correos = new();
            List<string> correosPro = new();

            foreach (PorcentajeEvaluacionDto item in porcentajes)
            {
                if (item == null) return NotFound();

                if (item.RespuestaPorcentaje != "100")
                {
                    List<EnvioMailTiempoLimiteDto> infousuario = _EvaluacionEmpresaService.GetCorreoTiempoLimite(item.SegmentacionAreaId, empresaId);

                    foreach (EnvioMailTiempoLimiteDto info in infousuario)
                    {
                        if (info.NombrePerfil == "Usuario básico")
                        {
                            int indice = correos.IndexOf(info.Email);
                            if (indice == -1)
                            {
                                correos.Add(info.Email);
                                //alias.Add(info.Nombre);

                                try
                                {
                                    MailDTO correo = new MailDTO
                                    {
                                        // From = Configuration["Mail:FromConfirmacion"], //CONFIGURAR FROM
                                        //From = "miloandres7@gmail.com", //CONFIGURAR FROM
                                        From = Configuration["MAIL:FromConfirmacion"],
                                        FromAlias = "Evaluacion",
                                        To = new string[] { info.Email },
                                        ToAlias = new string[] { info.Nombre },
                                        //To = new string[] { "miloandres7@gmail.com"},
                                        //ToAlias = new string[] { "nombre" },
                                        IsHtml = true,
                                        Subject = "¡Completa tu evaluación de madurez en NEVA!",
                                        Body = _emailHelper.GetBodyEmailCompletaEvaluacionUsuarioBasico(respuestas, info.Nombre)
                                    };

                                    string jsonString = JsonSerializer.Serialize(correo);
                                    _ = await _mailService.SendMailAsync(correo);
                                }
                                catch (Exception e)
                                {
                                    while (e.InnerException != null) e = e.InnerException;
                                    _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                                    return Problem(detail: e.Message, title: "ERROR");
                                }
                                /*finally
                                {
                                    _EvaluacionEmpresaService.Dispose();
                                    // _controlTokenService.Dispose();
                                }*/
                            }
                        }
                        else if (info.NombrePerfil == "Usuario pro (empresa)") {
                            int indicePro = correosPro.IndexOf(info.Email);
                            if (indicePro == -1)
                            {
                                correosPro.Add(info.Email);
                                //alias.Add(info.Nombre);

                                try
                                {
                                    MailDTO correo = new MailDTO
                                    {
                                        // From = Configuration["Mail:FromConfirmacion"], //CONFIGURAR FROM
                                        //From = "miloandres7@gmail.com", //CONFIGURAR FROM
                                        From = "miloandres7@gmail.com",
                                        FromAlias = "Evaluacion",
                                        To = new string[] { info.Email },
                                        ToAlias = new string[] { info.Nombre },
                                        //To = new string[] { "miloandres7@gmail.com"},
                                        //ToAlias = new string[] { "nombre" },
                                        IsHtml = true,
                                        Subject = "Actualización sobre la evaluación de madurez empresarial en NEVA",
                                        Body = _emailHelper.GetBodyEmailCompletaEvaluacionUsuarioPro(respuestas, info.Nombre)
                                    };

                                    string jsonString = JsonSerializer.Serialize(correo);
                                    _ = await _mailService.SendMailAsync(correo);
                                }
                                catch (Exception e)
                                {
                                    while (e.InnerException != null) e = e.InnerException;
                                    _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                                    return Problem(detail: e.Message, title: "ERROR");
                                }
                            }
                        } 
                    }
                }

            }

            if (correos == null) return NotFound();
            _EvaluacionEmpresaService.Dispose();
            return Ok(correos);
        }
    }


}
