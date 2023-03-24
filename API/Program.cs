using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Models;
using Services;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using JWTWeb;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://example.com",
                                "http://www.contoso.com",
                                "http://localhost:4200",
                                "http://localhost:5144")
                                .AllowAnyHeader();
        });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Add services to the container.

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<ItemServices>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

// AddSingleton => The same instance is shared across the entire app over the lifetime of the application
// AddScoped => The instance is created every new request
// AddTransient => The instance is created every single time it is required as a dependency 

builder.Services.AddScoped<IRepository, DBRepository>(ctx => new DBRepository(builder.Configuration.GetConnectionString("P2DB")));
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<ItemServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("/login", ([FromBody] User user, UserServices service) => {
    return service.UserLogin(user);
});


app.MapGet("/user-inventory/userid", ([FromQuery] int userid, UserServices service) => {
    User user = new User();
    user.Id=userid;
    return service.ViewPersonalInventory(user).listOfItems;
});

app.MapGet("/hello", () => {
    return "Hello";
});

app.MapGet("/user", ([FromQuery] int userid, UserServices service) => {
    return service.GetUserByID(userid);
});

app.MapGet("/user1", ([FromQuery] string username, UserServices service) => {
    return service.GetUserByUsername(username);
});

app.MapGet("/marketplace", (UserServices service) => {
    return service.GetMarketplaceItems();
});

app.MapGet("/marketplaceByName", (string searchitem, UserServices service) => {
    return service.getMarketplaceItemsByName(searchitem);
});

app.MapPost("/users/createAccount", ([FromBody] User user, UserServices service) => {
    return Results.Created("/users/createAccount", service.CreateAccount(user));
});


app.MapPost("store/buy/", ([FromBody] Misc intarr, ItemServices service) => {
    service.buyItem(intarr);
});

app.MapPost("grabbag", ([FromBody] int num, ItemServices service) => {

    return service.buy_rand(num);

});


app.MapPost("store/sell/", ([FromBody] Sellinfo intarr, ItemServices service) => {
    Console.WriteLine("AAAAAAAAAAAA",intarr);
    service.sellItem(intarr);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();
