using neva.entities;
using neva.Repository.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace api_public_backOffice.Repository
{
    public interface IControlTokenRepository : IRepository<ControlToken>
    {
        Task<bool> IsValidToken(string token, DateTime fechacreacion, bool usarToken);
    }

    public class ControlTokenRepository : Repository<ControlToken, Context>, IControlTokenRepository
    {
        public ControlTokenRepository(Context context) : base(context) { }

        public async Task<bool> IsValidToken(string token, DateTime fechacreacion, bool usarToken)
        {

            DateTime inicio = new DateTime(fechacreacion.Year, fechacreacion.Month, 1);
            DateTime fin = inicio.AddMonths(1).AddDays(-1);

            bool isValid = true;
            var controlToken = await Context().ControlTokens
                                               .Where(x => x.Token.Equals(token)
                                                   && x.Activo != true
                                                   && x.FechaCreacion >= inicio
                                                   && x.FechaCreacion <= fin)
                                               .FirstAsync();

            if (controlToken == null) isValid = false;

            if (usarToken && isValid)
            {
                controlToken.Activo = false;
                await Context().SaveChangesAsync();
            }

            return isValid;
        }

    }
}