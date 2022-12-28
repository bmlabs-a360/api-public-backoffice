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
    public class TipoTamanoEmpresaController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoTamanoEmpresaService _TipoTamanoEmpresaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoTamanoEmpresaController(
            TipoTamanoEmpresaService TipoTamanoEmpresaService,
            ILogger<TipoTamanoEmpresaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoTamanoEmpresaService = TipoTamanoEmpresaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoTamanoEmpresaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoTamanoEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoTamanoEmpresaModel>> GetTipoTamanoEmpresaById([FromBody] TipoTamanoEmpresaModel TipoTamanoEmpresaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Id.ToString())) return BadRequest("Debe indicar TipoTamanoEmpresaModel.Id");
                TipoTamanoEmpresaModel retorno = await _TipoTamanoEmpresaService.GetTipoTamanoEmpresaById(TipoTamanoEmpresaModel);
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
                _TipoTamanoEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoTamanoEmpresaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoTamanoEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoTamanoEmpresaModel>> TipoTamanoEmpresaInsertOrUpdate([FromBody] TipoTamanoEmpresaModel TipoTamanoEmpresaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoTamanoEmpresaModel retorno = await _TipoTamanoEmpresaService.InsertOrUpdate(TipoTamanoEmpresaModel);
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
        [HttpPost("GetTipoTamanoEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoTamanoEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoTamanoEmpresaModel>>> GetTipoTamanoEmpresas()
        {
            try
            {
                List<TipoTamanoEmpresaModel> retorno = await _TipoTamanoEmpresaService.GetTipoTamanoEmpresas();
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
                _TipoTamanoEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
