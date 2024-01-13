using MauiPointOfSale.StateContainer;
using Microsoft.AspNetCore.Components;
namespace MauiBlazorBarCodeScanner.XamlPages;

public partial class QRCamera : ContentPage
{
    public NavigationManager NavigationManager { get; set; }
    public ICameraStateContainer CameraContainer { get; set; }
    public QRCamera(NavigationManager nav, ICameraStateContainer cameraContainer)
    {
        InitializeComponent();
        BindingContext = cameraContainer;
        NavigationManager = nav;
        CameraContainer = cameraContainer;

        cameraView.CamerasLoaded -= CameraView_CamerasLoaded;
        cameraView.CamerasLoaded += CameraView_CamerasLoaded;
    }
    private void CameraView_CamerasLoaded(object? sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });
    }

    private async void NavigateBackToMainPage(object sender, EventArgs e)
    {
        if (ImageArr != null)
            CameraContainer.ImageSource = ImageArr.ToArray();
        switch (CameraContainer.WorkFlowState)
        {
            case WorkflowState.Front:
                CameraContainer.ConvertFrontDriverIdToBase64String();
                CameraContainer.WorkFlowState = WorkflowState.Back;
                break;
            case WorkflowState.Back:
                CameraContainer.ConvertBackDriverIdToBase64String();
                CameraContainer.WorkFlowState = WorkflowState.Done;
                break;
            case WorkflowState.Done:
                break;
            default:
                break;
        }

        await Navigation.PopModalAsync();
        NavigationManager.NavigateTo("/");
        CameraContainer.InvokeStateHasChanged();
    }

    private byte[] ImageArr;
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var image = await cameraView.TakePhotoAsync(Camera.MAUI.ImageFormat.JPEG);
        if (image is not null)
        {
            // Create a memory stream copy of the image stream
            MemoryStream memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset the position to the beginning of the stream

            // Use the memory stream for the ImageSource
            myImage.Source = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));

            // Use the original stream for conversion to byte array
            memoryStream.Position = 0; // Reset the position again before conversion
            ImageArr = await ConvertImageSourceToByteArray(memoryStream);
        }
    }

    private async Task<byte[]> ConvertImageSourceToByteArray(Stream imageStream)
    {
        byte[] imageBytes = null;
        if (imageStream != null && imageStream.CanRead)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (imageStream.CanSeek)
                    imageStream.Position = 0;
                await imageStream.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }
        }
        return imageBytes;
    }

}
