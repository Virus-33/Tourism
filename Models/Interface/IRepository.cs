using Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IRepository // Доступ к отправлению пользователя на отдых, изменению его числа применений приложения и доступ к наценкам
    {
        public void GoRest(Place place, int userId, DateTime? time);

        public void AddUsage(int userId);

        public Dictionary<string, double> GetPrices();
    }
}
