var builder = WebApplication.CreateBuilder(args);

// CORS politikas� ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()   // Herhangi bir kayna�a izin ver
               .AllowAnyMethod()   // Herhangi bir HTTP metoduna izin ver
               .AllowAnyHeader();  // Herhangi bir ba�l��a izin ver
    });
});

builder.Services.AddControllers(); // MVC Controller hizmetlerini ekle
builder.Services.AddEndpointsApiExplorer(); // API u� noktalar�n� ke�fetmek i�in hizmet ekle
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BlogManagerAPI",
        Version = "v1"
    });
    // Metotlara g�re s�ralama: POST (1), GET (2), PUT (3), DELETE (4)
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
}); // Swagger/OpenAPI belgelerini olu�turmak i�in hizmet ekle

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Geli�tirme ortam�nda Swagger'� kullan
    app.UseSwaggerUI(); // Swagger UI'yi kullan
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'ye y�nlendir
app.UseCors("AllowAll"); // CORS politikas�n� uygula
app.UseAuthorization(); // Yetkilendirme ara yaz�l�m�n� kullan
app.MapControllers(); // Controller u� noktalar�n� haritala

app.Run(); // Uygulamay� �al��t�r
