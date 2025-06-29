using Product_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;    
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
string? s=builder.Configuration["Jwt:Key"];
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("UserDb")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
options.TokenValidationParameters=new TokenValidationParameters{
ValidateIssuer=true,
ValidateAudience=true,
ValidateLifetime=true,
ValidIssuer=builder.Configuration["Jwt:Issuer"],
ValidAudience=builder.Configuration["Jwt:Audiance"],
IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
});
var app = builder.Build();
using (var scope=app.Services.CreateScope())
{
    var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();//seeds data to the application
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
