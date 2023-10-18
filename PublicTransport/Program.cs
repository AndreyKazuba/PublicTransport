using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Data;
using PublicTransport.Data.Entities;
using PublicTransport.Helpers;
using PublicTransport.Models;

namespace PublicTransport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PublicTransportDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/api/login");
            builder.Services.AddAuthorization();

            WebApplication app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            ConfigurePrivateUrls(app);
            app.UseDefaultFiles();
            app.UseStaticFiles();
            ConfigureWebApi(app);

            app.Run();
        }

        private static void ConfigurePrivateUrls(WebApplication app)
        {
            List<string> privateUrls = new List<string> { "/", "/index.html", "/ticket/ticket.html", "/buy/buy.html", "/balance/balance.html" };
            app.Use(async (httpContext, next) =>
            {
                string? path = httpContext.Request.Path.Value;
                if (privateUrls.Any(url => url == path))
                {
                    if (httpContext.User.Identity?.IsAuthenticated != true)
                    {
                        httpContext.Response.Redirect("/login/login.html");
                        return;
                    }
                }

                await next.Invoke();
            });
        }

        private static void ConfigureWebApi(WebApplication app)
        {
            app.MapPost("/api/register", async ([FromBody] Register registerModel, PublicTransportDbContext dbContext, HttpContext httpContext) =>
            {
                if (string.IsNullOrWhiteSpace(registerModel.PassportNumber)
                 || string.IsNullOrWhiteSpace(registerModel.Password)
                 || string.IsNullOrWhiteSpace(registerModel.PassportId)
                 || string.IsNullOrWhiteSpace(registerModel.FirstName)
                 || string.IsNullOrWhiteSpace(registerModel.LastName))
                {
                    return Results.BadRequest();
                }

                bool userExist = await dbContext.Users
                    .AnyAsync(user => user.PassportNumber == registerModel.PassportNumber && user.PasspostId == registerModel.PassportId);

                if (userExist)
                    return Results.Conflict();

                User user = dbContext.Users.Add(new User
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    PassportNumber = registerModel.PassportNumber,
                    PasspostId = registerModel.PassportId,
                    Password = PasswordHasher.GetHashString(registerModel.Password),
                    Balance = 0
                }).Entity;

                await dbContext.SaveChangesAsync();
                List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Results.Ok();
            });

            app.MapPost("/api/login", async ([FromBody] Login loginModel, PublicTransportDbContext dbContext, HttpContext httpContext) =>
            {
                User? user = await dbContext.Users.FirstOrDefaultAsync(currentUser => currentUser.PasspostId == loginModel.PassportId);

                if (user is null)
                    return Results.Unauthorized();

                string outerPasswordHash = PasswordHasher.GetHashString(loginModel.Password);
                if (user.Password != outerPasswordHash)
                    return Results.Unauthorized();

                List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Results.Ok();
            });

            app.MapPost("/api/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok();
            });

            app.MapGet("/api/tickets", [Authorize] async (PublicTransportDbContext dbContext, HttpContext httpContext) =>
            {
                Claim claim = httpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
                Guid userId = Guid.Parse(claim.Value);

                List<Ticket> tickets = await dbContext.Tickets
                    .Where(ticket => ticket.OwnerId == userId)
                    .ToListAsync();

                return Results.Ok(tickets);
            });

            app.MapPost("/api/ticket", [Authorize] async ([FromBody] Guid ticketId, PublicTransportDbContext dbContext) =>
            {
                Ticket ticket = await dbContext.Tickets.FirstAsync(ticket => ticket.Id == ticketId);
                return Results.Ok(ticket);
            });

            app.MapPost("/api/ticket/use", [Authorize] async ([FromBody] Guid ticketId, PublicTransportDbContext dbContext) =>
            {
                Ticket ticket = await dbContext.Tickets.FirstAsync(ticket => ticket.Id == ticketId);
                dbContext.Tickets.Remove(ticket);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapGet("api/currentUser", [Authorize] async (PublicTransportDbContext dbContext, HttpContext httpContext) =>
            {
                Claim claim = httpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
                Guid userId = Guid.Parse(claim.Value);

                User currentUser = await dbContext.Users.FirstAsync(user => user.Id == userId);

                return Results.Ok(currentUser);
            });

            app.MapPost("/api/increaseBalance", [Authorize] async ([FromBody] int balanceIncrease, PublicTransportDbContext dbContext, HttpContext httpContext) =>
            {
                Claim claim = httpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
                Guid userId = Guid.Parse(claim.Value);

                User currentUser = await dbContext.Users.FirstAsync(user => user.Id == userId);
                currentUser.Balance += balanceIncrease;
                await dbContext.SaveChangesAsync();

                return Results.Ok(currentUser);
            });

            app.MapGet("api/transports", [Authorize] async (PublicTransportDbContext dbContext) =>
            {
                List<Transport> transports = await dbContext.Transports.ToListAsync();
                return Results.Ok(transports);
            });

            app.MapPost("api/buyTicket", [Authorize] async ([FromBody] int transportId, PublicTransportDbContext dbContext, HttpContext httpContext) =>
            {
                Claim claim = httpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
                Guid userId = Guid.Parse(claim.Value);

                Transport transport = await dbContext.Transports.FirstAsync(currentTransport => currentTransport.Id == transportId);
                User user = await dbContext.Users.FirstAsync(user => user.Id == userId);

                if (user.Balance == 0)
                    return Results.StatusCode(402);

                user.Balance -= 2;

                dbContext.Tickets.Add(new Ticket
                {
                    OwnerId = userId,
                    Price = 2,
                    TransportNumber = transport.Number,
                    TransportType = transport.TransportType,
                });

                await dbContext.SaveChangesAsync();

                return Results.Ok();
            });
        }
    }
}