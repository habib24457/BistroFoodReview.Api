using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Helpers;
using BistroFoodReview.Api.Mappings;
using BistroFoodReview.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfiles>();
});
builder.Services.AddDbContext<BistroReviewDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMealRepository, MealRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

var app = builder.Build();
app.UseCors("AllowFrontend");

/*Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BistroReviewDbContext>();
    DbSeeder.InitializeSeeding(context);
}*/

/*Delete seeded data*/
/*
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BistroReviewDbContext>();

context.Ratings.RemoveRange(context.Ratings);
context.Meals.RemoveRange(context.Meals);
context.MealOptions.RemoveRange(context.MealOptions);
context.Users.RemoveRange(context.Users);
context.SaveChanges();*/


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();