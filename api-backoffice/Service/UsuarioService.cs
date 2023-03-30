using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using api_public_backOffice.Models;
using api_public_backOffice.Repository;
using api_public_backOffice.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using neva.entities;

namespace api_public_backOffice.Service
{
    public interface IUsuarioService
    {
        Task<UsuarioModel> GetUser(LoginModel loginModel);
        Task<UsuarioModel> InsertOrUpdate(UsuarioModel usuario);
        Task<UsuarioModel> Update(UsuarioModel usuario);
        Task<UsuarioModel> ConfirmarUsuario(string email);
       // Task<UsuarioModel> GetUsuarioByRut(string rut);
        Task<UsuarioModel> GetUsuarioByEmail(string email);
        Task<List<UsuarioModel>> GetAll();
        Task<List<UsuarioModel>> GetAllConsultor();
        
        Task<UsuarioModel> GetById(Guid? id);
        Task<UsuarioModel> UpdateUser(UsuarioModel usuario);
        Task<int> DeleteCascade(UsuarioModel usuarioModel);
        Task<UsuarioModel> GetUsuarioByPerfilIdEmpresaId(Guid perfilId, Guid empresaId);

        void Dispose();
    }
    public class UsuarioService : IUsuarioService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IUsuarioRepository _usuarioRepository;
  
       private IEmpresaRepository _empresaRepository;
        private ISecurityHelper _securityHelper;
        private IUsuarioEmpresaRepository _usuarioEmpresaRepository;
        private IUsuarioSuscripcionRepository _usuarioSuscripcionRepository;

        private IPerfilRepository _perfilRepository;
        private IUsuarioEvaluacionRepository _usuarioEvaluacionRepository;
        private IUsuarioAreaRepository _usuarioAreaRepository;

        public UsuarioService(IMapper mapper, IMemoryCache memoryCache,UsuarioAreaRepository usuarioAreaRepository, UsuarioEvaluacionRepository usuarioEvaluacionRepository, UsuarioRepository usuarioRepository, UsuarioEmpresaRepository usuarioEmpresaRepository, PerfilRepository perfilRepository, EmpresaRepository empresaRepository , SecurityHelper securityHelper, UsuarioSuscripcionRepository usuarioSuscripcionRepository)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _usuarioRepository = usuarioRepository;
            _securityHelper = securityHelper;
            _usuarioEmpresaRepository = usuarioEmpresaRepository; 
            _empresaRepository = empresaRepository;
            _perfilRepository = perfilRepository;
            _usuarioEvaluacionRepository = usuarioEvaluacionRepository;
            _usuarioAreaRepository = usuarioAreaRepository;
            _usuarioSuscripcionRepository = usuarioSuscripcionRepository;

        }
        public async Task<UsuarioModel> GetUser(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Email)) throw new ArgumentNullException("Email");
            if (string.IsNullOrEmpty(loginModel.Password)) throw new ArgumentNullException("Password");
            loginModel.Password = _securityHelper.EncryptPassword(loginModel.Password);
            var usuario = await _usuarioRepository.GetUser(loginModel);
            return _mapper.Map<UsuarioModel>(usuario);
        }
        public async Task<UsuarioModel> InsertOrUpdate(UsuarioModel usuario)
        {
            if (string.IsNullOrEmpty(usuario.Nombres)) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(usuario.Password)) throw new ArgumentNullException("Contraseña");
            if (string.IsNullOrEmpty(usuario.Email)) throw new ArgumentNullException("Email");
            //if (string.IsNullOrEmpty(usuario.Telefono)) throw new ArgumentNullException("Telefono");
            //if (string.IsNullOrEmpty(usuario.Empresa.RazonSocial)) throw new ArgumentNullException("Razón social");
           // if (string.IsNullOrEmpty(usuario.Empresa.RutEmpresa)) throw new ArgumentNullException("Rut empresa");

            usuario.Password = _securityHelper.EncryptPassword(usuario.Password);
            if (usuario.PerfilId.Equals(Guid.Empty))
            {
                var perfil = await _perfilRepository.GetPerfilByName("Usuario pro (empresa)");
                usuario.PerfilId = perfil.Id;
            }
            //var empresa = await _empresaRepository.GetEmpresaByRutEmpresa(usuario.Empresa.RutEmpresa);

            var retorno = await _usuarioRepository.InsertOrUpdate(_mapper.Map<Usuario>(usuario));
            if (retorno.UsuarioEmpresas.Count > 0) {
                List<UsuarioEmpresa> UsuarioEmpresasNew = new List<UsuarioEmpresa>();
               
                await _usuarioEmpresaRepository.DeleteByUsuarioId(_mapper.Map<Usuario>(usuario));

                foreach (UsuarioEmpresa usuarioEmpresa in retorno.UsuarioEmpresas)
                    UsuarioEmpresasNew.Add( await _usuarioEmpresaRepository.InsertOrUpdate(usuarioEmpresa));

                retorno.UsuarioEmpresas = UsuarioEmpresasNew;
            }
            if (retorno.UsuarioEvaluacions.Count > 0)
            {
                List<UsuarioEvaluacion> usuarioEvaluacionsNew = new List<UsuarioEvaluacion>();

                foreach (UsuarioEvaluacion usuarioEvaluacion in retorno.UsuarioEvaluacions)
                {
                    UsuarioEvaluacion salidaUsuarioEvaluacion = await _usuarioEvaluacionRepository.InsertOrUpdate(usuarioEvaluacion);
                    if (usuarioEvaluacion.UsuarioAreas.Count>0) {
                        List<UsuarioArea> usuarioAreasNew = new List<UsuarioArea>();
                        foreach (UsuarioArea usuarioArea in usuarioEvaluacion.UsuarioAreas)
                        {
                            UsuarioArea salidaUsuarioArea = await _usuarioAreaRepository.InsertOrUpdate(usuarioArea);
                            usuarioAreasNew.Add(salidaUsuarioArea);
                        }
                        salidaUsuarioEvaluacion.UsuarioAreas = usuarioAreasNew;
                    }
                    usuarioEvaluacionsNew.Add(salidaUsuarioEvaluacion);
                }
                retorno.UsuarioEvaluacions = usuarioEvaluacionsNew;
            }

            if (retorno.UsuarioSuscripcions.Count > 0)
            {
                List<UsuarioSuscripcion> UsuarioSuscripciones= new List<UsuarioSuscripcion>();

                foreach (UsuarioSuscripcion usuarioSuscripcion in retorno.UsuarioSuscripcions)
                    UsuarioSuscripciones.Add(await _usuarioSuscripcionRepository.InsertOrUpdate(usuarioSuscripcion));

                retorno.UsuarioSuscripcions = UsuarioSuscripciones;
            }


            return _mapper.Map<UsuarioModel>(retorno);
        }

        public async Task<UsuarioModel> UpdateUser(UsuarioModel usuario)
        {
            if (string.IsNullOrEmpty(usuario.Nombres)) throw new ArgumentNullException("Nombre");
            //if (string.IsNullOrEmpty(usuario.Password)) throw new ArgumentNullException("Contraseña");
            if (string.IsNullOrEmpty(usuario.Email)) throw new ArgumentNullException("Email");
            //if (string.IsNullOrEmpty(usuario.Telefono)) throw new ArgumentNullException("Telefono");
            //if (string.IsNullOrEmpty(usuario.Empresa.RazonSocial)) throw new ArgumentNullException("Razón social");
            // if (string.IsNullOrEmpty(usuario.Empresa.RutEmpresa)) throw new ArgumentNullException("Rut empresa");

            if (!string.IsNullOrEmpty(usuario.Password))
            {
                usuario.Password = _securityHelper.EncryptPassword(usuario.Password);
            }
            else
            {
                var usuarioInfo = await _usuarioRepository.GetById(usuario.Id);
                usuario.Password = usuarioInfo.Password;
            }

            if (usuario.PerfilId.Equals(Guid.Empty))
            {
                var perfil = await _perfilRepository.GetPerfilByName("Usuario pro (empresa)");
                usuario.PerfilId = perfil.Id;
            }

          
            Perfil perfilModel = new Perfil() { 
                Id = usuario.PerfilId,
            };
            var perfilUsuario = await _perfilRepository.GetPerfilById(perfilModel);

            var retorno = await _usuarioRepository.InsertOrUpdate(_mapper.Map<Usuario>(usuario));
            if (perfilUsuario.Nombre == "Usuario pro (empresa)") { 
               Empresa empresa = await _empresaRepository.InsertOrUpdate(_mapper.Map<Empresa>(usuario.Empresa));
            }
            if (retorno.UsuarioEmpresas.Count > 0)
            {
                List<UsuarioEmpresa> UsuarioEmpresasNew = new List<UsuarioEmpresa>();

                await _usuarioEmpresaRepository.DeleteByUsuarioId(_mapper.Map<Usuario>(usuario));

                foreach (UsuarioEmpresa usuarioEmpresa in retorno.UsuarioEmpresas)
                    UsuarioEmpresasNew.Add(await _usuarioEmpresaRepository.InsertOrUpdate(usuarioEmpresa));

                retorno.UsuarioEmpresas = UsuarioEmpresasNew;
            }
            if (retorno.UsuarioEvaluacions.Count > 0)
            {
                List<UsuarioEvaluacion> usuarioEvaluacionsNew = new List<UsuarioEvaluacion>();

                foreach (UsuarioEvaluacion usuarioEvaluacion in retorno.UsuarioEvaluacions)
                {
                    UsuarioEvaluacion salidaUsuarioEvaluacion = await _usuarioEvaluacionRepository.InsertOrUpdate(usuarioEvaluacion);
                    if (usuarioEvaluacion.UsuarioAreas.Count > 0)
                    {
                        List<UsuarioArea> usuarioAreasNew = new List<UsuarioArea>();
                        foreach (UsuarioArea usuarioArea in usuarioEvaluacion.UsuarioAreas)
                        {
                            UsuarioArea salidaUsuarioArea = await _usuarioAreaRepository.InsertOrUpdate(usuarioArea);
                            usuarioAreasNew.Add(salidaUsuarioArea);
                        }
                        salidaUsuarioEvaluacion.UsuarioAreas = usuarioAreasNew;
                    }
                    usuarioEvaluacionsNew.Add(salidaUsuarioEvaluacion);
                }
                retorno.UsuarioEvaluacions = usuarioEvaluacionsNew;
            }

            if (retorno.UsuarioSuscripcions.Count > 0)
            {
                List<UsuarioSuscripcion> UsuarioSuscripciones = new List<UsuarioSuscripcion>();

                foreach (UsuarioSuscripcion usuarioSuscripcion in retorno.UsuarioSuscripcions)
                {
                    usuarioSuscripcion.UsuarioId = retorno.Id;
                    UsuarioSuscripciones.Add(await _usuarioSuscripcionRepository.InsertOrUpdate(usuarioSuscripcion));
                }

                retorno.UsuarioSuscripcions = UsuarioSuscripciones;
            }


            return _mapper.Map<UsuarioModel>(retorno);
        }

        public async Task<UsuarioModel> Update(UsuarioModel usuario)
        {
            var retorno = await _usuarioRepository.Update(_mapper.Map<Usuario>(usuario));
            return _mapper.Map<UsuarioModel>(retorno);
        }
        public async Task<UsuarioModel> ConfirmarUsuario(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");
            var retorno = await _usuarioRepository.FindByEmail(email);
            retorno.Activo = true;
            _ = await _usuarioRepository.Update(retorno);
            return _mapper.Map<UsuarioModel>(retorno);
        }
       /* public async Task<UsuarioModel> GetUsuarioByRut(string rut)
        {
            if (string.IsNullOrEmpty(rut)) throw new ArgumentNullException("Rut");
            var retorno = await _usuarioRepository.GetUserByRut(rut);
            return _mapper.Map<UsuarioModel>(retorno);
        }*/
        public async Task<UsuarioModel> GetUsuarioByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");
            var retorno = await _usuarioRepository.FindByEmail(email);
            return _mapper.Map<UsuarioModel>(retorno);
        }
        public async Task<UsuarioModel> GetById(Guid? id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var retorno = await _usuarioRepository.GetById(id);
            return _mapper.Map<UsuarioModel>(retorno);
        }
        public async Task<List<UsuarioModel>> GetAll()
        {
            var retorno = await _usuarioRepository.GetAll();
            return _mapper.Map<List<UsuarioModel>>(retorno);
        }
        public async Task<List<UsuarioModel>> GetAllConsultor()
        {
            var retorno = await _usuarioRepository.GetAllConsultor();
            return _mapper.Map<List<UsuarioModel>>(retorno);
        }
        

        public async Task<int> DeleteCascade(UsuarioModel usuarioModel)
        {


            return await _usuarioRepository.DeleteCascade(_mapper.Map<Usuario>(usuarioModel));

        }
        public async Task<UsuarioModel> GetUsuarioByPerfilIdEmpresaId(Guid perfilId, Guid empresaId)
        {
            var retorno = await _usuarioRepository.GetUsuarioByPerfilIdEmpresaId(perfilId, empresaId);
            return _mapper.Map<UsuarioModel>(retorno);
        }
        
        public void Dispose() 
        { 
            if (_usuarioRepository != null)
            {
                _usuarioRepository.Dispose();
                _usuarioRepository = null;
            }
        }

    }

}
