using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using SampleWebApi.Swagger;
using SampleWebApi;
using SampleWebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opts =>
{
    opts.SchemaFilter<EnumSchemaFilter>();
    
    // These just add random extensions to the swagger document for testing.
    opts.SchemaFilter<ExtensionEnumFilter>();
    opts.OperationFilter<ExtensionOperationFilter>();
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opts.IncludeXmlComments(xmlPath);
    
    opts.SchemaGeneratorOptions.SchemaIdSelector = (type) => type.FullName;
    opts.AddServer(new OpenApiServer()
    { 
        Description = "Localhost Server",
        Url = "http://localhost:5240",
    });
    opts.SwaggerDoc("v1", new() 
    {
        Title = "SampleWebApi", Version = "v1",
        Description = "This is a sample web api for testing / demo purposes for Swagabond.",
        Contact = new OpenApiContact()
        {
            Email = "test@gmail.com",
            Name = "Hugh Man",
            Url = new Uri("https://www.google.com"),
            Extensions = new Dictionary<string, IOpenApiExtension>()
            {
                { "x-contactPhoneNumber", new OpenApiString("(123)-456-7890") }
            }
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT"),
            Extensions = new Dictionary<string, IOpenApiExtension>()
            {
                { "x-licenseDescription", new OpenApiString("The MIT License allows anyone to use, modify, and distribute the software for any purpose, as long as the original license and copyright notice are included.") }
            }
        },
        TermsOfService = new Uri("https://en.wikipedia.org/wiki/Lorem_ipsum"),
        Extensions = new Dictionary<string, IOpenApiExtension>()
        {
            { "x-infoTwitterHandle", new OpenApiString("@madeUpTwitterHandle") }
        }
    });
});
// Dependencies 
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IFranchiseRepository, FranchiseRepository>();


builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

TestDataPopulator.AddTestData();

app.Run();
