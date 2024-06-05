using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using VetAuto;
using VetAuto.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddLocalization(config =>
{
    config.ResourcesPath = "Resources";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(
    typeof(DependencyInjector), 
    typeof(BusinessLogicLayer.DependencyInjector),
    typeof(DataAccessLayer.DependencyInjector),
    typeof(Core.DependencyInjector)
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        ValidIssuer = builder.Configuration["JwtConfiguration:Issuer"],
        ValidAudience = builder.Configuration["JwtConfiguration:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfiguration:SecretKey"]
            ?? throw new ArgumentNullException("SecretKey")
        ))
    };
});

DependencyInjector.Inject(builder.Services, builder.Configuration);
BusinessLogicLayer.DependencyInjector.Inject(builder.Services, builder.Configuration);
DataAccessLayer.DependencyInjector.Inject(builder.Services, builder.Configuration);
Core.DependencyInjector.Inject(builder.Services, builder.Configuration);

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterAssemblyModules(typeof(BusinessLogicLayer.DependencyInjector).Assembly);
    containerBuilder.RegisterAssemblyModules(typeof(DataAccessLayer.DependencyInjector).Assembly);
    containerBuilder.RegisterAssemblyModules(typeof(Core.DependencyInjector).Assembly);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(options =>
{
    options.SupportedCultures = [
        new CultureInfo("en"),
        new CultureInfo("uk")
        ];
    options.SupportedUICultures = [
        new CultureInfo("en"),
        new CultureInfo("uk")
        ];
    options.DefaultRequestCulture = new RequestCulture("en");
    options.RequestCultureProviders = [options.RequestCultureProviders.Last()];
});

app.UseCors(cpb => cpb.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseErrorHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
