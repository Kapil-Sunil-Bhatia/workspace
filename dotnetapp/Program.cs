using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using dotnetapp.Data;
using dotnetapp.Models;
using dotnetapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseInMemoryDatabase("dotnetappDb"));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://8081-afdafeec333599263cdbaeceone.premiumproject.examly.io/")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Services
builder.Services.AddScoped<IContestService, ContestService>();
builder.Services.AddScoped<IScoreService, ScoreService>();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    context.Database.EnsureCreated();
    
    // Seed admin user
    if (!userManager.Users.Any(u => u.IsAdmin))
    {
        var admin = new ApplicationUser
        {
            UserName = "admin@test.com",
            Email = "admin@test.com",
            IsAdmin = true,
            EmailConfirmed = true
        };
        
        userManager.CreateAsync(admin, "Admin@123").Wait();
    }
    
        // Seed MatchPlayers for existing match and players
    if (!context.MatchPlayers.Any())
    {
        if (context.Matches.Any())
        {
            var match = context.Matches.First();
            var players = context.Players.ToList();

            var matchPlayers = players.Select(p => new MatchPlayer
            {
                MatchId = match.MatchId,
                PlayerId = p.PlayerId,
                Score = 0
            }).ToList();

            context.MatchPlayers.AddRange(matchPlayers);
            context.SaveChanges();
        }
    }
}

app.Run();
