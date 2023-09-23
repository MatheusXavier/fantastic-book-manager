using Identity.API.Infrastructure.Extensions;

using NetDevPack.Identity.Data;
using NetDevPack.Identity.User;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);

// Add swagger documentation
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

// Configure ASP.NET Identity
builder.Services
    .AddIdentityInfrastructure(builder.Configuration)
    .AddAspNetUserConfiguration();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthConfiguration();

app.MapControllers();

app.MigrateDbContext<NetDevPackAppDbContext>((_, __) => { });

await app.RunAsync();
