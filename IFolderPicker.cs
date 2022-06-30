namespace PrCollector
{
    public interface IFolderPicker
    {
        Task<string> PickFolder();
    }
}