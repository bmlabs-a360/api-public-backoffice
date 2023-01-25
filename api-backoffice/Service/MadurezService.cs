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
    public interface IMadurezService
    {
        List<MadurezCapacidadSubAreaDto> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubAreaDto);
        List<IMSADto> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto);
        List<IMADto> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto);
        List<IMDto> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto);
        List<MadurezCapacidadSubAreaDto> GetAllCapacidadSubAreas();
        List<IMSADto> GetAllIMSA();
        List<IMADto> GetAllIMA();
        List<IMDto> GetAllIM();
        void Dispose();
    }
    public class MadurezService : IMadurezService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IMadurezRepository _madurezRepository;
        private ISecurityHelper _securityHelper;
        public MadurezService(IMapper mapper, IMemoryCache memoryCache, MadurezRepository madurezRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _madurezRepository = madurezRepository;
            _securityHelper = securityHelper;
        }
        

        public List<MadurezCapacidadSubAreaDto> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubAreaDto)
        {
            return _madurezRepository.GetCapacidadSubAreas(madurezCapacidadSubAreaDto);
        }

        public List<IMSADto> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto)
        {
            return _madurezRepository.GetIMSA(madurezCapacidadSubAreasDto);
        }
        public List<IMADto> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto)
        {
            return _madurezRepository.GetIMA(madurezCapacidadSubAreasDto);
        }
        public List<IMDto> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto)
        {
            return _madurezRepository.GetIM(madurezCapacidadSubAreasDto);
        }

        public List<MadurezCapacidadSubAreaDto> GetAllCapacidadSubAreas( )
        {
            return _madurezRepository.GetAllCapacidadSubAreas();
        }

        public List<IMSADto> GetAllIMSA( )
        {
            return _madurezRepository.GetAllIMSA();
        }
        public List<IMADto> GetAllIMA( )
        {
            return _madurezRepository.GetAllIMA();
        }
        public List<IMDto> GetAllIM( )
        {
            return _madurezRepository.GetAllIM();
        }
        public void Dispose()
        {
            if (_madurezRepository != null)
            {
                _madurezRepository.Dispose();
                _madurezRepository = null;
            }
        }

    }

}
