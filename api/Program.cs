
using api.data;
using api.models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);


// 注册处理器
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // 增加标准错误详情支持

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Identity Configuration
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 3;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//authentication scheme
builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultScheme =
    options.DefaultForbidScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
  }
)
    .AddJwtBearer(options =>
    {
     options.TokenValidationParameters = new TokenValidationParameters
     {
       ValidateIssuer = true,
       ValidIssuer = builder.Configuration["JWT:Issuer"],
       ValidateAudience = true,
       ValidAudience = builder.Configuration["JWT:Audience"],
       ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SiginKey"])
       ),
     };
    });

//Dependency Injection for Repository
builder.Services.AddScoped<api.Interface.IStockRepository, api.Repository.StockRepository>();
builder.Services.AddScoped<api.Interface.ICommentRepository, api.Repository.CommentRepository>();
builder.Services.AddScoped<api.Interfaces.ITokenService, api.Services.TokenServices>();
builder.Services.AddScoped<api.Repository.IPortFolioRepository, api.Repository.PortFolioRepository>();
builder.Services.AddScoped<api.Interfaces.IServices.IStockService, api.Servivces.StockServices>();
//enable swagger jwt
builder.Services.AddSwaggerGen(api.Tools.SwaggerTools.EnableSwaggerJwt());


var app = builder.Build();
// Global exception handling
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

