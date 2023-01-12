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
using neva.entities;

namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class EvaluacionEmpresaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEvaluacionEmpresaService _EvaluacionEmpresaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public EvaluacionEmpresaController(
            EvaluacionEmpresaService EvaluacionEmpresaService,
            ILogger<EvaluacionEmpresaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _EvaluacionEmpresaService = EvaluacionEmpresaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionEmpresaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> GetEvaluacionEmpresaById([FromBody] EvaluacionEmpresaModel EvaluacionEmpresaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.Id.ToString())) return BadRequest("Debe indicar EvaluacionEmpresaModel.Id");
                EvaluacionEmpresaModel retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresaById(EvaluacionEmpresaModel);
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
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("EvaluacionEmpresaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> EvaluacionEmpresaInsertOrUpdate([FromBody] EvaluacionEmpresaModel EvaluacionEmpresaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.EmpresaId.ToString())) return BadRequest("Debe indicar EmpresaId");
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.EvaluacionId.ToString())) return BadRequest("Debe indicar EvaluacionId");
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.FechaInicioTiempoLimite.ToString())) return BadRequest("Debe indicar FechaInicioTiempoLimite");
                if (string.IsNullOrEmpty(EvaluacionEmpresaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                EvaluacionEmpresaModel retorno = await _EvaluacionEmpresaService.InsertOrUpdate(EvaluacionEmpresaModel);
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
        [HttpPost("GetEvaluacionEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionEmpresaModel>>> GetEvaluacionEmpresas()
        {
            try
            {
                List<EvaluacionEmpresaModel> retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresas();
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
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        [HttpPost("GetSeguimiento")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoEvaluacionEmpresaDto>>> GetSeguimiento()
        {
            try
            {
                List<SeguimientoEvaluacionEmpresaDto> retorno =  _EvaluacionEmpresaService.GetSeguimiento();
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
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


        [HttpPost("GetPlanMejoras")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SeguimientoPlanMejoraModelDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<SeguimientoPlanMejoraModelDto>>> GetPlanMejoras(EvaluacionEmpresa evaluacionEmpresa)
        {
            try
            {
                List<SeguimientoPlanMejoraModelDto> retorno = _EvaluacionEmpresaService.GetPlanMejoras(evaluacionEmpresa);
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
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }



        //[ApiKeyAuth]
        [HttpPost("GetEvaluacionEmpresasByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluacionEmpresaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<EvaluacionEmpresaModel>> GetEvaluacionEmpresasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                EvaluacionEmpresaModel retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresasByEvaluacionId(evaluacionModel);
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
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetEvaluacionEmpresasByEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EvaluacionEmpresaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<EvaluacionEmpresaModel>>> GetEvaluacionEmpresasByEmpresaId(EmpresaModel EmpresaMode)
        {
            try
            {
                List<EvaluacionEmpresaModel> retorno = await _EvaluacionEmpresaService.GetEvaluacionEmpresasByEmpresaId(EmpresaMode);
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
                _EvaluacionEmpresaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("DeleteList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> DeleteList(List<EvaluacionEmpresaModel> c)
        {
            try
            {


                await _EvaluacionEmpresaService.DeleteList(c);

                return Ok(true);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
            }
        }

        [HttpPost("InsertOrUpdateList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> InsertOrUpdateList(List<EvaluacionEmpresaModel> c)
        {
            try
            {


                await _EvaluacionEmpresaService.InsertOrUpdateList(c);

                return Ok(true);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _EvaluacionEmpresaService.Dispose();
            }
        }
    }



}
