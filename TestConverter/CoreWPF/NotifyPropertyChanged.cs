using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoreWPF.MVVM
{
    /// <summary>
    /// Реализует интерфейс <see cref="INotifyPropertyChanged"/>
    /// </summary>
    [Serializable]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие для обновления привязанного объекта (в XAML)
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод для обновления выбранного привязанного объекта (в XAML)
        /// </summary>
        /// <param name="prop">Принимает строку-имя объекта, который необходимо обновить</param>
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        //---метод OnPropertyChanged
    } //---класс NotifyPropertyChanged
} //---пространство имён CoreWPF.MVVM
//---EOF