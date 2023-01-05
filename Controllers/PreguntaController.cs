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
    public class PreguntaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPreguntaService _PreguntaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public PreguntaController(
            PreguntaService PreguntaService,
            ILogger<PreguntaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _PreguntaService = PreguntaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetPreguntaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PreguntaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PreguntaModel>> GetPreguntaById([FromBody] PreguntaModel PreguntaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(PreguntaModel.Id.ToString())) return BadRequest("Debe indicar PreguntaModel.Id");
                PreguntaModel retorno = await _PreguntaService.GetPreguntaById(PreguntaModel);
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
                _PreguntaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("PreguntaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PreguntaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PreguntaModel>> PreguntaInsertOrUpdate([FromBody] PreguntaModel PreguntaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(PreguntaModel.Capacidad.ToString())) return BadRequest("Debe indicar Capacidad");
                if (string.IsNullOrEmpty(PreguntaModel.Detalle.ToString())) return BadRequest("Debe indicar EvaluacionId");
                if (string.IsNullOrEmpty(PreguntaModel.EvaluacionId.ToString())) return BadRequest("Debe indicar EvaluacionId");
                if (string.IsNullOrEmpty(PreguntaModel.Orden.ToString())) return BadRequest("Debe indicar Orden");
                if (string.IsNullOrEmpty(PreguntaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar SegmentacionAreaId");
                if (string.IsNullOrEmpty(PreguntaModel.SegmentacionSubAreaId.ToString())) return BadRequest("Debe indicar SegmentacionSubAreaId");
                if (string.IsNullOrEmpty(PreguntaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar SegmentacionAreaId");
                if (string.IsNullOrEmpty(PreguntaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                PreguntaModel retorno = await _PreguntaService.InsertOrUpdate(PreguntaModel);
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
        [HttpPost("GetPreguntas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PreguntaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PreguntaModel>>> GetPreguntas()
        {
            try
            {
                List<PreguntaModel> retorno = await _PreguntaService.GetPreguntas();
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
                _PreguntaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetPreguntasByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PreguntaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PreguntaModel>>> GetPreguntasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            try
            {
                List<PreguntaModel> retorno = await _PreguntaService.GetPreguntasByEvaluacionId(evaluacionModel);
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
                _PreguntaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetPreguntasBySegmentacionAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PreguntaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PreguntaModel>>> GetPreguntasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            try
            {
                List<PreguntaModel> retorno = await _PreguntaService.GetPreguntasBySegmentacionAreaId(segmentacionAreaModel);
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
                _PreguntaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetPreguntasBySegmentacionSubAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PreguntaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PreguntaModel>>> GetPreguntasBySegmentacionSubAreaId(SegmentacionSubAreaModel segmentacionSubAreaModel)
        {
            try
            {
                List<PreguntaModel> retorno = await _PreguntaService.GetPreguntasBySegmentacionSubAreaId(segmentacionSubAreaModel);
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
                _PreguntaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("DeletePregunta")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> DeletePregunta([FromBody] PreguntaModel preguntaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(preguntaModel.Id.ToString())) return BadRequest("Debe indicar pregunta.Id");
                return await _PreguntaService.DeletePregunta(preguntaModel);

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
                _PreguntaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }
}
