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
    public class TipoImportanciaController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoImportanciaService _TipoImportanciaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoImportanciaController(
            TipoImportanciaService TipoImportanciaService,
            ILogger<TipoImportanciaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoImportanciaService = TipoImportanciaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoImportanciaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoImportanciaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoImportanciaModel>> GetTipoImportanciaById([FromBody] TipoImportanciaModel TipoImportanciaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoImportanciaModel.Id.ToString())) return BadRequest("Debe indicar TipoImportanciaModel.Id");
                TipoImportanciaModel retorno = await _TipoImportanciaService.GetTipoImportanciaById(TipoImportanciaModel);
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
                _TipoImportanciaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoImportanciaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoImportanciaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoImportanciaModel>> TipoImportanciaInsertOrUpdate([FromBody] TipoImportanciaModel TipoImportanciaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoImportanciaModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoImportanciaModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoImportanciaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoImportanciaModel retorno = await _TipoImportanciaService.InsertOrUpdate(TipoImportanciaModel);
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
        [HttpPost("GetTipoImportancias")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoImportanciaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoImportanciaModel>>> GetTipoImportancias()
        {
            try
            {
                List<TipoImportanciaModel> retorno = await _TipoImportanciaService.GetTipoImportancias();
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
                _TipoImportanciaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
