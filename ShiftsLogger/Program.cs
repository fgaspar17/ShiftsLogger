using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //});


builder.Services.AddDbContext<ShiftContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShiftContext")));

builder.Services.AddScoped<EmployeeService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRewriter(new RewriteOptions().AddRedirect("api/workers/(.*)", "api/employees/$1").AddRedirect("api/workers", "api/employees"));

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
