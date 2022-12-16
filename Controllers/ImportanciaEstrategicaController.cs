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
    public class ImportanciaEstrategicaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IImportanciaEstrategicaService _ImportanciaEstrategicaService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public ImportanciaEstrategicaController(
            ImportanciaEstrategicaService ImportanciaEstrategicaService,
            ILogger<ImportanciaEstrategicaController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _ImportanciaEstrategicaService = ImportanciaEstrategicaService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetImportanciaEstrategicaById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImportanciaEstrategicaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ImportanciaEstrategicaModel>> GetImportanciaEstrategicaById([FromBody] ImportanciaEstrategicaModel ImportanciaEstrategicaModel)

        {
            try
            {
                if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.Id.ToString())) return BadRequest("Debe indicar ImportanciaEstrategicaModel.Id");
                ImportanciaEstrategicaModel retorno = await _ImportanciaEstrategicaService.GetImportanciaEstrategicaById(ImportanciaEstrategicaModel);
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
                _ImportanciaEstrategicaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("ImportanciaEstrategicaInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImportanciaEstrategicaModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ImportanciaEstrategicaModel>> ImportanciaEstrategicaInsertOrUpdate([FromBody] ImportanciaEstrategicaModel ImportanciaEstrategicaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.ImportanciaRelativaId.ToString())) return BadRequest("Debe indicar ImportanciaRelativaId");
                if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.SegmentacionSubAreaId.ToString())) return BadRequest("Debe indicar SegmentacionSubAreaId");
                if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.Valor.ToString())) return BadRequest("Valor");
                if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                ImportanciaEstrategicaModel retorno = await _ImportanciaEstrategicaService.InsertOrUpdate(ImportanciaEstrategicaModel);
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
        [HttpPost("GetImportanciaEstrategicas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImportanciaEstrategicaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ImportanciaEstrategicaModel>>> GetImportanciaEstrategicas()
        {
            try
            {
                List<ImportanciaEstrategicaModel> retorno = await _ImportanciaEstrategicaService.GetImportanciaEstrategicas();
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
                _ImportanciaEstrategicaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetImportanciaEstrategicasByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImportanciaEstrategicaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ImportanciaEstrategicaModel>>> GetImportanciaEstrategicasByImportanciaRelativaId(ImportanciaRelativaModel importanciaRelativaModel)
        {
            try
            {
                List<ImportanciaEstrategicaModel> retorno = await _ImportanciaEstrategicaService.GetImportanciaEstrategicasByImportanciaRelativaId(importanciaRelativaModel);
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
                _ImportanciaEstrategicaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetImportanciaEstrategicasByEmpresaId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImportanciaEstrategicaModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<ImportanciaEstrategicaModel>>> GetImportanciaEstrategicasBySegmentacionSubAreaId(SegmentacionSubAreaModel segmentacionSubAreaModel)
        {
            try
            {
                List<ImportanciaEstrategicaModel> retorno = await _ImportanciaEstrategicaService.GetImportanciaEstrategicasBySegmentacionSubAreaId(segmentacionSubAreaModel);
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
                _ImportanciaEstrategicaService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


    }



}
