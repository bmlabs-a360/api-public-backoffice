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
    public class SeguimientoController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISeguimientoService _SeguimientoService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public SeguimientoController(
            SeguimientoService SeguimientoService,
            ILogger<SeguimientoController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _SeguimientoService = SeguimientoService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetSeguimientoById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SeguimientoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SeguimientoModel>> GetSeguimientoById([FromBody] SeguimientoModel SeguimientoModel)

        {
            try
            {
                if (string.IsNullOrEmpty(SeguimientoModel.Id.ToString())) return BadRequest("Debe indicar SeguimientoModel.Id");
                SeguimientoModel retorno = await _SeguimientoService.GetSeguimientoById(SeguimientoModel);
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
                _SeguimientoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("SeguimientoInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SeguimientoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SeguimientoModel>> SeguimientoInsertOrUpdate([FromBody] SeguimientoModel SeguimientoModel)
        {
            try
            {
                if (string.IsNullOrEmpty(SeguimientoModel.EmpresaId.ToString())) return BadRequest("Debe indicar EmpresaId");
                if (string.IsNullOrEmpty(SeguimientoModel.EvaluacionId.ToString())) return BadRequest("Debe indicar EvaluacionId");
                if (string.IsNullOrEmpty(SeguimientoModel.FechaUltimoAcceso.ToString())) return BadRequest("Debe indicar NombreSubArea");
                if (string.IsNullOrEmpty(SeguimientoModel.Madurez.ToString())) return BadRequest("Debe indicar Madurez");
                if (string.IsNullOrEmpty(SeguimientoModel.PlanMejoraId.ToString())) return BadRequest("Debe indicar NombreSubArea");
                if (string.IsNullOrEmpty(SeguimientoModel.PorcentajePlaMejora.ToString())) return BadRequest("Debe indicar PorcentajePlaMejora");
                if (string.IsNullOrEmpty(SeguimientoModel.PorcentajeRespuestas.ToString())) return BadRequest("Debe indicar PorcentajeRespuestas");
                if (string.IsNullOrEmpty(SeguimientoModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                SeguimientoModel retorno = await _SeguimientoService.InsertOrUpdate(SeguimientoModel);
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
        [HttpPost("GetSeguimientos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoModel>>> GetSeguimientos()
        {
            try
            {
                List<SeguimientoModel> retorno = await _SeguimientoService.GetSeguimientos();
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
                _SeguimientoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetSeguimientosByEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoModel>>> GetSeguimientosByEmpresaId(EmpresaModel empresaModel)
        {
            try
            {
                List<SeguimientoModel> retorno = await _SeguimientoService.GetSeguimientosByEmpresaId(empresaModel);
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
                _SeguimientoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetSeguimientosByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoModel>>> GetSeguimientosByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                List<SeguimientoModel> retorno = await _SeguimientoService.GetSeguimientosByEvaluacionId(evaluacionModel);
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
                _SeguimientoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetSeguimientosByPlanMejoraId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoModel>>> GetSeguimientosByPlanMejoraId(PlanMejoraModel planMejoraModel)
        {
            try
            {
                List<SeguimientoModel> retorno = await _SeguimientoService.GetSeguimientosByPlanMejoraId(planMejoraModel);
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
                _SeguimientoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
    }
}
