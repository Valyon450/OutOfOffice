using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using WebApi;
using WebApi.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDependency(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{    
    var xmFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmPath = Path.Combine(AppContext.BaseDirectory, xmFile);
    config.IncludeXmlComments(xmPath);
});

// Configure Authentication
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:44386/";
                    options.Audience = "NotesWebAPI";
                    options.RequireHttpsMetadata = false;
                });

builder.Services.AddVersionedApiExplorer(options =>
    options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
        ConfigureSwaggerOptions>();
builder.Services.AddApiVersioning();

builder.Services.AddHttpContextAccessor();

// Add Authorization with policies for roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireHRManagerRole", policy => policy.RequireRole("HR Manager"));
    options.AddPolicy("RequireProjectManagerRole", policy => policy.RequireRole("Project Manager"));
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            config.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
            config.RoutePrefix = string.Empty;
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseApiVersioning();

app.MapControllers();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OutOfOfficeDbContext>();
    DbInitializer.Initialize(context);
}

app.Run();
