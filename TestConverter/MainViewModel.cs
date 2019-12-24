using CoreWPF.MVVM;
using CoreWPF.Utilites;
using CoreWPF.Utilites.Navigation;

namespace TestConverter
{
    public class MainViewModel : ViewModel
    {
        private NavigationManager navigator;

        public ListExt<string> Modes { get; private set; }
        private string select_mode;
        public string Select_mode
        {
            get { return this.select_mode; }
            set
            {
                if (this.select_mode != value)
                {
                    this.select_mode = value;
                    this.navigator.Navigate(this.select_mode);
                    this.OnPropertyChanged("Select_mode");
                }
            }
        }

        public MainViewModel(NavigationManager navigator)
        {
            this.Title = "Конвертер тестов";
            this.navigator = navigator;
            this.Modes = new ListExt<string>(App.Modes);
            this.Select_mode = this.Modes.First;
        }
    }
}
