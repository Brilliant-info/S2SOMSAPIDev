using Microsoft.Extensions.DependencyInjection;
using S2SOMSAPI.Repository;
using S2SOMSAPI.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Register DbCommonFunctions (for ADO.NET)
builder.Services.AddScoped<CommonDBFunctionRepo>(provider =>
    new CommonDBFunctionRepo(builder.Configuration.GetConnectionString("GWC_ConnectionString")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IS2SOMSOrders, S2SOMSOrdersRepo>();
builder.Services.AddTransient<IS2SOrderHistory, S2SOrderHistoryRepo>();
builder.Services.AddTransient<IS2SOrderDriverlist, S2SOrderDriverlistRepo>();
builder.Services.AddTransient<IS2SOrderView, S2SOrderViewRepo>();
builder.Services.AddTransient<IS2SUserConfiguration, S2SUserConfigurationRepo>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
