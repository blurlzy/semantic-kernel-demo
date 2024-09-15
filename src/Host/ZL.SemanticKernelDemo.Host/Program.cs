using Azure.Extensions.AspNetCore.Configuration.Secrets;

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
// register semantic kernel 
builder.Services.ConfigureSemanticKernel(builder.Configuration);
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

app.UseHttpsRedirection();
// configure cors
app.UseCors("AllowCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
