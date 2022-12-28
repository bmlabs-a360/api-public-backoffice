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
    public interface IPerfilPermisoService
    {
        Task<PerfilPermisoModel> GetPerfilPermisoById(PerfilPermisoModel PerfilPermisoModel);
        Task<PerfilPermisoModel> InsertOrUpdate(PerfilPermisoModel PerfilPermisoModel);
        Task<List<PerfilPermisoModel>> GetPerfilPermisos();
        Task<List<PerfilPermisoModel>> GetGetPerfilPermisosByPerfilId(PerfilModel perfilModel);

       void Dispose();
    }
    public class PerfilPermisoService : IPerfilPermisoService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IPerfilPermisoRepository _PerfilPermisoRepository;
        private ISecurityHelper _securityHelper;

        public PerfilPermisoService(IMapper mapper, IMemoryCache memoryCache, PerfilPermisoRepository PerfilPermisoRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _PerfilPermisoRepository = PerfilPermisoRepository;
            _securityHelper = securityHelper;
        }
        public async Task<PerfilPermisoModel> GetPerfilPermisoById(PerfilPermisoModel PerfilPermisoModel)
        {
            if (string.IsNullOrEmpty(PerfilPermisoModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPerfilPermiso = await _PerfilPermisoRepository.GetPerfilPermisoById(_mapper.Map<PerfilPermiso>(PerfilPermisoModel));
            return _mapper.Map<PerfilPermisoModel>(miPerfilPermiso);
        }

        public async Task<List<PerfilPermisoModel>> GetPerfilPermisos()
        {
            var PerfilPermisosList = await _PerfilPermisoRepository.GetPerfilPermisos();
            return _mapper.Map<List<PerfilPermisoModel>>(PerfilPermisosList);
        }
        public async Task<PerfilPermisoModel> InsertOrUpdate(PerfilPermisoModel PerfilPermisoModel)
        {
            if (string.IsNullOrEmpty(PerfilPermisoModel.PerfilId.ToString())) throw new ArgumentNullException("Debe indicar PerfilId");
            if (string.IsNullOrEmpty(PerfilPermisoModel.Nombre)) throw new ArgumentNullException("Debe indicar nombre");
            if (string.IsNullOrEmpty(PerfilPermisoModel.Detalle)) throw new ArgumentNullException("Debe indicar Detalle");
            if (string.IsNullOrEmpty(PerfilPermisoModel.Activo.ToString())) throw new ArgumentNullException("Debe indicar Activo");

            var retorno = await _PerfilPermisoRepository.InsertOrUpdate(_mapper.Map<PerfilPermiso>(PerfilPermisoModel));
            return _mapper.Map<PerfilPermisoModel>(retorno);
        }


        public async Task<List<PerfilPermisoModel>>  GetGetPerfilPermisosByPerfilId(PerfilModel perfilModel)
        {

            if (string.IsNullOrEmpty(perfilModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Id del perfil");
            var PerfilPermisosList = await _PerfilPermisoRepository.GetGetPerfilPermisosByPerfilId(_mapper.Map<Perfil>(perfilModel));
            return _mapper.Map<List<PerfilPermisoModel>>(PerfilPermisosList);
        }



        public void Dispose() 
        { 
            if (_PerfilPermisoRepository != null)
            {
                _PerfilPermisoRepository.Dispose();
                _PerfilPermisoRepository = null;
            }
        }

    }

}
