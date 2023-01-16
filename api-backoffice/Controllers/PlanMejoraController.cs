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
    public class PlanMejoraController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPlanMejoraService _PlanMejoraService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public PlanMejoraController(
            PlanMejoraService PlanMejoraService,
            ILogger<PlanMejoraController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _PlanMejoraService = PlanMejoraService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetPlanMejoraById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanMejoraModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PlanMejoraModel>> GetPlanMejoraById([FromBody] PlanMejoraModel PlanMejoraModel)

        {
            try
            {
                if (string.IsNullOrEmpty(PlanMejoraModel.Id.ToString())) return BadRequest("Debe indicar PlanMejoraModel.Id");
                PlanMejoraModel retorno = await _PlanMejoraService.GetPlanMejoraById(PlanMejoraModel);
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
                _PlanMejoraService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("PlanMejoraInsertOrUpdate")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanMejoraModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        //public async Task<ActionResult<PlanMejoraModel>> PlanMejoraInsertOrUpdate( PlanMejoraModel planMejoraModel)
        public async Task<ActionResult> PlanMejoraInsertOrUpdate(PlanMejoraModel planMejoraModel)
        {
            try
            {
                if (string.IsNullOrEmpty(planMejoraModel.Mejora.ToString())) return BadRequest("Debe indicar Mejora");
                if (string.IsNullOrEmpty(planMejoraModel.PreguntaId.ToString())) return BadRequest("Debe indicar PreguntaId");
                if (string.IsNullOrEmpty(planMejoraModel.SegmentacionAreaId.ToString())) return BadRequest("SegmentacionAreaId");
                if (string.IsNullOrEmpty(planMejoraModel.TipoDiferenciaRelacionadaId.ToString())) return BadRequest("TipoDiferenciaRelacionadaId");
                if (string.IsNullOrEmpty(planMejoraModel.TipoImportanciaId.ToString())) return BadRequest("TipoImportanciaId");
                if (string.IsNullOrEmpty(planMejoraModel.AlternativaId.ToString())) return BadRequest("AlternativaId");
                if (string.IsNullOrEmpty(planMejoraModel.SegmentacionAreaId.ToString())) return BadRequest("SegmentacionAreaId");
                if (string.IsNullOrEmpty(planMejoraModel.EvaluacionId.ToString())) return BadRequest("EvaluacionId");
                if (string.IsNullOrEmpty(planMejoraModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                //PlanMejoraModel retorno = await _PlanMejoraService.InsertOrUpdate(planMejoraModel);
                //if (retorno == null) return NotFound();

                //return Ok(retorno);
                await _PlanMejoraService.PlanMejoraInsertOrUpdate(planMejoraModel);
                return Ok(true);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetPlanMejoras")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlanMejoraModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PlanMejoraModel>>> GetPlanMejoras()
        {
            try
            {
                List<PlanMejoraModel> retorno = await _PlanMejoraService.GetPlanMejoras();
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
                _PlanMejoraService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetPlanMejorasByPreguntaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlanMejoraModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<PlanMejoraModel>>> GetPlanMejorasByPreguntaId(PreguntaModel preguntaModel)
        {
            try
            {
                List<PlanMejoraModel> retorno = await _PlanMejoraService.GetPlanMejorasByPreguntaId(preguntaModel);
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
                _PlanMejoraService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

 

    }



}
