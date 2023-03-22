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
using System.Text.Json;
using neva.entities;
using Elastic.Apm.Api;

namespace api_public_backOffice.Controllers
{
    [TypeFilter(typeof(InterceptorLogAttribute))]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IPerfilService _PerfilService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IMailService _mailService;
        private readonly ITokenService _tokenService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISecurityHelper _securityHelper;
        private readonly IControlTokenService _controlTokenService;
        private readonly Helpers.IUrlHelper _urlHelper;

        public IConfiguration Configuration { get; }

        public UsuarioController(
            UsuarioService usuarioService,
            PerfilService PerfilService,
            UsuarioEmpresaService UsuarioEmpresaService,
            ILogger<UsuarioController> logger,
            MailService mailService,
            EmailHelper emailHelper,
            TokenService tokenService,
            SecurityHelper securityHelper,
            UrlHelper urlHelper,
            ControlTokenService controlTokenService,
            IConfiguration configuration)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _PerfilService = PerfilService;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _mailService = mailService;
            _emailHelper = emailHelper;
            _tokenService = tokenService;
            Configuration = configuration;
            _securityHelper = securityHelper;
            _urlHelper = urlHelper;
            _controlTokenService = controlTokenService;
        }

        [ReCaptchaAuth("UsuarioCreate")]
        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPost("UsuarioCreate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> UsuarioCreate([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioModel.Nombres)) return BadRequest("Debe indicar nombre");
                if (string.IsNullOrEmpty(usuarioModel.Password)) return BadRequest("Debe indicar contraseña");
                if (string.IsNullOrEmpty(usuarioModel.Email)) return BadRequest("Debe indicar email");
                //if (string.IsNullOrEmpty(usuarioModel.Telefono)) return BadRequest("Debe indicar Telefono");
                if (string.IsNullOrEmpty(usuarioModel.Empresa.RazonSocial)) return BadRequest("Debe indicar razón social");
                if (string.IsNullOrEmpty(usuarioModel.Empresa.RutEmpresa)) return BadRequest("Debe indicar rut empresa");
                if (string.IsNullOrEmpty(Configuration["Url:usuario.confirmacion"])) return BadRequest("Debe configurar el valor url: usuario.confirmacion");

                /*
                //validacion de token
                string uriDecrypt = _securityHelper.Decrypt(uri);
                Dictionary<string, string> listValues = _urlHelper.GetValuesFromUrlEncode(uriDecrypt);

                //bool isValid = (_tokenService.IsTokenValidSimpleToken(listValues["token"]) || _tokenService.IsTokenValid(listValues["token"]));
                //if (!isValid) return Unauthorized();

                if (!usuarioModel.Email.Equals(listValues["email"])) BadRequest("Debe indicar Email");
                //validacion de token
                
               
                string uriDecrypt = _securityHelper.Decrypt(uri);
                Dictionary<string, string> listValues = _urlHelper.GetValuesFromUrlEncode(uriDecrypt);

                //bool isValid = (_tokenService.IsTokenValidSimpleToken(listValues["token"]) || _tokenService.IsTokenValid(listValues["token"]));
                //if (!isValid) return Unauthorized();

                if (!usuarioModel.Email.Equals(listValues["email"])) BadRequest("Debe indicar Email");
                if (Guid.TryParse(listValues["perfilId"], out Guid idPerfl))
                {
                 usuarioModel.PerfilId = idPerfl; 
                } 
                */

                UsuarioModel retorno = await _usuarioService.InsertOrUpdate(usuarioModel);
                if (retorno == null) return NotFound();

                string linkIrFormulario = Configuration["Url:usuario.confirmacion"];
                string toEncode = "RazonSocial=" + usuarioModel.Empresa.RazonSocial + "&RutEmpresa=" + usuarioModel.Empresa.RutEmpresa;
                linkIrFormulario += "?uri=" + HttpUtility.UrlEncode(_securityHelper.Encrypt(toEncode));


                MailDTO correo = new MailDTO
                {
                    From = Configuration["Mail:FromConfirmacion"],
                    FromAlias = "Confirmacion",
                    To = new string[] { usuarioModel.Email },
                    ToAlias = new string[] { usuarioModel.Nombres },
                    IsHtml = true,
                    Subject = "Confirmar mail",
                    Body = _emailHelper.GetBodyEmailConfirmacion(linkIrFormulario)
                };

                string jsonString = JsonSerializer.Serialize(correo);
                _ = await _mailService.SendMailAsync(correo);

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
                // _controlTokenService.Dispose();
            }
        }
        /*
        [ReCaptchaAuth("CompletaDatos")]
        [ApiKeyAuth]
        [HttpPost("CompletaDatos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> CompletaDatos([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioModel.)) return BadRequest("Debe indicar Nombre");
                if (string.IsNullOrEmpty(usuarioModel.Password)) return BadRequest("Debe indicar Contraseña");
                if (string.IsNullOrEmpty(usuarioModel.Email)) return BadRequest("Debe indicar Email");
                //if (string.IsNullOrEmpty(usuarioModel.Telefono)) return BadRequest("Debe indicar Telefono");
                if (string.IsNullOrEmpty(usuarioModel.Empresa.RazonSocial)) return BadRequest("Debe indicar Razón Social");
                if (string.IsNullOrEmpty(usuarioModel.Empresa.RutEmpresa)) return BadRequest("Debe indicar Rut Rut Empresa");
                if (string.IsNullOrEmpty(Configuration["Url:usuario.confirmacion"])) return BadRequest("Debe configurar el valor url: usuario.confirmacion");


                UsuarioModel retorno = await _usuarioService.InsertOrUpdate(usuarioModel);
                if (retorno == null) return NotFound();

                string linkIrFormulario = Configuration["Url:usuario.confirmacion"];
                string toEncode = "email=" + usuarioModel.Email;
                linkIrFormulario += "?uri=" + HttpUtility.UrlEncode(_securityHelper.Encrypt(toEncode));

               _ = await _mailService.SendMailAsync(new MailDTO
                {
                    From = Configuration["Mail:FromConfirmacion"],
                    FromAlias = "Confirmacion",
                    To = new string[] { usuarioModel.Email },
                    ToAlias = new string[] { usuarioModel.Nombres },
                    IsHtml = true,
                    Subject = "Confirmar mail",
                    Body = _emailHelper.GetBodyEmailConfirmacion(linkIrFormulario)
                });

                return Ok(retorno);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                _logger.LogError("Error  Source:{0}, Trace:{1} ", e.Source, e);
                return Problem(detail: e.Message, title: "ERROR");
            }
        }
        */
        
        //[AllowAnonymous]
        [HttpGet("Confirmacion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> Confirmacion(string uri)
        {
            try
            {
                if (string.IsNullOrEmpty(uri)) return BadRequest("uri");
                if (string.IsNullOrEmpty(Configuration["Url:usuario.login"])) return BadRequest("Debe configurar el valor url: usuario.login");

                string uriDecrypt = _securityHelper.Decrypt(uri);
                Dictionary<string, string> listValues = _urlHelper.GetValuesFromUrlEncode(uriDecrypt);

                if (string.IsNullOrEmpty(listValues["RazonSocial"])) return BadRequest("RazonSocial");
                if (string.IsNullOrEmpty(listValues["RutEmpresa"])) return BadRequest("RutEmpresa");

                //UsuarioModel retorno = await _usuarioService.ConfirmarUsuario(listValues["rutempresa"]);
                //if (retorno == null) return NotFound();
                string urlReturn = Configuration["Url:usuario.completadatos"];
                return Redirect(urlReturn);
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

        /*
        [ReCaptchaAuth("SolicitudRecuperarContrasena")]
        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPost("SolicitudRecuperarContrasena")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputMail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<OutputMail>> SolicitudRecuperarContrasena(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return BadRequest("email");
                UsuarioModel usuarioModel = await _usuarioService.GetUsuarioByEmail(email);
                if (usuarioModel == null) return NotFound("Usuario no existe");

                string token = _tokenService.BuildTokenSimpleToken(Configuration["Jwt:DurationMinutesTokenRecuperacion"]);
                string linkIrFormulario = Configuration["Url:usuario.recuperar"];

                _ = await _controlTokenService.SaveOrUpdateControlToken(new ControlTokenModel
                {
                    Token = token,
                    Activo = true
                });

                string toEncode = "email=" + usuarioModel.Email + "&token=" + token;
                linkIrFormulario += "?uri=" + HttpUtility.UrlEncode(_securityHelper.Encrypt(toEncode));

                var retorno = await _mailService.SendMailAsync(new MailDTO
                {
                    From = Configuration["Mail:FromRecuperacion"],
                    FromAlias = "Recuperacion",
                    To = new string[] { usuarioModel.Email },
                    ToAlias = new string[] { usuarioModel.Nombres },
                    IsHtml = true,
                    Subject = "Recuperacion contrasena",
                    Body = _emailHelper.GetBodyEmailRecuperacion(linkIrFormulario)
                });

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

        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPost("RecuperarContrasena")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> RecuperarContrasena(string uri, string pass)
        {
            try
            {
                if (string.IsNullOrEmpty(uri)) return BadRequest("Rut invalido");
                if (string.IsNullOrEmpty(pass)) return BadRequest("Pass invalido");

                string uriDecrypt = _securityHelper.Decrypt(uri);
                Dictionary<string, string> listValues = _urlHelper.GetValuesFromUrlEncode(uriDecrypt);

                if (string.IsNullOrEmpty(listValues["email"])) return BadRequest("email");

                var usuarioModel = await _usuarioService.GetUsuarioByEmail(listValues["email"]);
                if (usuarioModel == null) return NotFound("Usuario no existe");

                usuarioModel.Password = pass;
                _ = await _usuarioService.InsertOrUpdate(usuarioModel);
                return Ok(true);
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
        }*/

        //[ApiKeyAuth]
        //[Authorize]
        [HttpPost("GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> GetUsuario([FromBody] LoginModel loginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModel.Email)) return BadRequest("Debe indicar Email");
                if (string.IsNullOrEmpty(loginModel.Password)) return BadRequest("Debe indicar Password");
                UsuarioModel retorno = await _usuarioService.GetUser(loginModel);
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
               // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        //[Authorize]
        [HttpPost("InsertOrUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> InsertOrUpdate([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioModel.Email)) return BadRequest("Debe indicar Email");
                if (string.IsNullOrEmpty(usuarioModel.Password)) return BadRequest("Debe indicar Password");
                if (string.IsNullOrEmpty(usuarioModel.Nombres)) return BadRequest("Debe indicar Nombres");
                UsuarioModel retorno = await _usuarioService.InsertOrUpdate(usuarioModel);
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

        /*[ApiKeyAuth]
        [Authorize]
        [HttpPost("SolicitudCreacionUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputMail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<OutputMail>> SolicitudCreacionUsuario(Solicitud solicitud)
        {
            try
            {
                if (solicitud.Idusuario == null) throw new ArgumentNullException("Debe inidicar idusuario");
                if (string.IsNullOrEmpty(solicitud.EmailDestinatario)) throw new ArgumentNullException("Debe inidicar Email Destinatario");
                if (string.IsNullOrEmpty(solicitud.DestinatarioAlias)) throw new ArgumentNullException("Debe inidicar Destinatario alias");

                string linkIrFormulario = Configuration["Url:usuario.registrar"];
                string token = _tokenService.BuildTokenSimpleToken(Configuration["Jwt:DurationMinutesTokenCreateUser"]);
                string toEncodeUrl = "token=" + token;
                toEncodeUrl += "&email=" + solicitud.EmailDestinatario;
                toEncodeUrl += "&perfilId=" + solicitud.perfilId;

                linkIrFormulario += "?email=" + solicitud.EmailDestinatario + "&uri=" + HttpUtility.UrlEncode(_securityHelper.Encrypt(toEncodeUrl));

                _ = await _controlTokenService.SaveOrUpdateControlToken(new ControlTokenModel
                {
                    Token = token,
                    Activo = true
                });

                var user = await _usuarioService.GetById(solicitud.Idusuario);
                string body = _emailHelper.GetBodyEmailSolicitudCreacionUsuario(user.Nombres, solicitud.DestinatarioAlias, linkIrFormulario);

                if (string.IsNullOrEmpty(body)) throw new Exception("No se pudo obtener body del email");

                MailDTO mailDTO = new MailDTO
                {
                    From = user.Email,
                    FromAlias = "Registro de usuario",
                    To = new string[] { solicitud.EmailDestinatario },
                    ToAlias = new string[] { solicitud.DestinatarioAlias },
                    IsHtml = true,
                    Subject = "noreply",
                    Body = body
                };

                var retorno = await _mailService.SendMailAsync(mailDTO);
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

        [ApiKeyAuth]
        [Authorize]
        [HttpPost("ToggleActivo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputMail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<OutputMail>> ToggleActivo(int? idusuario, int? idperfil,bool activo)
        {
            try
            {
                if (idusuario == null) throw new ArgumentNullException("Debe inidicar idusuario");
                var user = await _usuarioService.GetById(idusuario);
                user.Activo = activo;
                user.PerfilId = idperfil;
                var retorno = await _usuarioService.Update(user);
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
        */
        //[ApiKeyAuth]
        //[Authorize]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioModel>>> GetAll()
        {
            try
            {
                List<UsuarioModel> retorno = await _usuarioService.GetAll();
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

        //[ApiKeyAuth]
        [HttpPost("DeleteCascade")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<int>> DeleteCascade([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) return BadRequest("Debe indicar usuarioModel.Id");
                return await _usuarioService.DeleteCascade(usuarioModel);

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
                _usuarioService.Dispose();
                // _controlTokenService.Dispose();
            }
        }

        //[ApiKeyAuth]
        //[Authorize]
        [HttpGet("GetAllConsultor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioModel>>> GetAllConsultor()
        {
            try
            {
                List<UsuarioModel> retorno = await _usuarioService.GetAllConsultor();
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

        //[ApiKeyAuth]
        //[Authorize]
        [HttpPost("GetAllFilter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuarioModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<UsuarioModel>>> GetAllFilter(Guid perfilId, [FromBody] UsuarioModel UsuarioModel)
        {
            try
            {
                List<UsuarioModel> retorno = new List<UsuarioModel> { };
                if (string.IsNullOrEmpty(perfilId.ToString())) return BadRequest("Debe indicar perfilId");

                PerfilModel perfil = new PerfilModel
                {
                    Id = perfilId
                };

                PerfilModel perfilModel = await _PerfilService.GetPerfilById(perfil);

                if (perfilModel.Nombre == "Consultor")
                {
                    List<UsuarioEmpresaModel> usuarioEmpresa = await _UsuarioEmpresaService.GetUsuarioEmpresasByUsuario(UsuarioModel);
                    List<UsuarioModel> usuarios = await _usuarioService.GetAllConsultor();
                    foreach (var ue in usuarioEmpresa)
                    {
                        foreach (var u in usuarios)
                        {
                            if (ue.UsuarioId == u.Id)
                            {
                                int indiceUsuario = retorno.IndexOf(u);
                                if (indiceUsuario == -1)
                                {
                                    u.Password = "";
                                    retorno.Add(u);
                                }
                            }
                        }
                    }
                }
                else 
                {
                    List<UsuarioModel> usuarios = await _usuarioService.GetAll();
                    foreach (var u in usuarios)
                    {
                            u.Password = "";
                            retorno.Add(u);
                    }
                }
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

        [HttpPost("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UsuarioModel>> UpdateUser([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) return BadRequest("Debe indicar idusuario");
                if (string.IsNullOrEmpty(usuarioModel.Nombres)) return BadRequest("Debe indicar nombres");
                if (!string.IsNullOrEmpty(usuarioModel.Password)) //return BadRequest("Debe indicar Password");
                    //if (usuarioModel.Password.Length < 8 || usuarioModel.Password.Length > 50) return BadRequest("Contraseña debe tener un mínimo de 8 y máximo 50 caracteres");
                if (string.IsNullOrEmpty(usuarioModel.Email)) return BadRequest("Debe indicar Email");
                if (string.IsNullOrEmpty(usuarioModel.Activo.ToString())) return BadRequest("Debe indicar Activo");

                UsuarioModel retorno = await _usuarioService.UpdateUser(usuarioModel);
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
    }

    public class Solicitud
    {
        public int? Idusuario { get; set; }
        public string EmailDestinatario { get; set; }
        public string DestinatarioAlias { get; set; }
        public int? perfilId { get; set; }
    }

}
