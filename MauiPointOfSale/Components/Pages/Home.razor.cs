using MauiBlazorBarCodeScanner.XamlPages;
using MauiPointOfSale.Services;
using MauiPointOfSale.StateContainer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiPointOfSale.Components.Pages
{
    public partial class Home : IDisposable
    {
        public void Dispose() => CameraContainer.HomePageStateHasChanged -= StateHasChanged;

        public IDBarcodeEndpointResponse Response { get; set; } = null;
        public List<IDBarcodeEndpointResponse> Items { get; set; } = new List<IDBarcodeEndpointResponse>();
        async Task NavigateToCamera()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new QRCamera(Nav, CameraContainer));
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                CameraContainer.HomePageStateHasChanged += StateHasChanged;
        }

        async Task VerifyData()
        {
            try
            {
                var result = await API.ExtractVoterData(new IdBarcodeRequest()
                {
                    ageLimit = 0,
                    imageSource = CameraContainer.BackDriverIdBase64,
                    inputString = "",
                }, $"https://api.microblink.com/v1/recognizers/id-barcode");
                if(result is not null)
                {
                    Response = result;
                    Items.Add(Response);
                }
            }catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string Json { get; set; } = "";

        void ShowStuff()
        {
            var j = JsonSerializer.Serialize(Response.result);
            Json = j;
        }

        string GetJson() 
        { 
            return JsonSerializer.Serialize(Response.result); 
        }

    }
}
