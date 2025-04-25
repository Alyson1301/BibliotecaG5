namespace ApiProyectoFinal.Models
{
    public class ResetPasswordRequest
    {
        public string Correo { get; set; }
        public string Token { get; set; }
        public string NuevaContraseña { get; set; }
    }
}
