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
    public class EvaluacionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEvaluacionService _EvaluacionService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public EvaluacionController(
            EvaluacionService EvaluacionService,
            ILogger<EvaluacionController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _EvaluacionService = EvaluacionService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionModel>> GetEvaluacionById([FromBody] EvaluacionModel EvaluacionModel)

        {
            try
            {
                if (string.IsNullOrEmpty(EvaluacionModel.Id.ToString())) return BadRequest("Debe indicar EvaluacionModel.Id");
                EvaluacionModel retorno = await _EvaluacionService.GetEvaluacionById(EvaluacionModel);
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
                _EvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("EvaluacionInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionModel>> EvaluacionInsertOrUpdate(EvaluacionModel EvaluacionModel)
        {
            try
            {
                if (string.IsNullOrEmpty(EvaluacionModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(EvaluacionModel.TiempoLimite.ToString())) return BadRequest("Debe indicar TiempoLimite");
                if (string.IsNullOrEmpty(EvaluacionModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                EvaluacionModel retorno = await _EvaluacionService.InsertOrUpdate(EvaluacionModel);
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
        [HttpPost("GetEvaluacions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionModel>>> GetEvaluacions()
        {
            try
            {
                List<EvaluacionModel> retorno = await _EvaluacionService.GetEvaluacions();
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
                _EvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionsByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionModel>> GetEvaluacionsByUsuarioId(UsuarioModel usuarioModel)
        {
            try
            {
                EvaluacionModel retorno = await _EvaluacionService.GetEvaluacionsByUsuarioId(usuarioModel);
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
                _EvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetEvaluacionsByEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionModel>>> GetEvaluacionsByEmpresaId(EmpresaModel EmpresaMode)
        {
            try
            {
                List<EvaluacionModel> retorno = await _EvaluacionService.GetEvaluacionsByEmpresaId(EmpresaMode);
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
                _EvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        [HttpPost("InsertOrUpdateDefault")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> InsertOrUpdateDefault(EvaluacionModel evaluacionModel)
        {
            try
            {
                if (string.IsNullOrEmpty(evaluacionModel.Id.ToString())) return BadRequest("Debe indicar evaluacionModel.Id");
                return await _EvaluacionService.InsertOrUpdateDefault(evaluacionModel);

                //return NoContent();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


    }



}
