using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
using System.Linq;

namespace CoreWPF.Utilites
{
    /// <summary>
    /// Наследуется от <see cref="ObservableCollection{T}"/>, расширяя функционал
    /// </summary>
    /// <typeparam name="T">Принимает любой <see cref="object"/></typeparam>
    [Serializable]
    public class ListExt<T> : ObservableCollection<T>
    {
        #region Свойства
        /// <summary>
        /// Возвращает первый элемент последовательности
        /// </summary>
        public T First
        {
            get
            {
                return this.First();
            }
        } //---свойтсво FirstElement

        /// <summary>
        /// Возвращает последний элемент последовательности
        /// </summary>
        public T Last
        {
            get
            {
                return this.Last();
            }
        } //---свойство LastElement
        #endregion

        #region Констукторы
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ListExt{T}"/>.
        /// </summary>
        public ListExt() : base()
        {
            //For Async ListExt
            //CreateAsyncOp();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ListExt{T}"/>, который содержит элементы, скопированные из указанной коллекции.
        /// </summary>
        /// <param name="collection">Принимает коллекцию, которая будет скорпирована в текущий экземпляр <see cref="ListExt{T}"/>.</param>
        public ListExt(IEnumerable<T> collection) : base(collection)
        {
            //For Async ListExt
            //CreateAsyncOp();
        }
        #endregion

        #region Методы
        /// <summary>
        /// Добавляет коллекцию в конец массива
        /// </summary>
        /// <param name="collection">Принимает коллекцию для добавления</param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T model in collection)
            {
                this.Add(model);
            }
        } //---метод AddRange

        /// <summary>
        /// Добавляет коллекцию в указанный индекс массива
        /// </summary>
        /// <param name="index">Принимает индекс</param>
        /// <param name="collection">Принимает коллекцию для добавления</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (index < 0 && index >= this.Count()) return;
            foreach (T model in Inverse(collection))
            {
                this.Insert(index, model);
            }
        } //---метод InsertRange

        /// <summary>
        /// Возвращает первый элемент последовательности, удовлетворяющий указанному условию
        /// </summary>
        /// <param name="predicate">Функция для проверки каждого элемента на соответствие условию</param>
        public T FindFirst(Func<T,bool> predicate)
        {
            return this.First(predicate);
        } //---метод FindFirst

        /// <summary>
        /// Возвращает последний элемент последовательности, удовлетворяющий указанному условию
        /// </summary>
        /// <param name="predicate">Функция для проверки каждого элемента на соответствие условию</param>
        public T FindLast(Func<T, bool> predicate)
        {
            return this.Last(predicate);
        } //---метод FindLast

        /// <summary>
        /// Возвращает коллекцию, соответствующую указанному условию.
        /// </summary>
        /// <param name="predicate">Условие для проверки коллекции.</param>
        /// <returns>Возвращает коллекцию, соответствующую указанному условию.</returns>
        public ListExt<T> FindRange(Func<T, bool> predicate)
        {
            ListExt<T> tmp_send = new ListExt<T>();

            foreach (T model in this)
            {
                if (predicate(model)) tmp_send.Add(model);
            }

            return tmp_send;
        } //---метод FindRange

        /// <summary>
        /// Перетасовывает элементы коллекции.
        /// </summary>
        /// <returns>Возвращает перетасованную копию коллекции.</returns>
        public ListExt<T> Shuffle()
        {
            int[] tmp_index = new int[this.Count];
            ListExt<T> tmp_shuffle = new ListExt<T>();
            for (int i = 0; i < this.Count; i++)
            {
                tmp_index[i] = i;
            }

            for(int index = 0, step = tmp_index.Length; index < tmp_index.Length; index++, step--)
            {
                int tmp_rand = new Random().Next(step);
                tmp_shuffle.Insert(0, this[tmp_index[tmp_rand]]);
                tmp_index[tmp_rand] = tmp_index[step - 1];
            }
            
            return tmp_shuffle;
        } //---метод Shuffle

        /// <summary>
        /// Удаляет из данного массива элементы коллекции; сравнивает объекты массива и коллекции как критерий
        /// </summary>
        /// <param name="collection">Принимает коллекцию элементов для удаления</param>
        public void RemoveRange(IEnumerable<T> collection)
        {
            foreach (T remove in collection)
            {
                foreach (T model in this)
                {
                    if (model.Equals(remove))
                    {
                        this.Remove(model);
                        break;
                    }
                }
            }
        } //---метод RemoveRange

        /// <summary>
        /// Инвертирует текущую коллекцию
        /// </summary>
        /// <returns>Возвращает инвертированную текущую коллекцию</returns>
        public ListExt<T> Inverse()
        {
            ListExt<T> tmp_send = new ListExt<T>();

            for (int i = this.Count() - 1; i >= 0; i--)
            {
                tmp_send.Add(this.ElementAt(i));
            }

            return tmp_send;
        }//---метод Inverse
        #endregion

        #region Async ListExt
        /*
        [NonSerialized]
        private AsyncOperation asyncOp = null;

        private void CreateAsyncOp()
        {
            // Create the AsyncOperation to post events on the creator thread
            asyncOp = AsyncOperationManager.CreateOperation(null);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Post the CollectionChanged event on the creator thread
            asyncOp.Post(RaiseCollectionChanged, e);
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            // Post the PropertyChanged event on the creator thread
            asyncOp.Post(RaisePropertyChanged, e);
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }*/
        #endregion

        #region Статические методы
        /// <summary>
        /// Инвертирует полученную коллекцию
        /// </summary>
        /// <param name="collection">Принимает коллекцию</param>
        /// <returns>Возвращает инвертированную коллекцию</returns>
        public static T[] Inverse(IEnumerable<T> collection)
        {
            ListExt<T> tmp_send = new ListExt<T>();

            for (int i = collection.Count() - 1; i >= 0; i--)
            {
                tmp_send.Add(collection.ElementAt(i));
            }

            return tmp_send.ToArray();
        } //---метод Inverse

        /// <summary>
        /// Возвращает коллекцию, соответствующую указанному условию.
        /// </summary>
        /// <param name="collection">Коллекция для проверки.</param>
        /// <param name="predicate">Условие для проверки коллекции.</param>
        /// <returns>Возвращает коллекцию, соответствующую указанному условию.</returns>
        public static T[] FindRange(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            ListExt<T> tmp_send = new ListExt<T>();

            foreach (T model in collection)
            {
                if (predicate(model)) tmp_send.Add(model);
            }

            return tmp_send.ToArray();
        }
        #endregion
    } //---класс ListExt<T>
} //---пространство имён CoreWPF.Utilites
//---EOF