using Data;
using Models.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tourism.ViewModels;

namespace Tourism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Переопределяем базовый метод старта приложения
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаём экземпляр нового окна
            MainWindow window = new();

            // Создаёт реализации интерфейсов для разделения доступа к данным со стороны моделей представлений
            IPlaceAccess IPA = new Context();
            IRepository IRepo = new Context();
            IUserSelectionAccess IUSA = new Context();

            // Создаёт контейнер моделей представления, который сам является моделью представления
            var cvm = new ContainerVM(IPA, IUSA, IRepo);

            // Привязывает его как контекст данных к окну
            window.DataContext = cvm;

            // Показывает окно
            window.Show();
        }
    }
}
