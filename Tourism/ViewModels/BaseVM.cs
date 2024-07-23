using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.ViewModels
{
    public class BaseVM : INotifyPropertyChanged    // Базовый класс для всех моделей представления.
                                                    // Они наследуются от него, сразу имя при себе реализацию интерфейса INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        /// Метод, уведомляющий об изменении свойства
        /// </summary>
        /// <param name="name">Имя свойства</param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
