using BookStore.Api.Common.Behaviors;
using BookStore.Api.Common.Extensions;
using BookStore.Api.Data;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

builder.Services.AddDbContext<BookStoreContext>(
        options =>
            options.UseInMemoryDatabase("BookStoreDb")
                .UseSeeding(BookStoreContext.Seed))
    .AddEntityFrameworkInMemoryDatabase();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.RegisterEndpoints();

app.Run();
