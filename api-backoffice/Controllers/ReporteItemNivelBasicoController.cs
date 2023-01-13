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
    public class ReporteItemNivelBasicoController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReporteItemNivelBasicoService _ReporteItemNivelBasicoService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ReporteItemNivelBasicoController(
            ReporteItemNivelBasicoService ReporteItemNivelBasicoService,
            ILogger<ReporteItemNivelBasicoController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ReporteItemNivelBasicoService = ReporteItemNivelBasicoService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetReporteItemNivelBasicoById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteItemNivelBasicoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteItemNivelBasicoModel>> GetReporteItemNivelBasicoById([FromBody] ReporteItemNivelBasicoModel ReporteItemNivelBasicoModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ReporteItemNivelBasicoModel.Id.ToString())) return BadRequest("Debe indicar ReporteItemNivelBasicoModel.Id");
                ReporteItemNivelBasicoModel retorno = await _ReporteItemNivelBasicoService.GetReporteItemNivelBasicoById(ReporteItemNivelBasicoModel);
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
                _ReporteItemNivelBasicoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ReporteItemNivelBasicoInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteItemNivelBasicoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteItemNivelBasicoModel>> ReporteItemNivelBasicoInsertOrUpdate([FromBody] ReporteItemNivelBasicoModel ReporteItemNivelBasicoModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ReporteItemNivelBasicoModel.ReporteId.ToString())) return BadRequest("Debe indicar ReporteId");

                ReporteItemNivelBasicoModel retorno = await _ReporteItemNivelBasicoService.InsertOrUpdate(ReporteItemNivelBasicoModel);
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
        [HttpGet("GetReporteItemNivelBasicos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteItemNivelBasicoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteItemNivelBasicoModel>>> GetReporteItemNivelBasicos()
        {
            try
            {
                List<ReporteItemNivelBasicoModel> retorno = await _ReporteItemNivelBasicoService.GetReporteItemNivelBasicos();
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
                _ReporteItemNivelBasicoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteItemNivelBasicosByReporteId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteItemNivelBasicoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteItemNivelBasicoModel>>> GetReporteItemNivelBasicosByReporteId(ReporteModel reporteModel)
        {
            try
            {
                List<ReporteItemNivelBasicoModel> retorno = await _ReporteItemNivelBasicoService.GetReporteItemNivelBasicosByReporteId(reporteModel);
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
                _ReporteItemNivelBasicoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
