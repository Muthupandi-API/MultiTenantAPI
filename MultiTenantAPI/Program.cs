using Microsoft.EntityFrameworkCore;
using MultiTenantAPI.Data;
using MultiTenantAPI.Docker;
using MultiTenantAPI.Middleware;
using MultiTenantAPI.Repository;
using MultiTenantAPI.Services;

var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services.AddControllers();


// Http Context Accessor
builder.Services.AddHttpContextAccessor();


// Dynamic Tenant Connection Provider
builder.Services.AddScoped<IConnectionProvider, ConnectionProvider>();


// Dynamic DbContext
builder.Services.AddDbContext<ApplicationDbContext>(
(serviceProvider, options) =>
{
    var provider =
        serviceProvider.GetRequiredService<IConnectionProvider>();

    options.UseSqlServer(
        provider.GetConnectionString());
});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Services
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();

builder.Services.AddScoped<IDockerService, DockerService>();
builder.Services.AddScoped<IPleskService, PleskService>();


// Folder Service HttpClient
builder.Services.AddHttpClient<FolderService>(client =>
{
    client.DefaultRequestHeaders.UserAgent
        .ParseAdd("Mozilla/5.0 (compatible; MultiTenantAPI/1.0)");

    client.DefaultRequestHeaders.Accept
        .ParseAdd("application/json");

    client.Timeout = TimeSpan.FromSeconds(30);
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
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



// HTTPS
app.UseHttpsRedirection();



// CORS
app.UseCors("AllowAll");



// Multi Tenant Middleware
app.UseMiddleware<TenantMiddleware>();



// Static Files
app.UseDefaultFiles();
app.UseStaticFiles();



// Authorization
app.UseAuthorization();



// Controllers
app.MapControllers();



// Angular fallback
app.MapFallbackToFile("index.html");



app.Run();