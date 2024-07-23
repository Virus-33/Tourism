using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Logic
{
    public class StandartPricing : IStrategy    // Конкретная реализация стратегии №3, стандартный прайсинг
    {
        public string Name { get; private set; }

        public StandartPricing()
        {
            Name = "Стандарт";
        }

        // В данном случае пользователь заплатит цену без каких-либо скидок
        public double Calculation(int persons, int price, double koefficient)
        {
            return persons * price * koefficient;
        }
    }
}
