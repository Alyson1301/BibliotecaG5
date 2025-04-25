var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache(); // AÑADIR ANTES DEL BUILD
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ?? Configuración de CORS para permitir solicitudes desde el navegador
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configurar middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAllOrigins"); // ?? Usar CORS antes de los endpoints
app.UseSession(); // USAR DESPUÉS DE UseRouting Y ANTES DE los endpoints
app.UseAuthorization();

app.MapRazorPages();

app.Run();

