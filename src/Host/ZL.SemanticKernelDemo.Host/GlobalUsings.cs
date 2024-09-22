// semantic kernel
global using Microsoft.SemanticKernel.ChatCompletion;
global using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
global using Microsoft.SemanticKernel;
global using Azure.AI.OpenAI.Chat;

// identity & key vault
global using Azure.Identity;
global using Azure.Security.KeyVault.Secrets;

// web api
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;

// MediatR
global using MediatR;

// host
global using ZL.SemanticKernelDemo.Host;
global using ZL.SemanticKernelDemo.Host.Auth;
global using ZL.SemanticKernelDemo.Host.Attributes;
global using ZL.SemanticKernelDemo.Host.Models;
global using ZL.SemanticKernelDemo.Host.Services.Plugins;
global using ZL.SemanticKernelDemo.Host.Requests;
global using ZL.SemanticKernelDemo.Host.Persistence;