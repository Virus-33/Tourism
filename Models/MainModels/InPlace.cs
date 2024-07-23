using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MainModels
{
    public class InPlace // Модель сущности Пользователь-Место. Фактическая связь многие-ко-многим, ограниченная внутри программы до 1:1.
                         // Так вышло из-за необходимости хранения дополнительного параметра (время начала отдыха)
    {
        public InPlace(int id, int uid, int pid, DateTime datetime)
        {
            Id = id;
            Uid = uid;
            Pid = pid;
            RestStarted = datetime;
        }

        public int Id { get; private set; }
        public int Uid { get; private set; } // ID пользователя в месте отдыха
        public int Pid { get; private set; } // ID места отдыха

        public DateTime RestStarted { get; private set; } // дата начала отдыха
    }
}
