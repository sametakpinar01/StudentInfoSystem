var builder = WebApplication.CreateBuilder(args);

// CORS politikasý ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()   // Herhangi bir kaynaða izin ver
               .AllowAnyMethod()   // Herhangi bir HTTP metoduna izin ver
               .AllowAnyHeader();  // Herhangi bir baþlýða izin ver
    });
});

builder.Services.AddControllers(); // MVC Controller hizmetlerini ekle
builder.Services.AddEndpointsApiExplorer(); // API uç noktalarýný keþfetmek için hizmet ekle
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BlogManagerAPI",
        Version = "v1"
    });
    // Metotlara göre sýralama: POST (1), GET (2), PUT (3), DELETE (4)
    options.OrderActionsBy(apiDesc =>
    {
        var method = apiDesc.HttpMethod?.ToLowerInvariant();

        return method switch
        {
            "post" => "1",   // Create
            "get" => "2",    // Read
            "put" => "3",    // Update
            "delete" => "4", // Delete
            _ => "99"
        };
    });
}); // Swagger/OpenAPI belgelerini oluþturmak için hizmet ekle

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Geliþtirme ortamýnda Swagger'ý kullan
    app.UseSwaggerUI(); // Swagger UI'yi kullan
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'ye yönlendir
app.UseCors("AllowAll"); // CORS politikasýný uygula
app.UseAuthorization(); // Yetkilendirme ara yazýlýmýný kullan
app.MapControllers(); // Controller uç noktalarýný haritala

app.Run(); // Uygulamayý çalýþtýr
