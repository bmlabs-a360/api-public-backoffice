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
    public class ReporteItemNivelSubscripcionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReporteItemNivelSubscripcionService _ReporteItemNivelSubscripcionService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ReporteItemNivelSubscripcionController(
            ReporteItemNivelSubscripcionService ReporteItemNivelSubscripcionService,
            ILogger<ReporteItemNivelSubscripcionController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ReporteItemNivelSubscripcionService = ReporteItemNivelSubscripcionService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetReporteItemNivelSubscripcionById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteItemNivelSubscripcionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteItemNivelSubscripcionModel>> GetReporteItemNivelSubscripcionById([FromBody] ReporteItemNivelSubscripcionModel ReporteItemNivelSubscripcionModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ReporteItemNivelSubscripcionModel.Id.ToString())) return BadRequest("Debe indicar ReporteItemNivelSubscripcionModel.Id");
                ReporteItemNivelSubscripcionModel retorno = await _ReporteItemNivelSubscripcionService.GetReporteItemNivelSubscripcionById(ReporteItemNivelSubscripcionModel);
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
                _ReporteItemNivelSubscripcionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ReporteItemNivelSubscripcionInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteItemNivelSubscripcionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteItemNivelSubscripcionModel>> ReporteItemNivelSubscripcionInsertOrUpdate([FromBody] ReporteItemNivelSubscripcionModel ReporteItemNivelSubscripcionModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ReporteItemNivelSubscripcionModel.ReporteId.ToString())) return BadRequest("Debe indicar ReporteId");

                ReporteItemNivelSubscripcionModel retorno = await _ReporteItemNivelSubscripcionService.InsertOrUpdate(ReporteItemNivelSubscripcionModel);
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
        [HttpGet("GetReporteItemNivelSubscripcions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteItemNivelSubscripcionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteItemNivelSubscripcionModel>>> GetReporteItemNivelSubscripcions()
        {
            try
            {
                List<ReporteItemNivelSubscripcionModel> retorno = await _ReporteItemNivelSubscripcionService.GetReporteItemNivelSubscripcions();
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
                _ReporteItemNivelSubscripcionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteItemNivelSubscripcionsByReporteId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteItemNivelSubscripcionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteItemNivelSubscripcionModel>>> GetReporteItemNivelSubscripcionsByReporteId(ReporteModel reporteModel)
        {
            try
            {
                List<ReporteItemNivelSubscripcionModel> retorno = await _ReporteItemNivelSubscripcionService.GetReporteItemNivelSubscripcionsByReporteId(reporteModel);
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
                _ReporteItemNivelSubscripcionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
