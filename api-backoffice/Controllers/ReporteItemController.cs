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
    public class ReporteItemController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReporteItemService _ReporteItemService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ReporteItemController(
            ReporteItemService ReporteItemService,
            ILogger<ReporteItemController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ReporteItemService = ReporteItemService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetReporteItemById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteItemModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteItemModel>> GetReporteItemById([FromBody] ReporteItemModel ReporteItemModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ReporteItemModel.Id.ToString())) return BadRequest("Debe indicar ReporteItemModel.Id");
                ReporteItemModel retorno = await _ReporteItemService.GetReporteItemById(ReporteItemModel);
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
                _ReporteItemService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ReporteItemInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteItemModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ReporteItemModel>> ReporteItemInsertOrUpdate([FromBody] ReporteItemModel ReporteItemModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ReporteItemModel.ReporteId.ToString())) return BadRequest("Debe indicar ReporteId");
                if (string.IsNullOrEmpty(ReporteItemModel.TipoItemReporteId.ToString())) return BadRequest("Debe indicar TipoItemReporteId");

                ReporteItemModel retorno = await _ReporteItemService.InsertOrUpdate(ReporteItemModel);
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
        [HttpPost("GetReporteItems")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteItemModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteItemModel>>> GetReporteItems()
        {
            try
            {
                List<ReporteItemModel> retorno = await _ReporteItemService.GetReporteItems();
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
                _ReporteItemService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetReporteItemsByReporteId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReporteItemModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ReporteItemModel>>> GetReporteItemsByReporteId(ReporteModel reporteModel)
        {
            try
            {
                List<ReporteItemModel> retorno = await _ReporteItemService.GetReporteItemsByReporteId(reporteModel);
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
                _ReporteItemService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
