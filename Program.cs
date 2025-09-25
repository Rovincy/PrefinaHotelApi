using HotelWebApi;
using HotelWebApi.Helpers;
//using HotelWebApi.FrankiesKitchenModel;
using Microsoft.EntityFrameworkCore;
using System;
//using HotelWebApi.FrankiesKitchenModel;
using HotelWebApi.Helpers;
using HotelWebApi.Models;
using HotelWebApi.Helpers;
using HotelWebApi.UserModels;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var DefaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var UserDbConnection = builder.Configuration.GetConnectionString("UserDbConnection");
//builder.Services.AddDbContext<WdmsDbContext>(options => options.UseSqlServer(Wdms_Connection));

// Specify the MySQL ServerVersion for UseMySql
//builder.Services.AddDbContext<RxDBContext>(options => options.UseMySql(RxDbConnection, mySqlVersion));

builder.Services.AddDbContext<FrankiesHotelContext>(options => options.UseSqlServer(DefaultConnectionString));
//builder.Services.AddDbContext<SecondaryFrankiesKitchenContext>(options => options.UseSqlServer(SecondaryDefaultConnectionString));
//builder.Services.AddDbContext<TpaDbContext>(options => options.UseMySql(TpaConnectionString, mySqlVersion));
builder.Services.AddDbContext<UserDBContext>(options => options.UseSqlServer(UserDbConnection));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMappers).Assembly);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builderIn =>
    {
        builderIn.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }
    );
}
);
var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.MapHub<ChatHub>("/chatHub"); // Map SignalR hub

app.Run();