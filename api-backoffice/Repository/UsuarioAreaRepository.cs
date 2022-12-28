using api_public_backOffice.Models;
using neva.entities;
using neva.Repository.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api_public_backOffice.Repository
{
    public interface IUsuarioAreaRepository : IRepository<UsuarioArea>
    {
        Task<UsuarioArea> GetUsuarioAreaById(UsuarioArea UsuarioArea);
        Task<IEnumerable<UsuarioArea>> GetUsuarioAreas();
        Task<IEnumerable<UsuarioArea>> GetUsuarioAreasByUsuarioEvaluacionId(UsuarioEvaluacion usuarioEvaluacion);
        Task<IEnumerable<UsuarioArea>> GetUsuarioAreasByUsuarioSegmentacionAreaId(SegmentacionArea segmentacionArea);
    }
    public class UsuarioAreaRepository : Repository<UsuarioArea, Context>, IUsuarioAreaRepository
    {
        public UsuarioAreaRepository(Context context) : base(context) { }
        public async Task<UsuarioArea> GetUsuarioAreaById(UsuarioArea UsuarioArea)
        {
            if (string.IsNullOrEmpty(UsuarioArea.Id.ToString())) throw new ArgumentNullException("UsuarioAreaId");
            var retorno = await Context()
                            .UsuarioAreas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == UsuarioArea.Id  && x.Activo.Value);

            if (retorno == null) return null;
            return retorno; 
        }
        public async Task<IEnumerable<UsuarioArea>> GetUsuarioAreas()
        {
            var retorno = await  Context()
                            .UsuarioAreas
                            .AsNoTracking().Where(x => x.Activo.Value).ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<UsuarioArea>> GetUsuarioAreasByUsuarioEvaluacionId(UsuarioEvaluacion usuarioEvaluacion)
        {
            var retorno = await Context()
                            .UsuarioAreas.Where(y => y.UsuarioEvaluacionId == usuarioEvaluacion.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
        public async Task<IEnumerable<UsuarioArea>> GetUsuarioAreasByUsuarioSegmentacionAreaId(SegmentacionArea segmentacionArea)
        {
            var retorno = await Context()
                            .UsuarioAreas.Where(y => y.SegmentacionAreaId == segmentacionArea.Id && y.Activo.Value).AsNoTracking().ToListAsync();

            if (retorno == null) return null;
            return retorno;
        }
    }
}
