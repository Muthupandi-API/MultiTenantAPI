using MultiTenantAPI.Services;
using MultiTenantAPI.Middleware;
using MultiTenantAPI.Repository;


var builder = WebApplication.CreateBuilder(args);


// 📌 Add Controllers
builder.Services.AddControllers();


// 📌 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 📌 Tenant Service
builder.Services.AddScoped<ITenantService, TenantService>();


// 📌 Tenant Repository
builder.Services.AddScoped<ITenantRepository, TenantRepository>();


// 📌 CORS (Angular connect)
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



// 📌 Swagger UI
app.UseSwagger();
app.UseSwaggerUI();



// 📌 HTTPS
app.UseHttpsRedirection();



// 📌 CORS
app.UseCors("AllowAll");



// 📌 Tenant Middleware
// Must be before Controller execution
app.UseMiddleware<TenantMiddleware>();



// 📌 Authorization
app.UseAuthorization();



app.MapControllers();



app.Run();