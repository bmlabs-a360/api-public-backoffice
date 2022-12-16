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
    public class TipoNivelVentaController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoNivelVentaService _TipoNivelVentaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoNivelVentaController(
            TipoNivelVentaService TipoNivelVentaService,
            ILogger<TipoNivelVentaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoNivelVentaService = TipoNivelVentaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoNivelVentaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoNivelVentaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoNivelVentaModel>> GetTipoNivelVentaById([FromBody] TipoNivelVentaModel TipoNivelVentaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoNivelVentaModel.Id.ToString())) return BadRequest("Debe indicar TipoNivelVentaModel.Id");
                TipoNivelVentaModel retorno = await _TipoNivelVentaService.GetTipoNivelVentaById(TipoNivelVentaModel);
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
                _TipoNivelVentaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoNivelVentaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoNivelVentaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoNivelVentaModel>> TipoNivelVentaInsertOrUpdate([FromBody] TipoNivelVentaModel TipoNivelVentaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoNivelVentaModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoNivelVentaModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoNivelVentaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoNivelVentaModel retorno = await _TipoNivelVentaService.InsertOrUpdate(TipoNivelVentaModel);
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
        [HttpPost("GetTipoNivelVentas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoNivelVentaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoNivelVentaModel>>> GetTipoNivelVentas()
        {
            try
            {
                List<TipoNivelVentaModel> retorno = await _TipoNivelVentaService.GetTipoNivelVentas();
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
                _TipoNivelVentaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
