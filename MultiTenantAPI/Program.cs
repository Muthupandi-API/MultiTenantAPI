

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