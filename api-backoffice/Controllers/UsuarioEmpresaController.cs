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
    public class UsuarioEmpresaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public UsuarioEmpresaController(
            UsuarioEmpresaService UsuarioEmpresaService,
            ILogger<UsuarioEmpresaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEmpresaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioEmpresaModel>> GetUsuarioEmpresaById([FromBody] UsuarioEmpresaModel usuarioEmpresaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(usuarioEmpresaModel.Id.ToString())) return BadRequest("Debe indicar usuarioEmpresaModel.Id");
                UsuarioEmpresaModel retorno = await _UsuarioEmpresaService.GetUsuarioEmpresaById(usuarioEmpresaModel);
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
                _UsuarioEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("UsuarioEmpresaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioEmpresaModel>> UsuarioEmpresaInsertOrUpdate([FromBody] UsuarioEmpresaModel usuarioEmpresaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioEmpresaModel.UsuarioId.ToString())) return BadRequest("Debe indicar UsuarioId");
                if (string.IsNullOrEmpty(usuarioEmpresaModel.EmpresaId.ToString())) return BadRequest("Debe indicar EmpresaId");
                if (string.IsNullOrEmpty(usuarioEmpresaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                UsuarioEmpresaModel retorno = await _UsuarioEmpresaService.InsertOrUpdate(usuarioEmpresaModel);
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
        [HttpPost("GetUsuarioEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioEmpresaModel>>> GetUsuarioEmpresas()
        {
            try
            {
                List<UsuarioEmpresaModel> retorno = await _UsuarioEmpresaService.GetUsuarioEmpresas();
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
                _UsuarioEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        //[ApiKeyAuth]
        [HttpPost("GetUsuarioEmpresasByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioEmpresaModel>>> GetUsuarioEmpresasByUsuarioId(UsuarioModel usuarioModel)
        {
            try
            {
                List<UsuarioEmpresaModel> retorno = await _UsuarioEmpresaService.GetUsuarioEmpresasByUsuarioId( usuarioModel);
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
                _UsuarioEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }



}
