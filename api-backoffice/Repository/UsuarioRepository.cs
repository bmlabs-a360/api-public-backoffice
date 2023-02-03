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

namespace api_public_backOffice.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> GetUser(LoginModel loginModel);
       // Task<Usuario> GetUserByRut(string rut);
        Task<Usuario> FindByEmail(string email);
        Task<Usuario> GetById(int? id);
        Task<List<Usuario>> GetAll();
        Task<int> DeleteCascade(Usuario usuario);
    }
    /*(¯`·._.··¸.-~*´¨¯¨`*·~-.,-(IMPLEMENTACION)-,.-~*´¨¯¨`*·~-.¸··._.·´¯)*/
    public class UsuarioRepository : Repository<Usuario, Context>, IUsuarioRepository
    {
        public UsuarioRepository(Context context) : base(context) { }
    
        public async Task<Usuario> GetUser(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Email)) throw new ArgumentNullException("Email");
            if (string.IsNullOrEmpty(loginModel.Password)) throw new ArgumentNullException("Password");
            //MD5 md5Hash = MD5.Create();
            var retorno = await Context()
                            .Usuarios
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email.Equals(loginModel.Email) && x.Password.Equals(loginModel.Password) && x.Activo.Value);

            if (retorno == null) return null;
            await maperPerfil(retorno);
            return retorno; 
        }

        private async Task maperPerfil(Usuario retorno)
        {
            retorno.Perfil = await Context()
                    .Perfils
                    .FirstOrDefaultAsync(x => x.Id == retorno.PerfilId && x.Activo.Value);

           /* retorno.Perfil.PerfilRols = await Context()
                            .PerfilRols
                            .AsNoTracking()
                            .Where(x => x.PerfilId == retorno.PerfilId).ToListAsync();

            foreach (PerfilRol rol in retorno.Perfil.PerfilRols)
            {
                rol.PerfilPermisos = await Context()
                    .PerfilPermisos
                    .AsNoTracking()
                    .Where(x => x.PerfilRolId == rol.Id).ToListAsync();
            }*/
        }
        private async Task<List<Usuario>> maperUserSingle(List<Usuario> retorno)
        {

            foreach (var re in retorno)
            {


                re.Perfil = await Context()
                        .Perfils
                        .FirstOrDefaultAsync(x => x.Id == re.PerfilId && x.Activo.Value);
            }
            return retorno;
            /*retorno.Perfil.PerfilRols = await Context()
                            .PerfilRols
                            .AsNoTracking()
                            .Where(x => x.PerfilId == retorno.PerfilId).ToListAsync();

            foreach (PerfilRol rol in retorno.Perfil.PerfilRols)
            {
                rol.PerfilPermisos = await Context()
                    .PerfilPermisos
                    .AsNoTracking()
                    .Where(x => x.PerfilRolId == rol.Id).ToListAsync();
            }*/
        }

       /* public async Task<Usuario> GetUserByRut(string rut)
        {
            if (string.IsNullOrEmpty(rut)) throw new ArgumentNullException("Rut");
            var retorno = await Context()
                            .Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Rut.Equals(rut));
            return retorno;
        }*/

        public async Task<Usuario> FindByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");
            var retorno = await Context()
                            .Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email) && x.Activo.Value);
            return retorno;
        }

        public async Task<Usuario> GetById(int? id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var retorno = await Context()
                                .Usuarios
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id.Equals(id) && x.Activo.Value);
            await maperPerfil(retorno);
            return retorno;
        }
        public async Task<List<Usuario>> GetAll()
        {
            var retorno = await Context()
                                .Usuarios
                                .Include(x => x.UsuarioEmpresas)
                                .Include(x => x.UsuarioEvaluacions)
                                //.Include(x => x.UsuarioSuscripcions)
                                .OrderBy(x => x.Nombres)
                               // .Where(x =>  x.Activo.Value)
                                .ToListAsync();

            return await maperUsuarioAreas(retorno);
        }
        private async Task<List<Usuario>> maperUsuarioAreas(List<Usuario> retorno)
        {
            foreach (var re in retorno)
            {
                foreach (var item in re.UsuarioEvaluacions)
                {
                    item.UsuarioAreas = await Context()
                        .UsuarioAreas
                        .Where(x => x.UsuarioEvaluacionId == item.Id && x.Activo.Value).ToArrayAsync();
                }
            }
            return retorno;
        }

        public async Task<int> DeleteCascade(Usuario usuario)
        {
            try
            {
                var usuariosEvaluacion =  Context().UsuarioEvaluacions.Where(x => x.UsuarioId == usuario.Id).ToList();

                foreach (UsuarioEvaluacion item in usuariosEvaluacion)
                    await Context().UsuarioAreas.Where(x => x.UsuarioEvaluacionId == item.Id).DeleteFromQueryAsync();

                await Context().UsuarioEvaluacions.Where(x => x.UsuarioId == usuario.Id).DeleteFromQueryAsync();
                await Context().UsuarioSuscripcions.Where(x => x.UsuarioId == usuario.Id).DeleteFromQueryAsync();
                await Context().UsuarioEmpresas.Where(x => x.UsuarioId == usuario.Id).DeleteFromQueryAsync();
                return await Context().Usuarios.Where(x => x.Id == usuario.Id).DeleteFromQueryAsync();

            }
            catch (Exception ex)
            {

                throw new Exception( ex.Message);
            }
            
        }

    }
}
