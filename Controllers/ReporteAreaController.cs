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
    public class ReporteAreaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReporteAreaService _ReporteAreaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ReporteAreaController(
            ReporteAreaService ReporteAreaService,
            ILogger<ReporteAreaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ReporteAreaService = ReporteAreaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetReporteAreaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteAreaModel>> GetReporteAreaById([FromBody] ReporteAreaModel ReporteAreaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ReporteAreaModel.Id.ToString())) return BadRequest("Debe indicar ReporteAreaModel.Id");
                ReporteAreaModel retorno = await _ReporteAreaService.GetReporteAreaById(ReporteAreaModel);
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
                _ReporteAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ReporteAreaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteAreaModel>> ReporteAreaInsertOrUpdate([FromBody] ReporteAreaModel ReporteAreaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ReporteAreaModel.ReporteId.ToString())) return BadRequest("Debe indicar ReporteId");
                if (string.IsNullOrEmpty(ReporteAreaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar SegmentacionAreaId");

                ReporteAreaModel retorno = await _ReporteAreaService.InsertOrUpdate(ReporteAreaModel);
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
        [HttpPost("GetReporteAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteAreaModel>>> GetReporteAreas()
        {
            try
            {
                List<ReporteAreaModel> retorno = await _ReporteAreaService.GetReporteAreas();
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
                _ReporteAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteAreasByReporteId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteAreaModel>>> GetReporteAreasByReporteId(ReporteModel reporteModel)
        {
            try
            {
                List<ReporteAreaModel> retorno = await _ReporteAreaService.GetReporteAreasByReporteId(reporteModel);
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
                _ReporteAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteAreasBySegmentacionAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteAreaModel>>> GetReporteAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            try
            {
                List<ReporteAreaModel> retorno = await _ReporteAreaService.GetReporteAreasBySegmentacionAreaId(segmentacionAreaModel);
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
                _ReporteAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


    }
}
