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
    public class ReporteRecomendacionAreaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReporteRecomendacionAreaService _ReporteRecomendacionAreaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ReporteRecomendacionAreaController(
            ReporteRecomendacionAreaService ReporteRecomendacionAreaService,
            ILogger<ReporteRecomendacionAreaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ReporteRecomendacionAreaService = ReporteRecomendacionAreaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetReporteRecomendacionAreaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteRecomendacionAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreaById([FromBody] ReporteRecomendacionAreaModel ReporteRecomendacionAreaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.Id.ToString())) return BadRequest("Debe indicar ReporteRecomendacionAreaModel.Id");
                ReporteRecomendacionAreaModel retorno = await _ReporteRecomendacionAreaService.GetReporteRecomendacionAreaById(ReporteRecomendacionAreaModel);
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
                _ReporteRecomendacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ReporteRecomendacionAreaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteRecomendacionAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteRecomendacionAreaModel>> ReporteRecomendacionAreaInsertOrUpdate([FromBody] ReporteRecomendacionAreaModel ReporteRecomendacionAreaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.ReporteId.ToString())) return BadRequest("Debe indicar ReporteId");
                if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar SegmentacionAreaId");

                ReporteRecomendacionAreaModel retorno = await _ReporteRecomendacionAreaService.InsertOrUpdate(ReporteRecomendacionAreaModel);
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
        [HttpPost("GetReporteRecomendacionAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteRecomendacionAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteRecomendacionAreaModel>>> GetReporteRecomendacionAreas()
        {
            try
            {
                List<ReporteRecomendacionAreaModel> retorno = await _ReporteRecomendacionAreaService.GetReporteRecomendacionAreas();
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
                _ReporteRecomendacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteRecomendacionAreasByReporteId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteRecomendacionAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteRecomendacionAreaModel>>> GetReporteRecomendacionAreasByReporteId(ReporteModel reporteModel)
        {
            try
            {
                List<ReporteRecomendacionAreaModel> retorno = await _ReporteRecomendacionAreaService.GetReporteRecomendacionAreasByReporteId(reporteModel);
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
                _ReporteRecomendacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetReporteFeedbackAreasByReporteId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteRecomendacionAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteRecomendacionAreaModel>>> GetReporteFeedbackAreasByReporteId(UsuarioModel usuario, Guid reporteId)
        {
            try
            {
                List<ReporteRecomendacionAreaModel> retorno = await _ReporteRecomendacionAreaService.GetReporteFeedbackAreasByReporteId(usuario, reporteId);
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
                _ReporteRecomendacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteRecomendacionAreasBySegmentacionAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteRecomendacionAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteRecomendacionAreaModel>>> GetReporteRecomendacionAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            try
            {
                List<ReporteRecomendacionAreaModel> retorno = await _ReporteRecomendacionAreaService.GetReporteRecomendacionAreasBySegmentacionAreaId(segmentacionAreaModel);
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
                _ReporteRecomendacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


    }
}
