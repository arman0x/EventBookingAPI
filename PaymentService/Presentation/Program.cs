using Application.Interfaces;
using Application.Mappers;
using Application.Options;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.Configure<BookingServiceOptions>(
    builder.Configuration.GetSection("BookingService"));
builder.Services.AddTransient<InternalSecretHandler>();

builder.Services.AddControllers();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddHttpClient<IBookingApiClient, BookingApiClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<BookingServiceOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
}).AddHttpMessageHandler<InternalSecretHandler>();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => { }, typeof(PaymentProfile));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/ping", () => "pong");

app.MapControllers();

app.Run();