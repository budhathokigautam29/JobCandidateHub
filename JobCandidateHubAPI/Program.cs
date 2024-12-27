using JobCandidateHubAPI.Interfaces;
using JobCandidateHubAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);



// Register your custom services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<ISwaggerProvider, SwaggerGenerator>();
builder.Services.AddSingleton<IAsyncSwaggerProvider, SwaggerGenerator>();
builder.Services.AddTransient<IDatabaseConnectionFactory>(e =>
{
    return new SqlConnectionService(builder.Configuration.GetConnectionString("DatabaseConnection"));
});
builder.Services.AddScoped<IJobCandidate, JobCandidateService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
