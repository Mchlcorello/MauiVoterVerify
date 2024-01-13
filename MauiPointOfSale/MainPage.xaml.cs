using MauiPointOfSale.StateContainer;

namespace MauiPointOfSale
{
    public partial class MainPage : ContentPage
    {
        public ICameraStateContainer Camera { get; set; }
        public MainPage(ICameraStateContainer cam)
        {
            Camera = cam;
            InitializeComponent();
        }
    }
}
