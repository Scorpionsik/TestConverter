using CoreWPF.Utilites.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CoreWPF.Utilites.Navigation
{
    /// <summary>
    /// Предоставляет инструменты для переключения нескольких ViewModel в одном окне.
    /// </summary>
    [Serializable]
    public class NavigationManager
    {
        #region Поля и свойства
        private event Action<INavigateModule> navigation_invoke;

        /// <summary>
        /// Событие триггерится при каждом срабатывании метода <see cref="Navigate(string, object)"/>, при условии что ViewModel отнаследована от <see cref="INavigateModule"/>.
        /// </summary>
        public Action<INavigateModule> Navigation_invoke
        {
            get { return this.navigation_invoke; }
            set
            {
                this.navigation_invoke = new Action<INavigateModule>(value);
            }
        } //---свойство Navigation_invoke

        private readonly Dispatcher _dispatcher;
        private readonly ContentControl _frameControl;
        private readonly IDictionary<string, object> _viewModelsByNavigationKey = new Dictionary<string, object>();
        private readonly IDictionary<Type, Type> _viewTypesByViewModelType = new Dictionary<Type, Type>();

        /// <summary>
        /// Возвращает коллекцию ключей, которые были указаны при вызове метода <see cref="Register{TViewModel, TView}(TViewModel, string)"/>.
        /// </summary>
        public List<string> Keys
        {
            get
            {
                return new List<string>(this._viewModelsByNavigationKey.Keys);
            }
        } //---свойство Keys
        #endregion

        #region Конструкторы
        /// <summary>
        /// Инициализация менеджера.
        /// <para>Начало работы: Сначала требуется создать экземпляр текущего менеджера и передать в конструкторе <see cref="Dispatcher"/> и <see cref="ContentControl"/> окна, в котором будут меняться ViewModel.
        /// Далее регистрируются ViewModel (в который передается ссылка на текущий менеджер), View и ключ, по которому можно будет найти ViewModel в коллекции менеджера. Рекомендуется отдельно хранить список ключей, по которым можно обратиться к тому или иному ViewModel.
        /// После чего менеджер передается в конструкторе ViewModel основного окна.</para>
        /// Для удобства рекомендуется наследоваться от классов <see cref="NavigationViewModel"/> для вашей ViewModel и <see cref="NavigationModel"/> для вашей модели представления данных.
        /// </summary>
        /// <param name="dispatcher"><see cref="Dispatcher"/> окна, в котором данный диспетчер будет работать.</param>
        /// <param name="frameControl"><see cref="ContentControl"/> окна, в котором будут переключаться ViewModel.</param>
        /// <exception cref="ArgumentNullException"/>
        public NavigationManager(Dispatcher dispatcher, ContentControl frameControl)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher", "Dispatcher не указан!");
            if (frameControl == null)
                throw new ArgumentNullException("frameControl", "FrameControl не указан!");

            _dispatcher = dispatcher;
            _frameControl = frameControl;
        } //---конструктор NavigationManager
        #endregion

        #region Методы
        public void Register<TViewModel, TView>(TViewModel viewModel, string navigationKey)
            where TViewModel : class
            where TView : FrameworkElement
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");
            if (navigationKey == null)
                throw new ArgumentNullException("navigationKey");

            _viewModelsByNavigationKey[navigationKey] = viewModel;
            _viewTypesByViewModelType[typeof(TViewModel)] = typeof(TView);
        }

        public void Navigate(string navigationKey, object arg = null)
        {
            if (navigationKey == null)
                throw new ArgumentNullException("navigationKey");
            if (!this.Keys.Contains(navigationKey))
                throw new ArgumentException("Ключ не найден!", "navigationKey");

            InvokeInMainThread(() =>
            {
                InvokeNavigatingFrom();
                var viewModel = GetNewViewModel(navigationKey);
                InvokeNavigatingTo(viewModel, arg);

                var view = CreateNewView(viewModel);
                _frameControl.Content = view;

                if (viewModel is INavigateModule module)
                {
                    this.Navigation_invoke?.Invoke(module);
                }
            });
        }

        private void InvokeInMainThread(Action action)
        {
            _dispatcher.Invoke(action);
        }

        private FrameworkElement CreateNewView(object viewModel)
        {
            var viewType = _viewTypesByViewModelType[viewModel.GetType()];
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            return view;
        }

        private object GetNewViewModel(string navigationKey)
        {
            return _viewModelsByNavigationKey[navigationKey];
        }

        private void InvokeNavigatingFrom()
        {
            var oldView = _frameControl.Content as FrameworkElement;
            if (oldView == null)
                return;

            var navigationAware = oldView.DataContext as INavigateModule;
            if (navigationAware == null)
                return;

            navigationAware.OnNavigatingFrom();
        }

        private static void InvokeNavigatingTo(object viewModel, object arg)
        {
            var navigationAware = viewModel as INavigateModule;
            if (navigationAware == null)
                return;

            navigationAware.OnNavigatingTo(arg);
        }

        public string GetSubtitle(string navigationKey)
        {
            if (!this.Keys.Contains(navigationKey)) throw new ArgumentException("Ключ не найден!", "navigationKey");

            if (_viewModelsByNavigationKey[navigationKey] is INavigateModule module) return module.Subtitle;
            else throw new InvalidCastException("Выбранная ViewModel ("+ navigationKey +") не наследуется от INavigateModule!");
        }
        #endregion
    }
}
