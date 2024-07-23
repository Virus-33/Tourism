using Data;
using Models.Interface;
using Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using Tourism.Logic;

namespace Tourism.ViewModels
{
    /// <summary>
    /// Модель представления для бронирования отдыха в месте для отдыха
    /// </summary>
    public class PlaceOrderVM : BaseVM
    {
        // приватные поля
        IStrategy Pricing;          // Конкретная реализация стратегии расчёта цены
        IRepository Repos;          // Конкретная реализация доступа к данным
        ContainerVM Root;           // Основной контейнер
        Place Place;                // Выбранное место отдыха
        int UserId;                 // Id пользователя
        double currentModifier = 0; // Наценка на отдых, зависит от дня недели


        // Команды. Служат для привязки действия кнопки к методу
        Command confirm;
        public Command Confirm
        {
            get
            {
                return confirm ??= new Command(obj => Order()); // Создаёт новый экземпляр команды, если его ещё не было создано
            }
        }

        Command goBack;
        public Command GoBack
        {
            get
            {
                return goBack ??= new Command(obj => Return());
            }
        }

        Command calculate;
        public Command Calculate
        {
            get
            {
                return calculate ??= new Command(obj => CountAndBuild());
            }
        }


        /// <summary>
        /// Конструктор класса PlaceOrderVM
        /// </summary>
        /// <param name="Repo">Релизация интерфейса доступа к данным</param>
        /// <param name="pricing">Реализация стратегии расчёта цены</param>
        /// <param name="place">Выбранное пользователем место отдыха</param>
        /// <param name="userId">Айди залогиненного пользователя</param>
        /// <param name="root">Основной контейнер</param>
        public PlaceOrderVM(IRepository Repo, IStrategy pricing, Place place, int userId, ContainerVM root)
        {
            Root = root;
            Pricing = pricing;
            Repos = Repo;
            Place = place;
            UserId = userId;
            Summary = "";

            FillItems();
        }

        // Список для привязки данных к выбору числа человек для отдыха
        // Сделано чтоб не доверять пользовательскому вводу
        public List<int> ComboItems { get; private set; }

        // Итоговая стоимость отдыха
        public string Summary { get; private set; }


        // Выбранное пользователем число человек для отдыха
        int selectedAmount;
        public int SelectedAmount { get { return selectedAmount; } set { selectedAmount = value; OnPropertyChanged(nameof(SelectedAmount)); } }


        // Выбранная пользователем дата поездки
        // ? означает, что здесь допустимо значение null
        // Сделано чтобы можно было выбрать дату быстрее (по умолчанию стартовый год - первый от рождения Христа)
        DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            
            // При изменении вызывает метод, который получает наценку за день начала отдыха
            set { selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); GetPriceMultiplier(); }
        }


        /// <summary>
        /// Заполняет список числа людей для поездки
        /// </summary>
        void FillItems()
        {
            ComboItems = new();

            for (int i = 1; i <= Place.MaxPeople; i++)
            {
                ComboItems.Add(i);
            }

            OnPropertyChanged(nameof(ComboItems));
        }


        /// <summary>
        /// Считает итоговую стоимость поездки, используя реализацию стратегии расчёта и конвертирует её в строку для отображения
        /// Результат будет округлён с помощью Math.Round()
        /// </summary>
        void CountAndBuild()
        {
            if (selectedAmount != 0 && currentModifier != 0)
            {
                Summary = Math.Round(Pricing.Calculation(selectedAmount, Place.PricePerPerson, currentModifier)).ToString();
                OnPropertyChanged(nameof(Summary));
            }
        }


        /// <summary>
        /// Метод, получающий наценку за выбранный пользователем день начала отдыха, используя реализацию доступа к данным
        /// </summary>
        void GetPriceMultiplier()
        {
            if (selectedDate.Value.DayOfWeek == DayOfWeek.Saturday || selectedDate.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                currentModifier = Repos.GetPrices()["выходной"];
            } else
            {
                currentModifier = Repos.GetPrices()["будни"];
            }
        }


        /// <summary>
        /// Записывает информацию о том, что пользователь отдыхает в выбранном месте, используя реализацию доступа к данным
        /// </summary>
        void Order()
        {
            if (selectedAmount != 0 && selectedDate != null && Summary.Length > 0)
            {
                Repos.AddUsage(UserId);
                Repos.GoRest(Place, UserId, selectedDate);
            }
        }

        /// <summary>
        /// Обращается к основному контейнеру для переключения контента на основной экран
        /// </summary>
        void Return()
        {
            Root.Back();
        }
    }
}
