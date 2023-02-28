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

namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class EmpresaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmpresaService _EmpresaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public EmpresaController(
            EmpresaService EmpresaService,
            ILogger<EmpresaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _EmpresaService = EmpresaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        [ReCaptchaAuth("CompletaDatos")]
        [ApiKeyAuth]
        [HttpPost("CompletaDatos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> CompletaDatos([FromBody] EmpresaModel empresaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(empresaModel.RazonSocial.ToString())) return BadRequest("Debe indicar razón social");
                if (string.IsNullOrEmpty(empresaModel.RutEmpresa.ToString())) return BadRequest("Debe indicar rut empresa");
                if (string.IsNullOrEmpty(empresaModel.TipoRubroId.ToString())) return BadRequest("Debe indicar rubro");
                if (string.IsNullOrEmpty(empresaModel.TipoSubRubroId.ToString())) return BadRequest("Debe indicar sub rubro");
                if (string.IsNullOrEmpty(empresaModel.Comuna.ToString())) return BadRequest("Debe indicar comuna");
                if (string.IsNullOrEmpty(empresaModel.TipoTamanoEmpresaId.ToString())) return BadRequest("Debe indicar tamaño empresa");
                if (string.IsNullOrEmpty(empresaModel.TipoNivelVentaId.ToString())) return BadRequest("Debe indicar nivel venta");
                // if (string.IsNullOrEmpty(empresaModel.TipoRangoId)) return BadRequest("Debe indicar rango");

                empresaModel.Activo = true;

                EmpresaModel retorno = await _EmpresaService.InsertOrUpdate(empresaModel);
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
        [HttpPost("GetEmpresaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EmpresaModel>> GetEmpresaById([FromBody] EmpresaModel empresaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(empresaModel.Id.ToString())) return BadRequest("Debe indicar empresaModel.Id");
                EmpresaModel retorno = await _EmpresaService.GetEmpresaById(empresaModel);
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
                _EmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("EmpresaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EmpresaModel>> EmpresaInsertOrUpdate([FromBody] EmpresaModel empresaModel)
        {
            try
            {


                //if (string.IsNullOrEmpty(empresaModel.Id.ToString()))  return BadRequest("Debe indicar Password");
                if (string.IsNullOrEmpty(empresaModel.RazonSocial)) return BadRequest("Debe indicar RazonSocial");
                if (string.IsNullOrEmpty(empresaModel.RutEmpresa)) return BadRequest("Debe indicar RutEmpresa");
                if (string.IsNullOrEmpty(empresaModel.Comuna)) return BadRequest("Debe indicar Comuna");
                //if (string.IsNullOrEmpty(empresaModel.TipoRubroId.ToString())) return BadRequest("Debe indicar TipoRubroId");
                //if (string.IsNullOrEmpty(empresaModel.TipoSubRubroId.ToString())) return BadRequest("Debe indicar TipoSubRubroId");
                //if (string.IsNullOrEmpty(empresaModel.TipoTamanoEmpresaId.ToString())) return BadRequest("Debe indicar TipoTamanoEmpresaId");
                //if (string.IsNullOrEmpty(empresaModel.TipoNivelVentaId.ToString())) return BadRequest("Debe indicar TipoNivelVentaId");
               // if (string.IsNullOrEmpty(empresaModel.TipoCantidadEmpleadoId.ToString())) return BadRequest("Debe indicar TipoCantidadEmpleadoId");
                // if (string.IsNullOrEmpty(empresaModel.FechaCreacion.ToString()))  return BadRequest("Debe indicar Password");
                if (string.IsNullOrEmpty(empresaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");


                EmpresaModel retorno = await _EmpresaService.InsertOrUpdate(empresaModel);
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
        [HttpGet("GetEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EmpresaModel>>> GetEmpresas()
        {
            try
            {
                List<EmpresaModel> retorno = await _EmpresaService.GetEmpresas();
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
                _EmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        //[ApiKeyAuth]
        [HttpPost("GetEmpresasByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EmpresaModel>>> GetEmpresasByUsuarioId(UsuarioModel usuarioModel)
        {
            try
            {
                List<EmpresaModel> retorno = await _EmpresaService.GetEmpresasByUsuarioId( usuarioModel);
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
                _EmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetEmpresasByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EmpresaModel>>> GetEmpresasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                List<EmpresaModel> retorno = await _EmpresaService.GetEmpresasByEvaluacionId(evaluacionModel);
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
                _EmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }



}
