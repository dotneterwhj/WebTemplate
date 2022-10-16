using Abner.Api.Host;
using Abner.Api.Host.SeedWork;
using Abner.Application.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails(x =>
{
    x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
    // x.IncludeExceptionDetails
    // x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
});

//builder.Services.AddCap(options =>
//{
//    options.UseMySql(builder.Configuration["Default"]);
//});

//builder.Services.AddDbContext<BlogContext>(options =>
//{
//    var conStr = builder.Configuration["Default"];
//    options.UseMySql(conStr, ServerVersion.AutoDetect(conStr));
//});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(AutofacDependencyInjection.Init);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
// else
{
    app.UseProblemDetails();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


