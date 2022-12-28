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
    public class UsuarioAreaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsuarioAreaService _UsuarioAreaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public UsuarioAreaController(
            UsuarioAreaService UsuarioAreaService,
            ILogger<UsuarioAreaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _UsuarioAreaService = UsuarioAreaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetUsuarioAreaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioAreaModel>> GetUsuarioAreaById([FromBody] UsuarioAreaModel UsuarioAreaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(UsuarioAreaModel.Id.ToString())) return BadRequest("Debe indicar UsuarioAreaModel.Id");
                UsuarioAreaModel retorno = await _UsuarioAreaService.GetUsuarioAreaById(UsuarioAreaModel);
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
                _UsuarioAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("UsuarioAreaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioAreaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioAreaModel>> UsuarioAreaInsertOrUpdate([FromBody] UsuarioAreaModel UsuarioAreaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(UsuarioAreaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar SegmentacionAreaId");
                if (string.IsNullOrEmpty(UsuarioAreaModel.UsuarioEvaluacionId.ToString())) return BadRequest("Debe indicar UsuarioEvaluacionId");
                if (string.IsNullOrEmpty(UsuarioAreaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                UsuarioAreaModel retorno = await _UsuarioAreaService.InsertOrUpdate(UsuarioAreaModel);
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
        [HttpPost("GetUsuarioAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioAreaModel>>> GetUsuarioAreas()
        {
            try
            {
                List<UsuarioAreaModel> retorno = await _UsuarioAreaService.GetUsuarioAreas();
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
                _UsuarioAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetUsuarioAreasByUsuarioEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioAreaModel>>> GetUsuarioAreasByUsuarioEvaluacionId(UsuarioEvaluacionModel usuarioEvaluacionModel)
        {
            try
            {
                List<UsuarioAreaModel> retorno = await _UsuarioAreaService.GetUsuarioAreasByUsuarioEvaluacionId(usuarioEvaluacionModel);
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
                _UsuarioAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetUsuarioAreasByUsuarioSegmentacionAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioAreaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioAreaModel>>> GetUsuarioAreasByUsuarioSegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            try
            {
                List<UsuarioAreaModel> retorno = await _UsuarioAreaService.GetUsuarioAreasByUsuarioSegmentacionAreaId(segmentacionAreaModel);
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
                _UsuarioAreaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


    }
}
