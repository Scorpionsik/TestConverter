namespace CoreWPF.Utilites.Navigation.Interfaces
{
    public interface INavigateModule
    {
        string Subtitle { get; }
        void OnNavigatingTo(object arg);
        void OnNavigatingFrom();
    }
}
