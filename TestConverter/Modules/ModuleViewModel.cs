using CoreWPF.MVVM;
using CoreWPF.Utilites;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace TestConverter.Modules
{
    public abstract class ModuleViewModel : NotifyPropertyChanged
    {
        public abstract string Title { get; }
        public abstract string SavefileFilter { get; }

        private string filepath;
        public string Filepath
        {
            get { return this.filepath; }
            private set
            {
                this.filepath = value;
                this.OnPropertyChanged("Filepath");
            }
        }

        public string Filename { get; private set; }

        public string Filter { get; private set; }

        public ModuleViewModel(string filter)
        {
            this.Filter = filter;
        }

        public RelayCommand Command_GetFilepath
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    OpenFileDialog window = new OpenFileDialog();
                    window.Filter = this.Filter;
                    window.Title = "Выберите тест для работы";
                    if((bool)window.ShowDialog())
                    {
                        this.Filepath = window.FileName;
                        this.Filename = new Regex(@"\..+$").Replace(window.SafeFileName, "");
                    }
                });
            }
        }

        public abstract RelayCommand Command_StartConvert
        {
            get;
        }

        public string GetPathForSavefile()
        {
            SaveFileDialog window = new SaveFileDialog();
            window.FileName = this.Filename + "." + this.SavefileFilter;
            if ((bool)window.ShowDialog())
            {
                return window.FileName;
            }
            else return string.Empty;
        }

        public abstract void SaveFile(string path);
    }
}
