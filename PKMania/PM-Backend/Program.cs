using PM_DAL.Interfaces;
using PM_DAL.Services;
using PM_BLL.Interfaces;
using PM_BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using PM_Backend.Hubs;
using PM_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
// Add services to the container.
builder.Services.AddScoped<IGainsRepository, GainsRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentsListRepository, TournamentsListRepository>();
builder.Services.AddScoped<ITournamentsTypesRepository, TournamentsTypesRepository>();

builder.Services.AddScoped<ITournamentsManagerService, TournamentsManagerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMembersService, MembersService>();
builder.Services.AddScoped<ISecurityTokenService, SecurityTokenService>();
builder.Services.AddScoped<ITournamentsListService, TournamentsListService>();
builder.Services.AddScoped<ITournamentsTypesService, TournamentsTypesService>();
builder.Services.AddSingleton<PkHub>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API PokerMania", Version = "v1" });
    OpenApiSecurityScheme securitySchema = new()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement();
    securityRequirement.Add(securitySchema, new[] { "Bearer" });
    c.AddSecurityRequirement(securityRequirement);
}
);
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("https://localhost:4200");
    //builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowCredentials();
    builder.AllowAnyHeader();
}));
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<PkHub>("pkhub");
Task myTask = Task.Run(() =>
{
    //ITournamentsListRepository _tournamentsListRepository = new TournamentsListRepository();
    //IGainsRepository _gainsRepository = new GainsRepository();
    //ITournamentsListService _tournamentsListService = new TournamentsListService(_tournamentsListRepository,_gainsRepository);
    PkHub pk = new PkHub();
    TournamentsManagerService tms = new TournamentsManagerService(pk);
    tms.TournamentsManager();
});
app.Run();