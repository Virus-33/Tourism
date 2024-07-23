using Models.Interface;
using Models.MainModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourism.Logic;

namespace Tourism.ViewModels
{
    /// <summary>
    /// Модель представления экрана выбора пользователя
    /// </summary>
    public class USelectionVM : BaseVM
    {

        // Реализация доступа к списку пользователей в бд и основной контейнер
        IUserSelectionAccess repo;
        ContainerVM Root;


        // команда для привязки действия кнопки к методу
        Command select;
        public Command Select
        {
            get
            {
                return select ??= new Command(obj => ActionSelect());
            }
        }

        Command registering;
        public Command Registering
        {
            get
            {
                return registering ??= new Command(obj => Register());
            }
        }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="iusa">Реализация доступа к пользователям</param>
        /// <param name="root">Основной контейнер представления</param>
        public USelectionVM(IUserSelectionAccess iusa, ContainerVM root)
        {
            repo = iusa;
            Root = root;
            Input = "";

            LoadUsers();
        }


        // коллекция, отображающая список пользователей
        public ObservableCollection<User> Users { get; private set; }

        //Текст, вводимый пользователем (имя)
        string input;
        public string Input { get { return input; } set { input = value; OnPropertyChanged(nameof(Input)); } }

        // выбранный пользователь, привязан к списку пользователей
        User selectedUser;
        public User SelectedUser { get { return selectedUser; } set { selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); } }


        /// <summary>
        /// Получает список пользователей, используя реализацию доступа к данным
        /// </summary>
        void LoadUsers()
        {
            Users = new();
            List<User> users = new();
            users = repo.GetUsers();
            foreach (User u in users) Users.Add(u);
            OnPropertyChanged(nameof(Users));
        }


        /// <summary>
        /// Метод регистрации нового пользователя
        /// </summary>
        void Register()
        {
            if (Input.Length > 0)
            {
                if (Input.Length > 20)
                {
                    Input = Input.Substring(0, 19); // укорачиваем слишком длинное имя пользователя
                }

                User temp = new User(Input);
                repo.AddUser(temp);
                LoadUsers();
            }
        }


        /// <summary>
        /// Метод, который обращается к контейнеру для переключения на экран выбора места
        /// </summary>
        void ActionSelect()
        {
            if (selectedUser != null)
            {
                Root.SwitchToTitle(selectedUser);
            }
        }
    }
}
