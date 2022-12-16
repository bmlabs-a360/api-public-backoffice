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
    public class TipoCantidadEmpleadoController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITipoCantidadEmpleadoService _TipoCantidadEmpleadoService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public TipoCantidadEmpleadoController(
            TipoCantidadEmpleadoService TipoCantidadEmpleadoService,
            ILogger<TipoCantidadEmpleadoController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _TipoCantidadEmpleadoService = TipoCantidadEmpleadoService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
        //[ApiKeyAuth]
        [HttpPost("GetTipoCantidadEmpleadoById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoCantidadEmpleadoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoCantidadEmpleadoModel>> GetTipoCantidadEmpleadoById([FromBody] TipoCantidadEmpleadoModel TipoCantidadEmpleadoModel)

        {
            try
            {
                if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Id.ToString())) return BadRequest("Debe indicar TipoCantidadEmpleadoModel.Id");
                TipoCantidadEmpleadoModel retorno = await _TipoCantidadEmpleadoService.GetTipoCantidadEmpleadoById(TipoCantidadEmpleadoModel);
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
                _TipoCantidadEmpleadoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("TipoCantidadEmpleadoInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoCantidadEmpleadoModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TipoCantidadEmpleadoModel>> TipoCantidadEmpleadoInsertOrUpdate([FromBody] TipoCantidadEmpleadoModel TipoCantidadEmpleadoModel)
        {
            try
            {
                if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Detalle.ToString())) return BadRequest("Debe indicar Detalle");
                if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Nombre.ToString())) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                TipoCantidadEmpleadoModel retorno = await _TipoCantidadEmpleadoService.InsertOrUpdate(TipoCantidadEmpleadoModel);
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
        [HttpPost("GetTipoCantidadEmpleados")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoCantidadEmpleadoModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TipoCantidadEmpleadoModel>>> GetTipoCantidadEmpleados()
        {
            try
            {
                List<TipoCantidadEmpleadoModel> retorno = await _TipoCantidadEmpleadoService.GetTipoCantidadEmpleados();
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
                _TipoCantidadEmpleadoService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        
    }
}
