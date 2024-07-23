using Models.Interface;
using Models.MainModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Logic;
using Tourism.Logic.Misc;

namespace Tourism.ViewModels
{
    /// <summary>
    /// Модель представления основного экрана
    /// </summary>
    public class MainVM : BaseVM
    {
        // Команда перехода на экран расчёта цены
        Command _continue;
        public Command Continue
        {
            get
            {
                return _continue ??= new Command(obj => PriceScreen());
            }
        }

        // Реализация доступа к местам отдыха и основной контейнер
        IPlaceAccess Repo;
        ContainerVM Root;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="repo">Конкретная реализация доступа к данным</param>
        /// <param name="user">Выбранный пользователь</param>
        /// <param name="root">Основной контейнер</param>
        public MainVM(IPlaceAccess repo, User user, ContainerVM root)
        {
            Repo = repo;
            Root = root;
            CurrentUser = user;
            Pricings = new();

            LoadPlaces();

            FillUI();
        }

        // Текущий пользователь программы
        public User CurrentUser { get; private set; }

        // Коллекции со стратегиями платежей и местами отдыха. Для отображения их в таблице мест и списке прайсингов
        public ObservableCollection<Place> Places { get; private set; }
        public ObservableCollection<IStrategy> Pricings { get; private set; }


        // Строка с короткой сводкой информации по пользователю: имя и статус (обычный/постоянный пользователь)
        public string UserInfo { get; private set; }

        // Выбранное пользователем место отдыха из таблицы, привязано к этой самой таблице
        Place selectedPlace;
        public Place SelectedPlace { get { return selectedPlace; } set { selectedPlace = value; OnPropertyChanged(nameof(SelectedPlace)); } }


        // Выбранный пользователем прайсинг, привязано к списку прайсингов
        IStrategy selectedPricing;
        public IStrategy SelectedPricing { get { return selectedPricing; } set { selectedPricing = value; OnPropertyChanged(nameof(SelectedPricing)); } }
        
        /// <summary>
        /// Метод, предназначенный для вызова контейнером. Обновляет информацию о местах отдыха в этой модели представления при переключении на неё
        /// </summary>
        public void Refresh()
        {
            LoadPlaces();
        }


        /// <summary>
        /// Заполняет информацию о пользователе и список прайсингов
        /// </summary>
        void FillUI()
        {
            UserInfo = "Пользователь: " + CurrentUser.Name + "\tСтатус: ";
            UserInfo += CurrentUser.IsRegularUser() ? "Постоянный клиент" : "Обычный"; // тернарный оператор, то что до ? является условием (метод проверки статуса пользователя)
                                                                                       // всё что после - возвращаемые значения, разделённые : для true и false соответственно 
            OnPropertyChanged(nameof(UserInfo));
            
            //Создаёт стратегии и заполняет ими список для выбора пользователем
            //В случае, если пользователь не является постоянным клиентом, то ему доступен лишь базовый прайсинг
            Pricings.Add(new StandartPricing());
            if (CurrentUser.IsRegularUser())
            {
                Pricings.Add(new DiscountPerPerson());
                Pricings.Add(new DiscountNoKoefficient());
            }
            OnPropertyChanged(nameof(Pricings));
            SelectedPricing = Pricings[0];
        }


        /// <summary>
        /// Асинхронный метод заполнения списка мест отдыха
        /// </summary>
        async void LoadPlaces()
        {
            Places = new();

            List<Place> temp = Repo.GetPlaces();                                                        // получает полный список мест отдыха

            List<Place> toUnrest = await Task.Run(() => RestReseter.ResetRest(temp, Repo.GetRest()));   // получает список мест отдыха с истёкшим временем отдыха (асинхронно)

            foreach (Place p in toUnrest) { Repo.ResetRest(p); }                                        // освобождает их от пользователей внутри базы данных

            List<Place> toExclude = await Task.Run(() => RestReseter.Exclude(temp, Repo.GetRest()));    // получает список занятых пользователями мест (асинхронно)

            List<Place> copy = new();                                                                   // мы не можем удалять элементы из изменяющейся коллекции, поэтому создаём её копию

            copy.AddRange(temp);                                                                        // и заполняем её

            foreach (Place p in temp) { if (toExclude.Contains(p)) { copy.Remove(p); } }                // удаляем занятые места из копии

            foreach (Place p in copy) Places.Add(p);                                                    // заполняем копией коллекцию для отображения в таблице
            OnPropertyChanged(nameof(Places));
        }


        /// <summary>
        /// Метод, обращающийся к основному контейнеру для переключения на экран расчёта цены за отдых
        /// </summary>
        void PriceScreen()
        {
            if (selectedPlace != null)
            {
                Root.SwitchToPricing(selectedPricing, selectedPlace, CurrentUser.Id, this);
            }
        }
    }
}
