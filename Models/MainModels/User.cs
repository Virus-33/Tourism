using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MainModels
{
    public class User // Класс, описывающий сущность "Пользователь"
    {

        public User(int _id, string _name, int _usageAmount)
        {
            Id = _id;
            Name = _name;
            UsageAmont = _usageAmount;
        }

        public User(string _name)
        {
            Name = _name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public int UsageAmont { get; private set; }


        /// <summary>
        /// Метод, определяющий, является ли пользователь постоянным пользователем приложения
        /// </summary>
        /// <returns>Возвращает true, если пользователь пользовался приложением более 8 раз и false в ином случае</returns>
        public bool IsRegularUser()
        {
            return UsageAmont >= 8;
        }
    }
}
