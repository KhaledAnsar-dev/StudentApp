using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Student_API_Project.Services.Auth;
using StudentData.DataModels;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------- JWT Configuration ----------------

// Read JWT settings from appsettings.json (Key, Issuer, Audience)
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

// Add JWT Authentication to the service container
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,              // Check if token issuer is valid
            ValidateAudience = true,            // Check if token audience is valid
            ValidateLifetime = true,            // Check if token is not expired
            ValidateIssuerSigningKey = true,    // Check if signing key is valid
            ValidIssuer = issuer,               // Expected issuer
            ValidAudience = audience,           // Expected audience
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes) // Key used to sign token
        };
    });
// ---------------- Database (EF Core) ----------------
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDB")));

// Register custom JWT token generator service
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// ---------------- Add Controllers & Swagger ----------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();   // For minimal APIs and Swagger
builder.Services.AddSwaggerGen();             // Add Swagger for API documentation

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "ضع التوكن هنا: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ---------------- Middleware Pipeline ----------------
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI only in development mode
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();   // Force HTTPS requests

app.UseAuthentication();     // Enable JWT authentication middleware
app.UseAuthorization();      // Enable role/permission checks

app.MapControllers();        // Map controller endpoints (e.g., AuthController)

app.Run();                   // Run the application
