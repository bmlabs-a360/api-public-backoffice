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
    public class MadurezController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMadurezService _madurezService;
        //private readonly IUsuarioSuscripcionService _usuarioSubscripcionService;
        //private readonly IReporteService _reporteService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public MadurezController(
            //UsuarioSuscripcionService usuarioSuscripcionService,
            //ReporteService reporteService,
            MadurezService madurezService,
            ILogger<MadurezController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            //_usuarioSubscripcionService = usuarioSuscripcionService;
            //_reporteService = reporteService;
            _madurezService = madurezService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
      

        //[ApiKeyAuth]
        [HttpPost("GetCapacidadSubAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MadurezCapacidadSubAreaDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<MadurezCapacidadSubAreaDto>>> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            try
            {
                List<MadurezCapacidadSubAreaDto> retorno =  _madurezService.GetCapacidadSubAreas(madurezCapacidadSubArea);
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetIMSA")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMSADto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMSADto>>> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            try
            {
                List<IMSADto> retorno = _madurezService.GetIMSA(madurezCapacidadSubArea);
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        [HttpPost("GetIMA")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMADto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMADto>>> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            try
            {
                List<IMADto> retorno = _madurezService.GetIMA(madurezCapacidadSubArea);
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetIM")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMDto>>> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubArea)
        {
            try
            {
                List<IMDto> retorno = _madurezService.GetIM(madurezCapacidadSubArea);
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }




        [HttpPost("GetAllCapacidadSubAreas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MadurezCapacidadSubAreaDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<MadurezCapacidadSubAreaDto>>> GetAllCapacidadSubAreas( )
        {
            try
            {
                List<MadurezCapacidadSubAreaDto> retorno = _madurezService.GetAllCapacidadSubAreas();
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        [HttpPost("GetAllIMSA")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMSADto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMSADto>>> GetAllIMSA( )
        {
            try
            {
                List<IMSADto> retorno = _madurezService.GetAllIMSA();
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        [HttpPost("GetAllIMA")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMADto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMADto>>> GetAllIMA( )
        {
            try
            {
                List<IMADto> retorno = _madurezService.GetAllIMA();
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetAllIM")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMDto>>> GetAllIM( )
        {
            try
            {
                List<IMDto> retorno = _madurezService.GetAllIM();
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("GetIMAReporteSubscripcionOBasico")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IMADto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<IMADto>>> GetIMAReporteSubscripcionOBasico([FromBody] UsuarioModel usuario, Guid evaluacionId, Guid empresaId )
        {
            try
            {
                List<IMADto> retorno = await _madurezService.GetIMAReporteSubscripcionOBasico(usuario, evaluacionId, empresaId);
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
                _madurezService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

    }



}
