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

// HttpClient for FolderService: some servers / WAFs block requests without common headers
// Add a User-Agent and Accept header to reduce chance of ModSecurity blocking the request.
builder.Services.AddHttpClient<FolderService>(client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MultiTenantAPI/1.0)");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});


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