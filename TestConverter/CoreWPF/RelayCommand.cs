using System;
using System.Windows.Input;

namespace CoreWPF.Utilites
{
    /// <summary>
    /// Класс, реализующий интерфейс <see cref="ICommand"/>; определяет команду, принимающей в качестве параметра <see cref="object"/>
    /// </summary>
    [Serializable]
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Хранит тело команды
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// Хранит условие выполения команды
        /// </summary>
        private Func<object, bool> canExecute;

        /// <summary>
        /// Происходит при <see cref="System.Windows.Input.CommandManager"/> определяет условия, которые могут повлиять на возможность выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        } //---свойство CanExecuteChanged

        public RelayCommand() { }

        /// <summary>
        /// Конструктор инициализации команды
        /// </summary>
        /// <param name="execute">Принимает тело команды</param>
        /// <param name="canExecute">Принимает условие выполнения команды</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        } //---конструктор RelayCommand

        /// <summary>
        /// Метод для проверки, может ли данная команда выполняться в ее текущем состоянии.
        /// </summary>
        /// <param name="parameter">Данные, используемые данной командой.</param>
        /// <returns>Вернет true, если эту команду можно выполнить; в противном случае — false.</returns>
        public bool CanExecute(object parameter = null)
        {
            return this.canExecute == null || this.canExecute(parameter);
        } //---метод CanExecute

        /// <summary>
        /// Метод для вызова текущей команды.
        /// </summary>
        /// <param name="parameter">Данные, которые будут переданы для текущей команды.</param>
        public void Execute(object parameter = null)
        {
            this.execute(parameter);
        } //---метод Execute
    } //---класс RelayCommand

    /// <summary>
    /// Класс, реализующий интерфейс <see cref="ICommand"/>; определяет команду, принимающей в качестве параметра <see cref="T"/>
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// Хранит тело команды
        /// </summary>
        private Action<T> execute;

        /// <summary>
        /// Хранит условие выполения команды
        /// </summary>
        private Func<T, bool> canExecute;

        /// <summary>
        /// Происходит при <see cref="System.Windows.Input.CommandManager"/> определяет условия, которые могут повлиять на возможность выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        } //---свойство CanExecuteChanged

        /// <summary>
        /// Конструктор инициализации команды
        /// </summary>
        /// <param name="execute">Принимает тело команды</param>
        /// <param name="canExecute">Принимает условие выполнения команды</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        } //---конструктор RelayCommand

        /// <summary>
        /// Метод для проверки, может ли данная команда выполняться в ее текущем состоянии.
        /// </summary>
        /// <param name="parameter">Данные, используемые текущей командой.</param>
        /// <returns>Вернет true, если эту команду можно выполнить; в противном случае — false.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute((T)parameter);
        } //---метод CanExecute

        /// <summary>
        /// Метод для вызова текущей команды.
        /// </summary>
        /// <param name="parameter">Данные, которые будут переданы для текущей команды.</param>
        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        } //---метод Execute
    } //---класс RelayCommand<T>
} //---пространство имён CoreWPF.Utilites
//---EOF