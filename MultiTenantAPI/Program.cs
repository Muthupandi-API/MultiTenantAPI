//using Microsoft.EntityFrameworkCore;
//using MultiTenantAPI.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services
//builder.Services.AddControllers();

//// EF Core
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection")));

//// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//    {
//        policy.WithOrigins("http://localhost:4200")
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

//// Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();

//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MultiTenant API V1");
//        c.RoutePrefix = "swagger";
//    });
//}

//app.UseHttpsRedirection();

//app.UseCors("AllowAngular");

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using MultiTenantAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//
// 📌 Add Controllers
//
builder.Services.AddControllers();

//
// 📌 Swagger
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// 📌 Tenant Service (IMPORTANT FIX for your error)
//
builder.Services.AddScoped<TenantConfigService>();

//
// 📌 CORS (Angular connect ஆக)
//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

//
// 📌 Swagger UI
//
app.UseSwagger();
app.UseSwaggerUI();

//
// 📌 HTTPS
//
app.UseHttpsRedirection();

//
// 📌 CORS enable
//
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();