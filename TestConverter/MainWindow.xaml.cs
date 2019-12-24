using CoreWPF.Utilites.Navigation;
using CoreWPF.Windows;
using TestConverter.Modules;

namespace TestConverter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowExt
    {
        public MainWindow()
        {
            InitializeComponent();

            NavigationManager nav = new NavigationManager(this.Dispatcher, this.Frame);
            nav.Register<ForMobileViewModel, ModuleView>(new ForMobileViewModel(), App.Modes[0]);
            nav.Register<ForPCViewModel, ModuleView>(new ForPCViewModel(), App.Modes[1]);

            this.DataContext = new MainViewModel(nav);
        }
    }
}
