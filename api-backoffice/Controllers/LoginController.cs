using api_public_backOffice.Models;
using api_public_backOffice.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using api_public_backOffice.Helpers;

using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;
using api_public_backOffice.Interceptors;
using System.Security.Claims;
using System.Collections.Generic;

namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    //[ApiKeyAuth]
    [Route("/api/v1/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly ISecurityHelper _securityHelper;
        private readonly IControlTokenService _controlTokenService;
        private readonly Helpers.IUrlHelper _urlHelper;

        public LoginController(
            UsuarioService usuarioService, 
            TokenService tokenService,
            UrlHelper urlHelper,
            SecurityHelper securityHelper,
            ControlTokenService controlTokenService,
            ILogger<LoginController> logger)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
            _controlTokenService = controlTokenService;
        }

        /*[ReCaptchaAuth("Authenticate")]
        [AllowAnonymous]*/
        [HttpPost("Authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseLoginModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ResponseLoginModel>> Authenticate([FromBody] LoginModel loginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModel.Email)) return BadRequest("Debe indicar Email");
                if (string.IsNullOrEmpty(loginModel.Password)) return BadRequest("Debe indicar Password");
                UsuarioModel userModel = await _usuarioService.GetUserLogin(loginModel);
                if (userModel == null) return Unauthorized();
                userModel.FechaUltimoAcceso = DateTime.Now;
                _ = await _usuarioService.Update(userModel);
                var retorno = new ResponseLoginModel
                {
                    UsuarioModel = userModel,
                    TokenBearer = "_tokenService.BuildToken(userModel)"//_tokenService.BuildToken(userModel)
                };

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
                _usuarioService.Dispose();
                //_controlTokenService.Dispose();
            }
        }
        
        
        //[AllowAnonymous]
        [HttpPost("ValidateToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return BadRequest();
                var result = _tokenService.IsTokenValid(token);
                if (!result) return Unauthorized();
                return Ok(result);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _usuarioService.Dispose();
                _controlTokenService.Dispose();
            }
        }

        //[ReCaptchaAuth("ValidateSimpleToken")]
        //[AllowAnonymous]
        [HttpPost("ValidateSimpleToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> ValidateSimpleToken(string uri)
        {
            try
            {
                if (string.IsNullOrEmpty(uri)) return BadRequest();

                string uriDecrypt = _securityHelper.Decrypt(uri);
                Dictionary<string, string> listValues = _urlHelper.GetValuesFromUrlEncode(uriDecrypt);

                if (string.IsNullOrEmpty(listValues["token"])) return Unauthorized();
                
                //Validamos tiempo de expiracion
                var resultJwtValidation = _tokenService.IsTokenValidSimpleToken(listValues["token"]);
                if (!resultJwtValidation) return Unauthorized();

                //Validamos uso del token
                var result = await _controlTokenService.IsValidToken(listValues["token"], DateTime.Now, true);
                if (!result) return Unauthorized();

                return Ok(result);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
            finally
            {
                _usuarioService.Dispose();
                _controlTokenService.Dispose();
            }
        }
        
        [HttpPost("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseLoginModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ResponseLoginModel>> RefreshToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return BadRequest();
                if (!_tokenService.IsTokenValid(token)) return Unauthorized();

                var claims = _tokenService.GetTokenInfo(token);
                string idusuario = claims[ClaimTypes.SerialNumber];
                
                Guid id;
                if (!Guid.TryParse(idusuario, out id)) throw new ArgumentNullException(nameof(idusuario));
                
                var userModel = await _usuarioService.GetById(id);
                userModel.FechaUltimoAcceso = DateTime.Now;
                _ = await _usuarioService.Update(userModel);

                var retorno = new ResponseLoginModel
                {
                    UsuarioModel = userModel,
                    TokenBearer = _tokenService.BuildToken(userModel)
                };

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
                _usuarioService.Dispose();
                _controlTokenService.Dispose();
            }
        }
    }
}
