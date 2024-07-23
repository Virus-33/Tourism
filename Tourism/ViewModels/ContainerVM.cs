using Models.Interface;
using Models.MainModels;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Tourism.Views;

namespace Tourism.ViewModels
{
    public class ContainerVM : BaseVM   // Контейнер для всех моделей представления, привязанный к окну
    {

        // Приватное поле и его публичное свойство-дублёр. По идее скрывает реализацию (поле)
        BaseVM _content;
        public BaseVM Content { get => _content; set { _content = value; OnPropertyChanged(nameof(Content)); } }

        // Приватные поля для хранения реализаций интерфейсов до тех пор, пока они не понадобятся при создании модели представления
        IRepository tempRepo;
        IPlaceAccess tempPlaceRepo;

        // Приватное поле для сохранения состояния окна с выбором места отдыха
        MainVM Title;


        /// <summary>
        /// Конструктор класса ContainerVM
        /// </summary>
        /// <param name="repo">Реализация интерфейса IPlaceAccess для доступа к местам отдыха</param>
        /// /// <param name="selectionRepo">Реализация интерфейса IUserSelectionAccess для доступа к пользователям</param>
        /// <param name="repository">Реализация интерфейса IRepository для доступа к остальным данным</param>
        public ContainerVM(IPlaceAccess repo, IUserSelectionAccess selectionRepo, IRepository repository)
        {
            tempPlaceRepo = repo;
            tempRepo = repository;
            USelectionVM usvm = new(selectionRepo, this);
            Content = usvm;
            OnPropertyChanged(nameof(Content));
        }


        /// <summary>
        /// Переключает отображаемый контент на основной экран с выбором места отдыха
        /// </summary>
        /// <param name="user">Залогиненый пользователь</param>
        public void SwitchToTitle(User user)
        {
            MainVM mvm = new(tempPlaceRepo, user, this); // Создаёт новую модель представления выбора места
            Title = mvm;
            Content = mvm;
            OnPropertyChanged(nameof(Content));
        }


        /// <summary>
        /// Переключает отображаемый контент на экран с выбором даты отдыха и подсчётом его стоимости
        /// </summary>
        /// <param name="strategy">Реализация стратегии расчёта цены</param>
        /// <param name="place">Выбранное место отдыха</param>
        /// <param name="userId">Айди залогиненного пользователя</param>
        /// <param name="sender">Контент, вызывавший этот метод. Необходим для сохранения состояния основного экрана</param>
        public void SwitchToPricing(IStrategy strategy, Place place, int userId, MainVM sender)
        {
            Title = sender;
            PlaceOrderVM povm = new(tempRepo, strategy, place, userId, this); // Создаёт новую модель представления расчёта цены на отдых
            Content = povm;
            OnPropertyChanged(nameof(Content));
        }


        /// <summary>
        /// Метод, переключающий отображаемый контент на сохранённый главный экран
        /// </summary>
        public void Back()
        {
            Title.Refresh();
            Content = Title;
            OnPropertyChanged(nameof(Content));
        }
    }
}
