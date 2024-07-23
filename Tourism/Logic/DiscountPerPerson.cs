using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Logic
{
    public class DiscountPerPerson : IStrategy // конкретная реализация стратегии №2
    {
        public string Name { get; private set; }

        public DiscountPerPerson()
        {
            Name = "Скидка 30% за человека";
        }

        // В этом случае цена за человека снижается на 30%
        public double Calculation(int persons, int price, double koefficient)
        {
            return persons * (price / 1.3) * koefficient;
        }
    }
}
