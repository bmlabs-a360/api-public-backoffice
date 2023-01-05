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
    public class ImportanciaRelativaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IImportanciaRelativaService _ImportanciaRelativaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ImportanciaRelativaController(
            ImportanciaRelativaService ImportanciaRelativaService,
            ILogger<ImportanciaRelativaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ImportanciaRelativaService = ImportanciaRelativaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetImportanciaRelativaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImportanciaRelativaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ImportanciaRelativaModel>> GetImportanciaRelativaById([FromBody] ImportanciaRelativaModel ImportanciaRelativaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ImportanciaRelativaModel.Id.ToString())) return BadRequest("Debe indicar ImportanciaRelativaModel.Id");
                ImportanciaRelativaModel retorno = await _ImportanciaRelativaService.GetImportanciaRelativaById(ImportanciaRelativaModel);
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
                _ImportanciaRelativaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ImportanciaRelativaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImportanciaRelativaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ImportanciaRelativaModel>> ImportanciaRelativaInsertOrUpdate([FromBody] ImportanciaRelativaModel ImportanciaRelativaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ImportanciaRelativaModel.EvaluacionEmpresaId.ToString())) return BadRequest("Debe indicar EvaluacionEmpresaId");
                if (string.IsNullOrEmpty(ImportanciaRelativaModel.SegmentacionAreaId.ToString())) return BadRequest("Debe indicar SegmentacionAreaId");
                if (string.IsNullOrEmpty(ImportanciaRelativaModel.Valor.ToString())) return BadRequest("Valor");
                if (string.IsNullOrEmpty(ImportanciaRelativaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                ImportanciaRelativaModel retorno = await _ImportanciaRelativaService.InsertOrUpdate(ImportanciaRelativaModel);
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
        [HttpPost("GetImportanciaRelativas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImportanciaRelativaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ImportanciaRelativaModel>>> GetImportanciaRelativas()
        {
            try
            {
                List<ImportanciaRelativaModel> retorno = await _ImportanciaRelativaService.GetImportanciaRelativas();
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
                _ImportanciaRelativaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetImportanciaRelativasByEvaluacionEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImportanciaRelativaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ImportanciaRelativaModel>>> GetImportanciaRelativasByEvaluacionEmpresaId(EvaluacionEmpresaModel evaluacionEmpresaModel)
        {
            try
            {
                List<ImportanciaRelativaModel> retorno = await _ImportanciaRelativaService.GetImportanciaRelativasByEvaluacionEmpresaId(evaluacionEmpresaModel);
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
                _ImportanciaRelativaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetImportanciaRelativasBySegmentacionAreaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImportanciaRelativaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ImportanciaRelativaModel>>> GetImportanciaRelativasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            try
            {
                List<ImportanciaRelativaModel> retorno = await _ImportanciaRelativaService.GetImportanciaRelativasBySegmentacionAreaId(segmentacionAreaModel);
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
                _ImportanciaRelativaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


    }



}
