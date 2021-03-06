﻿using CoreWPF.MVVM;
using CoreWPF.Utilites;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace TestConverter.Modules
{
    public abstract class ModuleViewModel : NotifyPropertyChanged
    {
        public abstract string Title { get; }
        public abstract string SavefileFilter { get; }
        private bool IsValidFile = false;
        private string filepath;
        public string Filepath
        {
            get { return this.filepath; }
            private set
            {
                this.filepath = value;
                this.OnPropertyChanged("Filepath");
                this.OnPropertyChanged("FileStatus");
            }
        }

        public string FileStatus
        {
            get
            {
                if (this.Filepath == null || this.Filepath.Length == 0) return "Выберите файл";
                else
                {
                    this.IsValidFile = this.CheckFile();
                    if (this.IsValidFile)
                    {
                        return "Готов к обработке";
                    }
                    else
                    {
                        return "Файл некорректен!";
                    }
                }
            }
        }

        private string errorString;
        public string ErrorString
        {
            get { return this.errorString; }
            set
            {
                this.errorString = value;
                this.OnPropertyChanged("ErrorString");
            }
        }

        public string Filename { get; private set; }

        public string Filter { get; private set; }

        public ModuleViewModel(string filter)
        {
            this.Filter = filter;
        }

        public abstract bool CheckFile();

        public RelayCommand<string> Command_GetFilepath
        {
            get
            {
                return new RelayCommand<string>(obj =>
                {
                    if(obj != null && obj.Length > 0)
                    {
                        this.Filepath = obj;
                        this.Filename = new Regex(@"\..+$").Replace(new Regex(@"[^\\]+$").Match(obj).Value, "");
                        return;
                    }
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

        public virtual RelayCommand Command_StartConvert
        {
            get
            {
                return new RelayCommand(obj => 
                {
                    if (obj == null) this.SaveFile(this.GetPathForSavefile());
                }, (obj) => this.Filepath != null && this.Filepath.Length > 0 && this.IsValidFile);
            }
        }

        public abstract RelayCommand Command_HelpLink
        {
            get;
        }

        public RelayCommand Command_UpdateStatus
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.OnPropertyChanged("FileStatus");
                });
            }
        }

        public string GetPathForSavefile()
        {
            if (this.Filename != null && this.Filename.Length > 0)
            {
                SaveFileDialog window = new SaveFileDialog();
                window.Title = "Сохранение теста...";
                window.Filter = this.SavefileFilter;
                window.FileName = this.Filename;
                if ((bool)window.ShowDialog())
                {
                    return window.FileName;
                }
                else return string.Empty;
            }
            else return string.Empty;
        }

        public abstract void SaveFile(string path);
    }
}
