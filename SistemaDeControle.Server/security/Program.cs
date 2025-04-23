using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using sistemadecontrole.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers
builder.Services.AddControllers();

// Registra JwtService como Singleton
builder.Services.AddSingleton<JwtService>();

// Configura autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]); // Usando a chave do arquivo de configuração
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Configura autorização com base em roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("AlunoOnly", policy => policy.RequireRole("ALUNO"));
    options.AddPolicy("ProfessorOnly", policy => policy.RequireRole("PROFESSOR"));
});

var app = builder.Build();

// Middlewares
app.UseRouting();
app.UseAuthentication(); // obrigatório para usar [Authorize]
app.UseMiddleware<JwtAuthMiddleware>(); // middleware customizado que seta o HttpContext.User
app.UseAuthorization();

app.MapControllers();

app.Run();
