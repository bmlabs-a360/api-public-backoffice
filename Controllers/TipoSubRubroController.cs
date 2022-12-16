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
    public class TipoSubRubroController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoSubRubroService _TipoSubRubroService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoSubRubroController(
            TipoSubRubroService TipoSubRubroService,
            ILogger<TipoSubRubroController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoSubRubroService = TipoSubRubroService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoSubRubroById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoSubRubroModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoSubRubroModel>> GetTipoSubRubroById([FromBody] TipoSubRubroModel TipoSubRubroModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoSubRubroModel.Id.ToString())) return BadRequest("Debe indicar TipoSubRubroModel.Id");
                TipoSubRubroModel retorno = await _TipoSubRubroService.GetTipoSubRubroById(TipoSubRubroModel);
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
                _TipoSubRubroService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoSubRubroInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoSubRubroModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoSubRubroModel>> TipoSubRubroInsertOrUpdate([FromBody] TipoSubRubroModel TipoSubRubroModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoSubRubroModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoSubRubroModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoSubRubroModel.TipoRubroId.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoSubRubroModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoSubRubroModel retorno = await _TipoSubRubroService.InsertOrUpdate(TipoSubRubroModel);
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
        [HttpPost("GetTipoSubRubros")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoSubRubroModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoSubRubroModel>>> GetTipoSubRubros()
        {
            try
            {
                List<TipoSubRubroModel> retorno = await _TipoSubRubroService.GetTipoSubRubros();
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
                _TipoSubRubroService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
