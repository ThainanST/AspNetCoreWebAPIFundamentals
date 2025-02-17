using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // return HTTP 406 (Not Acceptable) when the client
    // requests a response format that the API does not support
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters();

builder.Services.AddProblemDetails( options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Here is some additional information");
        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


// Make the implicit Program class public so test projects can access it
public partial class Program { }
