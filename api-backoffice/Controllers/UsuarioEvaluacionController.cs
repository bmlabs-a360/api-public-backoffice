using api_public_backOffice.Models;
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

namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsuarioEvaluacionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsuarioEvaluacionService _UsuarioEvaluacionService;
        private readonly IEvaluacionEmpresaService _EvaluacionEmpresaService;
        private readonly IUsuarioAreaService _UsuarioAreaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public UsuarioEvaluacionController(
            UsuarioEvaluacionService UsuarioEvaluacionService,
            EvaluacionEmpresaService EvaluacionEmpresaService,
             UsuarioAreaService UsuarioAreaService,
            ILogger<UsuarioEvaluacionController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _UsuarioEvaluacionService = UsuarioEvaluacionService;
            _EvaluacionEmpresaService = EvaluacionEmpresaService;
            _UsuarioAreaService = UsuarioAreaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEvaluacionById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioEvaluacionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioEvaluacionModel>> GetUsuarioEvaluacionById([FromBody] UsuarioEvaluacionModel UsuarioEvaluacionModel)

        {
            try
            {
                if (string.IsNullOrEmpty(UsuarioEvaluacionModel.Id.ToString())) return BadRequest("Debe indicar UsuarioEvaluacionModel.Id");
                UsuarioEvaluacionModel retorno = await _UsuarioEvaluacionService.GetUsuarioEvaluacionById(UsuarioEvaluacionModel);
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
                _UsuarioEvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEvaluacions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioEvaluacionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioEvaluacionModel>>> GetUsuarioEvaluacions()
        {
            try
            {
                List<UsuarioEvaluacionModel> retorno = await _UsuarioEvaluacionService.GetUsuarioEvaluacions();
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
                _UsuarioEvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEvaluacionsByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioEvaluacionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioEvaluacionModel>>> GetUsuarioEvaluacionsByUsuarioId(UsuarioModel usuarioModel)
        {
            try
            {
                List<UsuarioEvaluacionModel> retorno = await _UsuarioEvaluacionService.GetUsuarioEvaluacionsByUsuarioId(usuarioModel);
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
                _UsuarioEvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEvaluacionsByEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioEvaluacionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioEvaluacionModel>>> GetUsuarioEvaluacionsByEmpresaId(EmpresaModel empresaModel)
        {
            try
            {
                List<UsuarioEvaluacionModel> retorno = await _UsuarioEvaluacionService.GetUsuarioEvaluacionsByEmpresaId(empresaModel);
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
                _UsuarioEvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEvaluacionsByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioEvaluacionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioEvaluacionModel>>> GetUsuarioEvaluacionsByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                List<UsuarioEvaluacionModel> retorno = await _UsuarioEvaluacionService.GetUsuarioEvaluacionsByEvaluacionId(evaluacionModel);
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
                _UsuarioEvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("InsertUsuarioEvaluacionConsultorOrEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioEmpresaModel>> InsertUsuarioEvaluacionConsultorOrEmpresa([FromBody] List<UsuarioEmpresaModel> usuarioEmpresaModel)
        {
            try
            {
                foreach (var ue in usuarioEmpresaModel)
                {
                    EmpresaModel evaluacionEmpresa = new EmpresaModel
                    {
                        Id = ue.EmpresaId
                    };

                    List<EvaluacionEmpresaModel> evaluacionEmpresas = await _EvaluacionEmpresaService.GetEvaluacionEmpresasByEmpresaId(evaluacionEmpresa);

                    foreach (var e in evaluacionEmpresas)
                    {
                        UsuarioEvaluacionModel usuarioEvaluacion = new UsuarioEvaluacionModel
                        {
                            UsuarioId = ue.UsuarioId,
                            EmpresaId = ue.EmpresaId,
                            EvaluacionId = e.EvaluacionId,
                            Activo = true,
                        };

                        UsuarioEvaluacionModel nuevoUsuarioEvaluacion = await _UsuarioEvaluacionService.InsertOrUpdate(usuarioEvaluacion);

                        foreach (var importanciarelativa in e.ImportanciaRelativas)
                        {
                            if (importanciarelativa.SegmentacionArea.Activo == true)
                            {
                                UsuarioAreaModel usuarioArea = new UsuarioAreaModel
                                {
                                    UsuarioEvaluacionId = nuevoUsuarioEvaluacion.Id,
                                    SegmentacionAreaId = importanciarelativa.SegmentacionAreaId,
                                    Activo = true,
                                };
                                _ = await _UsuarioAreaService.InsertOrUpdate(usuarioArea);
                            }
                        }
                    }
                }
                return Ok(true);
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
