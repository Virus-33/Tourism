using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IStrategy // интерфейс стратегии. Необходим для корректной работы паттерна Стратегия
    {
        public double Calculation(int persons, int price, double koefficient);
    }
}
