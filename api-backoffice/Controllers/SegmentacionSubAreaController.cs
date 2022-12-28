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
    public class SegmentacionSubAreaController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISegmentacionSubAreaService _SegmentacionSubAreaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public SegmentacionSubAreaController(
            SegmentacionSubAreaService SegmentacionSubAreaService,
            ILogger<SegmentacionSubAreaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _SegmentacionSubAreaService = SegmentacionSubAreaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetSegmentacionSubAreaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SegmentacionSubAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SegmentacionSubAreaModel>> GetSegmentacionSubAreaById([FromBody] SegmentacionSubAreaModel SegmentacionSubAreaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(SegmentacionSubAreaModel.Id.ToString())) return BadRequest("Debe indicar SegmentacionSubAreaModel.Id");
                SegmentacionSubAreaModel retorno = await _SegmentacionSubAreaService.GetSegmentacionSubAreaById(SegmentacionSubAreaModel);
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
                _SegmentacionSubAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("SegmentacionSubAreaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SegmentacionSubAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SegmentacionSubAreaModel>> SegmentacionSubAreaInsertOrUpdate([FromBody] SegmentacionSubAreaModel SegmentacionSubAreaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(SegmentacionSubAreaModel.NombreSubArea.ToString())) return BadRequest("Debe indicar NombreSubArea");
                if (string.IsNullOrEmpty(SegmentacionSubAreaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar NombreSubArea");

                SegmentacionSubAreaModel retorno = await _SegmentacionSubAreaService.InsertOrUpdate(SegmentacionSubAreaModel);
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
        [HttpPost("GetSegmentacionSubAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SegmentacionSubAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SegmentacionSubAreaModel>>> GetSegmentacionSubAreas()
        {
            try
            {
                List<SegmentacionSubAreaModel> retorno = await _SegmentacionSubAreaService.GetSegmentacionSubAreas();
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
                _SegmentacionSubAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetSegmentacionSubAreasBySegmentacionAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SegmentacionSubAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SegmentacionSubAreaModel>>> GetSegmentacionSubAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            try
            {
                List<SegmentacionSubAreaModel> retorno = await _SegmentacionSubAreaService.GetSegmentacionSubAreasBySegmentacionAreaId(segmentacionAreaModel);
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
                _SegmentacionSubAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
