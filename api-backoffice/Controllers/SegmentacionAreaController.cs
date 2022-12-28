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
    public class SegmentacionAreaController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISegmentacionAreaService _SegmentacionAreaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public SegmentacionAreaController(
            SegmentacionAreaService SegmentacionAreaService,
            ILogger<SegmentacionAreaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _SegmentacionAreaService = SegmentacionAreaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetSegmentacionAreaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SegmentacionAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SegmentacionAreaModel>> GetSegmentacionAreaById([FromBody] SegmentacionAreaModel SegmentacionAreaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(SegmentacionAreaModel.Id.ToString())) return BadRequest("Debe indicar SegmentacionAreaModel.Id");
                SegmentacionAreaModel retorno = await _SegmentacionAreaService.GetSegmentacionAreaById(SegmentacionAreaModel);
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
                _SegmentacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("SegmentacionAreaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SegmentacionAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SegmentacionAreaModel>> SegmentacionAreaInsertOrUpdate([FromBody] SegmentacionAreaModel SegmentacionAreaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(SegmentacionAreaModel.NombreArea.ToString())) return BadRequest("Debe indicar NombreArea");

                SegmentacionAreaModel retorno = await _SegmentacionAreaService.InsertOrUpdate(SegmentacionAreaModel);
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
        [HttpPost("GetSegmentacionAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SegmentacionAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SegmentacionAreaModel>>> GetSegmentacionAreas()
        {
            try
            {
                List<SegmentacionAreaModel> retorno = await _SegmentacionAreaService.GetSegmentacionAreas();
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
                _SegmentacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetSegmentacionAreasByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SegmentacionAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SegmentacionAreaModel>>> GetSegmentacionAreasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                List<SegmentacionAreaModel> retorno = await _SegmentacionAreaService.GetSegmentacionAreasByEvaluacionId(evaluacionModel);
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
                _SegmentacionAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
