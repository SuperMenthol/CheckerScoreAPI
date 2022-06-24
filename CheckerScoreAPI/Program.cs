using Domain.Data;
using Domain.Data.Abstracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddScoped<IDataContext, DataContext>();
builder.Services.Configure<CheckerScoreDatabaseSettings>(builder.Configuration.GetSection("CheckerScoreDatabase"));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
