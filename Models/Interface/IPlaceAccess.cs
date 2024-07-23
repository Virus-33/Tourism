using Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IPlaceAccess // интерфейс доступа к местам отдыха, записям об отдыхе и право на удаление записи об отдыхе
    {
        public List<Place> GetPlaces();

        public List<InPlace> GetRest();

        public void ResetRest(Place place);
    }
}
