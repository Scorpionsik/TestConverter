using CoreWPF.Utilites;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace TestConverter
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ListExt<string> Modes = new ListExt<string> { "Для телефона (Ассистент)", "Для компьютера (MyTestXPro)" };

        public static string AppPath = new Regex(@"[^\\]+$").Replace(Assembly.GetExecutingAssembly().Location, "");

        public static void KillHelpWindows()
        {
            List<string> name = new List<string> { "Инструкция для получения XML из MTX", "Инструкция для получения MTX из XML" };//процесс, который нужно убить
            System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();//получим процессы
            foreach (System.Diagnostics.Process anti in etc)//обойдем каждый процесс
            {
                foreach (string s in name)
                {
                    if (anti.MainWindowTitle.ToLower().Contains(s.ToLower())) //найдем нужный и убьем
                    {
                        anti.Kill();
                        name.Remove(s);
                        break;
                    }
                }
            }
        }
}
}
