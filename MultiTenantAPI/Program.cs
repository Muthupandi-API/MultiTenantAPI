using MultiTenantAPI.Docker;
using MultiTenantAPI.Middleware;
using MultiTenantAPI.Repository;
using MultiTenantAPI.Services;

var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//  Dependency Injection

builder.Services.AddScoped<ITenantService, TenantService>();

builder.Services.AddScoped<ITenantRepository, TenantRepository>();

builder.Services.AddScoped<IDockerService, DockerService>();


//  Plesk Service
builder.Services.AddScoped<IPleskService, PleskService>();

builder.Services.AddHttpClient<FolderService>();


// CORS

builder.Services.AddCors (options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();


// Swagger

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "MultiTenant API V1");

    c.RoutePrefix = "swagger";
});


// CORS

app.UseCors("AllowAll");


// Tenant Middleware

app.UseMiddleware<TenantMiddleware>();


// HTTPS
app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();