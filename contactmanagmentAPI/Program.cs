using contactmanagmentAPI.Middleware;
using contactmanagmentAPI.Models;
using contactmanagmentAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// Add configuration
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<Data>(builder.Configuration.GetSection("Data"));
builder.Services.AddScoped<IContactRepository, ContactRepository>();
var app = builder.Build();

app.UseMiddleware<Errorhandling>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();
