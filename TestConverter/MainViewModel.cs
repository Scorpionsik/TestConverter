using CoreWPF.MVVM;
using CoreWPF.Utilites;
using CoreWPF.Utilites.Navigation;
using CoreWPF.Windows.Enums;
using Microsoft.Win32;
using System;
using System.IO;

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
            int index = 0;
            try
            {
                RegistryKey currentUserKey = Registry.CurrentUser;
                currentUserKey = currentUserKey.OpenSubKey("SOFTWARE", true);
                if (currentUserKey.OpenSubKey("Coreman soft", true) == null) currentUserKey.CreateSubKey("Coreman soft");
                currentUserKey = currentUserKey.OpenSubKey("Coreman soft", true);
                if (currentUserKey.OpenSubKey("TestConverter", true) == null) currentUserKey.CreateSubKey("TestConverter").SetValue("saveMode", "0");
                currentUserKey = currentUserKey.OpenSubKey("TestConverter", true);
                index = Convert.ToInt32(currentUserKey.GetValue("saveMode").ToString());
            }
            catch { }

            this.Title = "Конвертер тестов";
            this.navigator = navigator;
            this.Modes = new ListExt<string>(App.Modes);
            try
            {
                this.Select_mode = this.Modes[index];
            }
            catch
            {
                this.Select_mode = this.Modes.First;
            }
        }

        public override WindowClose CloseMethod()
        {
            try
            {
                RegistryKey currentUserKey = Registry.CurrentUser;
                currentUserKey = currentUserKey.OpenSubKey("SOFTWARE", true);
                currentUserKey = currentUserKey.OpenSubKey("Coreman soft", true);
                currentUserKey = currentUserKey.OpenSubKey("TestConverter", true);
                currentUserKey.SetValue("saveMode", (this.Modes.IndexOf(this.Select_mode)).ToString());
            }
            catch { }

            App.KillHelpWindows();
            try
            {
                if (File.Exists(App.AppPath + "1.chm"))
                {
                    File.Delete(App.AppPath + "1.chm");
                }
                if (File.Exists(App.AppPath + "2.chm"))
                {
                    File.Delete(App.AppPath + "2.chm");
                }
            }
            catch { }
            

            return base.CloseMethod();
        }
    }
}
