using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Logic
{
    public class DiscountNoKoefficient : IStrategy //Конкретная реализация стратегии №1
    {
        public string Name { get; private set; }   // название стратегии. Отображается в выпадающем списке выбора прайсинга

        public DiscountNoKoefficient()
        {
            Name = "Без наценки";                  // название стратегии фиксировано
        }

        // конкретный метод реализации стратегии. В этом случае не учитывается наценка за выходной/будний день
        public double Calculation(int persons, int price, double koefficient)
        {
            return persons * price;
        }
    }
}
