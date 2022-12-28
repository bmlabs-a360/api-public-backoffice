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
    public class TipoDiferenciaRelacionadaController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoDiferenciaRelacionadaService _TipoDiferenciaRelacionadaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoDiferenciaRelacionadaController(
            TipoDiferenciaRelacionadaService TipoDiferenciaRelacionadaService,
            ILogger<TipoDiferenciaRelacionadaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoDiferenciaRelacionadaService = TipoDiferenciaRelacionadaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoDiferenciaRelacionadaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoDiferenciaRelacionadaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoDiferenciaRelacionadaModel>> GetTipoDiferenciaRelacionadaById([FromBody] TipoDiferenciaRelacionadaModel TipoDiferenciaRelacionadaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Id.ToString())) return BadRequest("Debe indicar TipoDiferenciaRelacionadaModel.Id");
                TipoDiferenciaRelacionadaModel retorno = await _TipoDiferenciaRelacionadaService.GetTipoDiferenciaRelacionadaById(TipoDiferenciaRelacionadaModel);
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
                _TipoDiferenciaRelacionadaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoDiferenciaRelacionadaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoDiferenciaRelacionadaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoDiferenciaRelacionadaModel>> TipoDiferenciaRelacionadaInsertOrUpdate([FromBody] TipoDiferenciaRelacionadaModel TipoDiferenciaRelacionadaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoDiferenciaRelacionadaModel retorno = await _TipoDiferenciaRelacionadaService.InsertOrUpdate(TipoDiferenciaRelacionadaModel);
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
        [HttpPost("GetTipoDiferenciaRelacionadas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoDiferenciaRelacionadaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoDiferenciaRelacionadaModel>>> GetTipoDiferenciaRelacionadas()
        {
            try
            {
                List<TipoDiferenciaRelacionadaModel> retorno = await _TipoDiferenciaRelacionadaService.GetTipoDiferenciaRelacionadas();
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
                _TipoDiferenciaRelacionadaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
