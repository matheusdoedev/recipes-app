using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Npgsql;

using DotEnv.Core;

new EnvLoader().Load();

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new ArgumentException("jwt key not defined");
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContextPool(options => {
	options.UseNp
});
builder.Services.AddNpgsql(connectionString);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "localhost",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

WebApplication? app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

await app.RunAsync();
