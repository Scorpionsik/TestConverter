using CoreWPF.Utilites;
using CoreWPF.Windows;
using CoreWPF.Windows.Enums;
using System;
using System.Windows;
using System.Windows.Threading;

namespace CoreWPF.MVVM
{
    /// <summary>
    /// Класс, который автоматически привязывается к событиям <see cref="WindowExt"/>
    /// </summary>
    [Serializable]
    public abstract class ViewModel : NotifyPropertyChanged
    {
        private Dispatcher dispatcher;
        /// <summary>
        /// Позволяет присвоить данной ViewModel <see cref="System.Windows.Threading.Dispatcher"/> текущего окна.
        /// </summary>
        public Dispatcher Dispatcher
        {
            set
            {
                this.dispatcher = value;
            }
        } //---свойство Dispatcher

        private event Action event_close;
        /// <summary>
        /// Позволяет назначить событие для закрытия окна.
        /// </summary>
        public virtual event Action Event_close
        {
            add { this.event_close += value; }
            remove { this.event_close -= value; }
        } //---свойство Event_close

        private event Action event_save;
        /// <summary>
        /// Позволяет назначить событие <see cref="DialogWindowExt"/>; закрытие окна с сохранением результатов. 
        /// </summary>
        public virtual event Action Event_save
        {
            add { this.event_save += value; }
            remove { this.event_save -= value; }
        } //---свойство Event_save

        private event Action event_minimized;
        /// <summary>
        /// Позволяет назначить событие для сворачивания окна.
        /// </summary>
        public virtual event Action Event_minimized
        {
            add { this.event_minimized += value; }
            remove { this.event_minimized -= value; }
        } //---свойство Event_minimized

        private event Action event_state;
        /// <summary>
        /// Позволяет назначить событие для развертывания (и обратно) текущего окна.
        /// </summary>
        public virtual event Action Event_state
        {
            add { this.event_state += value; }
            remove { this.event_state -= value; }
        } //---свойство Event_state

        private string title;
        /// <summary>
        /// Заголовок окна.
        /// </summary>
        public virtual string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        } //---метод Title

        public virtual void ContentRenderedMethod() { }

        /// <summary>
        /// Метод вызывается после срабатывания события <see cref="Window.Closing"/>. Позволяет предотвратить закрытие окна.
        /// </summary>
        /// <returns>Если нужно, чтобы окно было закрыто, нужно вернуть <see cref="WindowClose.Confirm"/>, иначе - <see cref="WindowClose.Abort"/>.</returns>
        public virtual WindowClose CloseMethod()
        {
            return WindowClose.Confirm;
        } //---метод CloseMethod

        /// <summary>
        /// Выполняет указанную <see cref="Action"/> синхронно в потоке <see cref="System.Windows.Threading.Dispatcher"/> текущего окна.
        /// </summary>
        /// <param name="action"><see cref="Action"/> для запуска.</param>
        public void InvokeInMainThread(Action action)
        {
            this.dispatcher.Invoke(action);
        } //---метод InvokeInMainThread

        /// <summary>
        /// Команда для вызова события закрытия окна.
        /// </summary>
        public virtual RelayCommand Command_close
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.event_close?.Invoke();
                });
            }
        } //---команда Command_close

        

        /// <summary>
        /// Команда для вызова события сворачивания окна.
        /// </summary>
        public virtual RelayCommand Command_minimized
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.event_minimized?.Invoke();
                });
            }
        } //---команда Command_minimized

        /// <summary>
        /// Команда для вызова события развертывания (и обратно) текущего окна.
        /// </summary>
        public virtual RelayCommand Command_state
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.event_state?.Invoke();
                });
            }
        } //---команда Command_state

        /// <summary>
        /// Команда для вызова события закрытия окна с сохранением результатов работы.
        /// </summary>
        public virtual RelayCommand Command_save
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.event_save?.Invoke();
                });
            }
        } //---команда Command_save
    } //---класс ViewModel
} //---пространство имён CoreWPF.MVVM
//---EOF