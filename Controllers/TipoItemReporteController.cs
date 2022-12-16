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
    public class TipoItemReporteController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoItemReporteService _TipoItemReporteService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoItemReporteController(
            TipoItemReporteService TipoItemReporteService,
            ILogger<TipoItemReporteController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoItemReporteService = TipoItemReporteService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoItemReporteById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoItemReporteModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoItemReporteModel>> GetTipoItemReporteById([FromBody] TipoItemReporteModel TipoItemReporteModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoItemReporteModel.Id.ToString())) return BadRequest("Debe indicar TipoItemReporteModel.Id");
                TipoItemReporteModel retorno = await _TipoItemReporteService.GetTipoItemReporteById(TipoItemReporteModel);
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
                _TipoItemReporteService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoItemReporteInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoItemReporteModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoItemReporteModel>> TipoItemReporteInsertOrUpdate([FromBody] TipoItemReporteModel TipoItemReporteModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoItemReporteModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoItemReporteModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoItemReporteModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoItemReporteModel retorno = await _TipoItemReporteService.InsertOrUpdate(TipoItemReporteModel);
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
        [HttpPost("GetTipoItemReportes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoItemReporteModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoItemReporteModel>>> GetTipoItemReportes()
        {
            try
            {
                List<TipoItemReporteModel> retorno = await _TipoItemReporteService.GetTipoItemReportes();
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
                _TipoItemReporteService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
