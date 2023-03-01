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
    public class IndicadoresController : Controller
    {
        private readonly ILogger _logger;
        private readonly IIndicadoresService _IndicadoresService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public IndicadoresController(
            IndicadoresService IndicadoresService,
            ILogger<IndicadoresController> logger,
            EmailHelper emailHelper,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            IConfiguration configuration)
        {
            _logger = logger;
            _IndicadoresService = IndicadoresService;
            _emailHelper = emailHelper;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
        }
       
        //[ApiKeyAuth]
        [HttpPost("CantidadEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> CantidadEmpresas()
        {
            try
            {
                return await _IndicadoresService.CantidadEmpresas();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
        //[ApiKeyAuth]
        //[HttpPost("CantidadEmpresasSql")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        //public  ActionResult<int> CantidadEmpresasSql()
        //{
        //    try
        //    {
        //        return  _IndicadoresService.CantidadEmpresasSql();
        //    }
        //    catch (Exception e)
        //    {
        //        while (e.InnerException != null) e = e.InnerException;
        //        _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
        //        return Problem(detail: e.Message, title: "ERROR");
        //    }
        //    finally
        //    {
        //        _IndicadoresService.Dispose();
        //        // _controlTokenService.Dispose();
        //    }
        //}



        //[ApiKeyAuth]
        [HttpPost("CantidadEmpresasSuscripcion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<int> CantidadEmpresasSuscripcion()
        {
            try
            {
                return _IndicadoresService.CantidadEmpresasSuscripcion();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }//[ApiKeyAuth]
        [HttpPost("CantidadGranEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<int> CantidadGranEmpresa()
        {
            try
            {
                return _IndicadoresService.CantidadGranEmpresa();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }//[ApiKeyAuth]
        [HttpPost("CantidaEmpresasEvaluacionProceso")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<int> CantidaEmpresasEvaluacionProceso()
        {
            try
            {
                return _IndicadoresService.CantidaEmpresasEvaluacionProceso();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }//[ApiKeyAuth]
        [HttpPost("CantidadEmpresasEvaluacionFinalizada")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<int> CantidadEmpresasEvaluacionFinalizada()
        {
            try
            {
                return _IndicadoresService.CantidadEmpresasEvaluacionFinalizada();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("PromedioIMTamanoEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<List<PromedioIMTamanoEmpresaDto>> PromedioIMTamanoEmpresa()
        {
            try
            {
                return _IndicadoresService.PromedioIMTamanoEmpresa();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("PromedioIMRubro")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<List<PromedioIMRubroDto>> PromedioIMRubro()
        {
            try
            {
                return _IndicadoresService.PromedioIMRubro();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }


        [HttpPost("PromedioIMTamanoEmpresaByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<List<PromedioIMTamanoEmpresaDto>> PromedioIMTamanoEmpresaByEvaluacionId(EvaluacionModel evalaucion)
        {
            try
            {
                return _IndicadoresService.PromedioIMTamanoEmpresaByEvaluacionId(evalaucion);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        [HttpPost("PromedioIMRubroByEvaluacionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public ActionResult<List<PromedioIMRubroDto>> PromedioIMRubroByEvaluacionId(EvaluacionModel evalaucion)
        {
            try
            {
                return _IndicadoresService.PromedioIMRubroByEvaluacionId(evalaucion);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _IndicadoresService.Dispose();
                // _controlTokenService.Dispose();
            }
        }
    }



}
