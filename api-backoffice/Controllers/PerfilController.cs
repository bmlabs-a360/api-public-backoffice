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
    public class PerfilController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPerfilService _PerfilService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public PerfilController(
            PerfilService PerfilService,
            ILogger<PerfilController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _PerfilService = PerfilService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetPerfilById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PerfilModel>> GetPerfilById([FromBody] PerfilModel PerfilModel)

        {
            try
            {
                if (string.IsNullOrEmpty(PerfilModel.Id.ToString())) return BadRequest("Debe indicar PerfilModel.Id");
                PerfilModel retorno = await _PerfilService.GetPerfilById(PerfilModel);
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
                _PerfilService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("PerfilInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PerfilModel>> PerfilInsertOrUpdate([FromBody] PerfilModel PerfilModel)
        {
            try
            {


                if (string.IsNullOrEmpty(PerfilModel.Nombre)) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(PerfilModel.Detalle)) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(PerfilModel.Activo.ToString())) return BadRequest("Debe indicar Activo");


                PerfilModel retorno = await _PerfilService.InsertOrUpdate(PerfilModel);
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
        [HttpGet("GetPerfils")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PerfilModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PerfilModel>>> GetPerfils()
        {
            try
            {
                List<PerfilModel> retorno = await _PerfilService.GetPerfils();
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
                _PerfilService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpGet("GetPerfilsConsultor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PerfilModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PerfilModel>>> GetPerfilsConsultor()
        {
            try
            {
                List<PerfilModel> retorno = await _PerfilService.GetPerfilsConsultor();
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
                _PerfilService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetPerfilByPerfilIdFilter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PerfilModel>>> GetPerfilByPerfilIdFilter([FromBody] PerfilModel PerfilModel)

        {
            try
            {
                List<PerfilModel> retorno = null;
                if (string.IsNullOrEmpty(PerfilModel.Id.ToString())) return BadRequest("Debe indicar PerfilModel.Id");
                PerfilModel perfil = await _PerfilService.GetPerfilById(PerfilModel);

                if (perfil.Nombre == "Consultor")
                {
                    retorno = await _PerfilService.GetPerfilsConsultor();
                }
                else 
                {
                    retorno = await _PerfilService.GetPerfils();
                }
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
                _PerfilService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
    }



}
