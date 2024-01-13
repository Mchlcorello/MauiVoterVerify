namespace MauiPointOfSale.StateContainer
{
    public enum WorkflowState
    {
        Front,
        Back,
        Done
    }
    public interface ICameraStateContainer
    {
        WorkflowState WorkFlowState { get; set; }
        byte[] ImageSource { get; set; }
        string FrontDriverIdBase64 { get; set; }
        string BackDriverIdBase64 { get; set; }

        event Action HomePageStateHasChanged;
        void ConvertFrontDriverIdToBase64String();
        void ConvertBackDriverIdToBase64String();
        void InvokeStateHasChanged();
    }
    public class CameraStateContainer : ICameraStateContainer
    {
        public WorkflowState WorkFlowState { get; set; } = WorkflowState.Front;
        public byte[] ImageSource { get; set; }
        public string FrontDriverIdBase64 { get; set; } = "";
        public string BackDriverIdBase64 { get; set; } = "";
        public void InvokeStateHasChanged() => HomePageStateHasChanged?.Invoke();

        public event Action HomePageStateHasChanged;

        public void ConvertFrontDriverIdToBase64String()
        {
            if (ImageSource is not null)
                FrontDriverIdBase64 = $"data:image/jpeg;base64," + Convert.ToBase64String(ImageSource);
        }
        public void ConvertBackDriverIdToBase64String()
        {
            if (ImageSource is not null)
                BackDriverIdBase64 = $"data:image/jpeg;base64," + Convert.ToBase64String(ImageSource);
        }
    }
}
