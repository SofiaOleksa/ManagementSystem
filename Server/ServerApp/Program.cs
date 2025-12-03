using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ASP_proj.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger (äëÿ REST API äîêóìåíòàö³¿)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core – ï³äêëþ÷åííÿ äî SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Àäì³íñüê³ êðåäåíøëè (äëÿ MVC-ëîã³íó)
builder.Services.AddSingleton(new AdminCredentials());

// MVC (êîíòðîëåðè + â'þøêè)
builder.Services.AddControllersWithViews();

// ===== JWT-àóòåíòèô³êàö³ÿ =====
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };

        // Òåïåð ï³äòðèìóºìî ÄÂÀ âàð³àíòè:
        // 1) Authorization: Bearer <token>  (äëÿ Postman / MAUI)
        // 2) cookie AuthToken                 (äëÿ MVC-àäì³íêè)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // ßêùî º çàãîëîâîê Authorization – âèêîðèñòîâóºìî éîãî
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    // JwtBearer ñàì âèòÿãíå òîêåí ³ç "Bearer xxx"
                    return Task.CompletedTask;
                }

                // ²íàêøå ïðîáóºìî âçÿòè òîêåí ³ç cookie AuthToken
                var cookieToken = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(cookieToken))
                {
                    context.Token = cookieToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Îáðîáêà ïîìèëîê + Swagger
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Àòðèáóòèâí³ ìàðøðóòè äëÿ API-êîíòðîëåð³â (ItemsController, ActionsController, ApiAuthController)
app.MapControllers();

// Ìàðøðóò äëÿ MVC (àäì³í-ïàíåëü, ëîã³í ³ CRUD)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();