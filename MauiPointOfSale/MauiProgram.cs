using Camera.MAUI;
using CommunityToolkit.Maui;
using MauiPointOfSale.Services;
using MauiPointOfSale.StateContainer;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.Text;

namespace MauiPointOfSale
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            string username = "";
            string password = "";

            string apikey = "";
            string apisecret = "";
            
            string verifyCredentials = $"{username}:{password}";
            string cloudCredentials = $"{apikey}:{apisecret}";
            
            string encodedVerifyCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(verifyCredentials));
            string encodedCloudCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(cloudCredentials));

            builder.Services.AddHttpClient("MicroBlinkCloud", x =>
            {
                x.BaseAddress = new Uri("https://api.microblink.com.com/api/v1/docver");
                x.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");
            });
            builder.Services.AddHttpClient("MicroBlinkVerify", x =>
            {
                x.BaseAddress = new Uri("https://usc1.verify.microblink.com/api/v1/docver");
                x.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "");
            });
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<IdentityService>();
            builder.Services.AddSingleton<ICameraStateContainer, CameraStateContainer>();
#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
