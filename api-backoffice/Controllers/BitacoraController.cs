using Microsoft.AspNetCore.Mvc;
using api_public_backOffice.Interceptors;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using api_public_backOffice.Service;
using api_public_backOffice.Models;

using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;
using Microsoft.AspNetCore.Authorization;


namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    //[Authorize]
    //[ApiKeyAuth]
    [Route("/api/v1/[controller]")]
    public class BitacoraController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBitacoraService _bitacoraService;

        public BitacoraController(BitacoraService bitacoraService, ILogger<BitacoraController> logger)
        {
            _logger = logger;
            _bitacoraService = bitacoraService;
        }

        [HttpPost("GetBitacorasByUsuarioId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BitacoraModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<BitacoraModel>>> GetBitacorasByUsuarioId([FromBody] BitacoraModel bitacoraModel)
        {
            try
            {
                if (string.IsNullOrEmpty(bitacoraModel.UsuarioId.ToString())) return BadRequest("Debe informar bitacoraModel.UsuarioId.");
                List<BitacoraModel> retorno = await _bitacoraService.GetBitacorasByUsuarioId(bitacoraModel);
                if (!retorno.Any()) return NotFound();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
        }
       
        
        [HttpPost("BitacoraInsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BitacoraModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<BitacoraModel>> BitacoraInsertOrUpdate([FromBody] BitacoraModel input)

        {
            try
            {
               // if (input == null) return BadRequest("Debe indicar Bitacora");

                BitacoraModel retorno = await _bitacoraService.InsertOrUpdate(input);
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

    }
}
