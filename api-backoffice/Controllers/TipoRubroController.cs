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
    public class TipoRubroController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoRubroService _TipoRubroService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoRubroController(
            TipoRubroService TipoRubroService,
            ILogger<TipoRubroController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoRubroService = TipoRubroService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoRubroById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoRubroModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoRubroModel>> GetTipoRubroById([FromBody] TipoRubroModel TipoRubroModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoRubroModel.Id.ToString())) return BadRequest("Debe indicar TipoRubroModel.Id");
                TipoRubroModel retorno = await _TipoRubroService.GetTipoRubroById(TipoRubroModel);
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
                _TipoRubroService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoRubroInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoRubroModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoRubroModel>> TipoRubroInsertOrUpdate([FromBody] TipoRubroModel TipoRubroModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoRubroModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoRubroModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoRubroModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoRubroModel retorno = await _TipoRubroService.InsertOrUpdate(TipoRubroModel);
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
        [HttpPost("GetTipoRubros")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoRubroModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoRubroModel>>> GetTipoRubros()
        {
            try
            {
                List<TipoRubroModel> retorno = await _TipoRubroService.GetTipoRubros();
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
                _TipoRubroService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
