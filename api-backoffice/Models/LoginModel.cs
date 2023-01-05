namespace api_public_backOffice.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ResponseLoginModel
    {
        public UsuarioModel UsuarioModel { get; set; }
        public string TokenBearer { get; set; }

    }
}
