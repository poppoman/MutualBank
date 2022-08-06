using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MutualBank.Data;
using MutualBank.Hubs;
using MutualBank.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var MutualBankconnectionString = builder.Configuration.GetConnectionString("MutualBank");
builder.Services.AddDbContext<MutualBankContext>(options =>
    options.UseSqlServer(MutualBankconnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/UserLogin/Login";
    }).AddFacebook(opt =>
    {
        opt.AppId = builder.Configuration.GetSection("OAuth:FacebookAppId").Value;
        opt.AppSecret = builder.Configuration.GetSection("OAuth:FacebookAppSecret").Value;
        opt.Events = new OAuthEvents
        {
            OnTicketReceived = ctx =>
                {
                    var db = ctx.HttpContext.RequestServices.GetRequiredService<MutualBankContext>();
                    var Name = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var Email = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var Fname = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                    var Lname = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                    var UID = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var user = db.Logins.FirstOrDefault(u => u.LoginName == Name && u.LoginEmail == Email);
                    if (user == null)
                    {
                        var newuser = new Login
                        {
                            LoginName = Name,
                            LoginPwd = Name,
                            LoginEmail = Email,
                            LoginActive = true
                        };
                        db.Logins.Add(newuser);
                        db.SaveChanges();
                        var user2 = db.Logins.Where(u => u.LoginName == Name).FirstOrDefault();
                        var newuser2 = new MutualBank.Models.User
                        {
                            UserEmail = Email,
                            UserNname = Name,
                            UserId = user2.LoginId,
                            UserFname = Fname,
                            UserLname = Lname
                        };
                        db.Users.Add(newuser2);
                        db.SaveChanges();
                        user = user2;
                    }
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, Name),
                    new Claim("UserId", user.LoginId.ToString())
                    };
                    ctx.Principal.Identities.First().AddClaims(claims);
                    return Task.CompletedTask;
                },
        };
    })
    .AddGoogle(opt => {
        opt.ClientId = builder.Configuration["OAuth:GoogleAppId"];
        opt.ClientSecret = builder.Configuration["OAuth:GoogleAppSecret"];
        opt.Events = new OAuthEvents
        {
            OnTicketReceived = ctx =>
            {
                var db = ctx.HttpContext.RequestServices.GetRequiredService<MutualBankContext>();
                var Name = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var Email = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var Fname = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var Lname = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var UID = ctx.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = db.Logins.FirstOrDefault(u => u.LoginName == Name && u.LoginEmail == Email);
                if (user == null)
                {
                    var newuser = new Login
                    {
                        LoginName = Name,
                        LoginPwd = Name,
                        LoginEmail = Email,
                        LoginActive = true
                    };
                    db.Logins.Add(newuser);
                    db.SaveChanges();
                    var user2 = db.Logins.Where(u => u.LoginName == Name).FirstOrDefault();
                    var newuser2 = new MutualBank.Models.User
                    {
                        UserEmail = Email,
                        UserNname = Name,
                        UserId = user2.LoginId,
                        UserFname = Fname,
                        UserLname = Lname
                    };
                    db.Users.Add(newuser2);
                    db.SaveChanges();
                    user = user2;
                }
                var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, Name),
                    new Claim("UserId", user.LoginId.ToString())
                    };
                ctx.Principal.Identities.First().AddClaims(claims);
                return Task.CompletedTask;
            },
        };
    });


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<MessageHub>("/messageHub");


app.Run();
