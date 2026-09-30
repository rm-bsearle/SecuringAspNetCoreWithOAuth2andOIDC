using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ImageGallery.Client.Controllers;

public class AuthenticationController(
    IHttpClientFactory httpClientFactory
) : Controller
{
    [Authorize]
    public async Task Logout()
    {
        var client = httpClientFactory.CreateClient("IDPClient");

        var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync();
        if (discoveryDocumentResponse.IsError)
        {
            throw new Exception(discoveryDocumentResponse.Error);
        }

        var accessTokenRevocationReponse = await client.RevokeTokenAsync(new()
        {
            Address = discoveryDocumentResponse.RevocationEndpoint,
            ClientId = "imagegalleryclient",
            ClientSecret = "secret",
            Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken),
        });
        
        var refreshTokenRevocationReponse = await client.RevokeTokenAsync(new()
        {
            Address = discoveryDocumentResponse.RevocationEndpoint,
            ClientId = "imagegalleryclient",
            ClientSecret = "secret",
            Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken),
        });
        
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        
        await HttpContext.SignOutAsync(
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}