using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Net;

namespace PrograAvanBiblioteca.Pages
{
    public class reseteoClaveModel : PageModel
    {
        [BindProperty]
        public string Correo { get; set; }

        public string Mensaje { get; set; }
        public bool Exito { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Correo))
            {
                Mensaje = "El correo es requerido.";
                Exito = false;
                return Page();
            }

            var token = Guid.NewGuid().ToString();

            try
            {
                var resetLink = Url.Page("/nuevaClave", null, new { token = token, email = Correo }, Request.Scheme);
                var subject = "Restablecimiento de Contraseña - Biblioteca Aura de Sabiduria";

                var body = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f8f9fa;
            color: #343a40;
        }}
        .container {{
            max-width: 600px;
            margin: auto;
            background: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #007bff;
        }}
        .button {{
            display: inline-block;
            margin-top: 20px;
            padding: 12px 24px;
            font-size: 16px;
            background-color: #007bff;
            color: white;
            text-decoration: none;
            border-radius: 6px;
        }}
        p {{
            line-height: 1.6;
        }}
        .footer {{
            font-size: 12px;
            color: #888;
            margin-top: 40px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Solicitud de Restablecimiento de Contraseña</h2>
        <p>Hola,</p>
        <p>Hemos recibido una solicitud para restablecer la contraseña de tu cuenta registrada en la <strong>Biblioteca Aura de Sabiduria</strong>.</p>
        <p>Para continuar con el proceso y establecer una nueva contraseña, por favor haz clic en el botón de abajo:</p>
        <p><a href='{resetLink}' class='button'>Restablecer Contraseña</a></p>
        <p>Si no solicitaste este restablecimiento, ignora este mensaje. Tu cuenta seguirá siendo segura.</p>
        <p>Si tienes alguna inquietud o no reconoces esta acción, te recomendamos contactar de inmediato al administrador del sistema.</p>
        <div class='footer'>
            © {DateTime.Now.Year} Biblioteca Aura de Sabiduria. Todos los derechos reservados.
        </div>
    </div>
</body>
</html>";

                using var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("bricenoc506@gmail.com", "lxte nsud zhch yrmc"),
                    EnableSsl = true
                };

                var message = new MailMessage("bricenoc506@gmail.com", Correo, subject, body)
                {
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);

                Mensaje = "Se ha enviado un enlace de restablecimiento a tu correo.";
                Exito = true;
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al enviar el correo. Intenta más tarde. Detalles: {ex.Message}";
                Exito = false;
            }

            return Page();
        }
    }
}
