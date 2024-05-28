using back.Models;
using back.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin",
//         policyBuilder =>
//         {
//             policyBuilder.WithOrigins("http://localhost:4200") // Allowed origin
//                          .AllowAnyHeader()
//                          .AllowAnyMethod();
//         });
// });

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("MySqlDbConnection"),
            new MySqlServerVersion(new Version(8, 0, 37)))
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITaskDbService, TaskDbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.Urls.Add("http://*:80");  // Listen on port 80

// Enable CORS with the defined policy
// app.UseCors("AllowSpecificOrigin");

// app.UseAuthorization();

app.MapControllers();

app.Run();
