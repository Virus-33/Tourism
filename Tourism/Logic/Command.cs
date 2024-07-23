using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tourism.Logic
{
    public class Command : ICommand // класс команды, реализующий интерфейс ICommand
    {                               // С помощью этого класса осуществляется привязка методов к кнопка в представлениях
        Action<object> act;

        Func<object, bool> canAct;

        public Command(Action<object> act, Func<object, bool>? canAct = null)
        {
            this.act = act;
            this.canAct = canAct;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canAct == null || this.canAct(parameter);
        }

        public void Execute(object parameter)
        {
            this.act(parameter);
        }
    }
}
