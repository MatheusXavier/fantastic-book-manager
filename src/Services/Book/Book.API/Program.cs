using Book.API.Extensions;
using Book.Infrastructure.Database.Migrations;
using Book.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddSwagger()
    .AddAuthentication(builder.Configuration)
    .AddInfrastructureLayer()
    .AddIdentityUser()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

MigrationsManager.MigrateDb(app.Configuration);

await app.RunAsync();