using Azure.Extensions.AspNetCore.Configuration.Secrets;
using ZL.SemanticKernelDemo.Host.Extensions;
using ZL.SemanticKernelDemo.Host.Persistence;

var builder = WebApplication.CreateBuilder(args);

// register secret client
SecretClient secretClient = new SecretClient(new Uri($"https://{builder.Configuration["Azure:KeyVault"]}.vault.azure.net"),
                                              new DefaultAzureCredential(new DefaultAzureCredentialOptions
                                              {
                                                  ExcludeEnvironmentCredential = true,
                                                  ExcludeVisualStudioCodeCredential = true,
                                                  ExcludeSharedTokenCacheCredential = true,
                                                  ExcludeInteractiveBrowserCredential = true,
                                              }));

// loads secrets into configuration. ## it requres Azure.Extensions.AspNetCore.Configuration.Secrets package
builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());


// Add services to the container.
// add entra id auth
builder.Services.ConfigureAzureEntraIDAuth(builder.Configuration);
// register semantic kernel 
builder.Services.ConfigureSemanticKernel(builder.Configuration);
// register mediatR
builder.Services.ConfigureMediatR();
// register persistence services
builder.Services.ConfigurePersistence(builder.Configuration);

// api controller
builder.Services.AddControllers();
// cors policy
builder.Services.ConfigureCors("AllowCors");
// swagger
builder.Services.ConfigureSwagger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// get the logger instance from the app
// ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
// error handling pipeline (middleware)
app.UseGlobalExceptionHandler(app.Logger);


app.UseHttpsRedirection();
// configure cors
app.UseCors("AllowCors");
// authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
