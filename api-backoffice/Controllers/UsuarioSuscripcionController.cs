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
    public class UsuarioSuscripcionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsuarioSuscripcionService _UsuarioSuscripcionService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public UsuarioSuscripcionController(
            UsuarioSuscripcionService UsuarioSuscripcionService,
            ILogger<UsuarioSuscripcionController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _UsuarioSuscripcionService = UsuarioSuscripcionService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetUsuarioSuscripcionById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioSuscripcionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioSuscripcionModel>> GetUsuarioSuscripcionById([FromBody] UsuarioSuscripcionModel UsuarioSuscripcionModel)

        {
            try
            {
                if (string.IsNullOrEmpty(UsuarioSuscripcionModel.Id.ToString())) return BadRequest("Debe indicar UsuarioSuscripcionModel.Id");
                UsuarioSuscripcionModel retorno = await _UsuarioSuscripcionService.GetUsuarioSuscripcionById(UsuarioSuscripcionModel);
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
                _UsuarioSuscripcionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("UsuarioSuscripcionInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioSuscripcionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioSuscripcionModel>> UsuarioSuscripcionInsertOrUpdate([FromBody] UsuarioSuscripcionModel UsuarioSuscripcionModel)
        {
            try
            {
                if (string.IsNullOrEmpty(UsuarioSuscripcionModel.FechaExpiracion.ToString())) return BadRequest("Debe indicar FechaExpiracion");
                if (string.IsNullOrEmpty(UsuarioSuscripcionModel.TiempoSuscripcion.ToString())) return BadRequest("Debe indicar TiempoSuscripcion");
                if (string.IsNullOrEmpty(UsuarioSuscripcionModel.UsuarioId.ToString())) return BadRequest("Debe indicar UsuarioId");
                if (string.IsNullOrEmpty(UsuarioSuscripcionModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                UsuarioSuscripcionModel retorno = await _UsuarioSuscripcionService.InsertOrUpdate(UsuarioSuscripcionModel);
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
        [HttpPost("GetUsuarioSuscripcions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioSuscripcionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioSuscripcionModel>>> GetUsuarioSuscripcions()
        {
            try
            {
                List<UsuarioSuscripcionModel> retorno = await _UsuarioSuscripcionService.GetUsuarioSuscripcions();
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
                _UsuarioSuscripcionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetUsuarioSuscripcionsByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioSuscripcionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioSuscripcionModel>> GetUsuarioSuscripcionsByUsuarioId([FromBody] UsuarioModel UsuarioModel)

        {
            try
            {
                if (string.IsNullOrEmpty(UsuarioModel.Id.ToString())) return BadRequest("Debe indicar UsuarioModel.Id");
                UsuarioSuscripcionModel retorno = await _UsuarioSuscripcionService.GetUsuarioSuscripcionsByUsuarioId(UsuarioModel);
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
                _UsuarioSuscripcionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
