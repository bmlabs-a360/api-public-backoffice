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
    public class PerfilPermisoController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPerfilPermisoService _PerfilPermisoService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public PerfilPermisoController(
            PerfilPermisoService PerfilPermisoService,
            ILogger<PerfilPermisoController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _PerfilPermisoService = PerfilPermisoService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetPerfilPermisoById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilPermisoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PerfilPermisoModel>> GetPerfilPermisoById([FromBody] PerfilPermisoModel PerfilPermisoModel)

        {
            try
            {
                if (string.IsNullOrEmpty(PerfilPermisoModel.Id.ToString())) return BadRequest("Debe indicar PerfilPermisoModel.Id");
                PerfilPermisoModel retorno = await _PerfilPermisoService.GetPerfilPermisoById(PerfilPermisoModel);
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
                _PerfilPermisoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("PerfilPermisoInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilPermisoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PerfilPermisoModel>> PerfilPermisoInsertOrUpdate([FromBody] PerfilPermisoModel PerfilPermisoModel)
        {
            try
            {


                if (string.IsNullOrEmpty(PerfilPermisoModel.PerfilId.ToString())) return BadRequest("Debe indicar PerfilId");
                if (string.IsNullOrEmpty(PerfilPermisoModel.Detalle)) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(PerfilPermisoModel.Nombre)) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(PerfilPermisoModel.Activo.ToString())) return BadRequest("Debe indicar Activo");


                PerfilPermisoModel retorno = await _PerfilPermisoService.InsertOrUpdate(PerfilPermisoModel);
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
        [HttpPost("GetPerfilPermisos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PerfilPermisoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PerfilPermisoModel>>> GetPerfilPermisoByEvaluacionId()
        {
            try
            {
                List<PerfilPermisoModel> retorno = await _PerfilPermisoService.GetPerfilPermisos();
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
                _PerfilPermisoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        //[ApiKeyAuth]
        [HttpPost("GetGetPerfilPermisosByPerfilId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PerfilPermisoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PerfilPermisoModel>>> GetGetPerfilPermisosByPerfilId(PerfilModel perfilModel)
        {
            try
            {
                List<PerfilPermisoModel> retorno = await _PerfilPermisoService.GetGetPerfilPermisosByPerfilId(perfilModel);
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
                _PerfilPermisoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }



}
