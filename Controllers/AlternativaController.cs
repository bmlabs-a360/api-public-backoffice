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
    public class AlternativaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAlternativaService _alternativaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public AlternativaController(
            AlternativaService alternativaService,
            ILogger<AlternativaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _alternativaService = alternativaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetAlternativaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlternativaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<AlternativaModel>> GetAlternativaById([FromBody] AlternativaModel alternativaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(alternativaModel.Id.ToString())) return BadRequest("Debe indicar alternativaModel.Id");
                AlternativaModel retorno = await _alternativaService.GetAlternativaById(alternativaModel);
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
                _alternativaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("AlternativaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlternativaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<AlternativaModel>> AlternativaInsertOrUpdate([FromBody] AlternativaModel alternativaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(alternativaModel.EvaluacionId.ToString())) return BadRequest("Debe indicar EvaluacionId");
                if (string.IsNullOrEmpty(alternativaModel.PreguntaId.ToString())) return BadRequest("Debe indicar PreguntaId");
                if (string.IsNullOrEmpty(alternativaModel.Detalle)) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(alternativaModel.Orden.ToString())) return BadRequest("Debe indicar Orden");
                if (string.IsNullOrEmpty(alternativaModel.Valor.ToString())) return BadRequest("Debe indicar Valor");
                if (string.IsNullOrEmpty(alternativaModel.Retroalimentacion)) return BadRequest("Debe indicar Retroalimentacion");
                //if (string.IsNullOrEmpty(alternativaModel.FechaCreacion.ToString())) return BadRequest("Debe indicar FechaCreacion");//se maneja en base de datos
                if (string.IsNullOrEmpty(alternativaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                AlternativaModel retorno = await _alternativaService.InsertOrUpdate(alternativaModel);
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
        [HttpPost("GetAlternativaByPreguntaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlternativaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<AlternativaModel>> GetAlternativaByPreguntaId([FromBody] AlternativaModel alternativaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(alternativaModel.PreguntaId.ToString())) return BadRequest("Debe indicar PreguntaId");
                AlternativaModel retorno = await _alternativaService.GetAlternativaByPreguntaId(alternativaModel);
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
                _alternativaService.Dispose();
               // _controlTokenService.Dispose();
            }
        }
        //[ApiKeyAuth]
        [HttpPost("GetAlternativaByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AlternativaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<AlternativaModel>>> GetAlternativaByEvaluacionId([FromBody] AlternativaModel alternativaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(alternativaModel.EvaluacionId.ToString())) return BadRequest("Debe indicar EvaluacionId");
                List<AlternativaModel> retorno = await _alternativaService.GetAlternativaByEvaluacionId(alternativaModel);
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
                _alternativaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }



}
