using CoreWPF.Utilites;
using System.Windows;

namespace TestConverter
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ListExt<string> Modes = new ListExt<string> { "Для телефона (Ассистент)", "Для компьютера (MyTestXPro)" };
}
}
