using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MainModels
{
    public class Place // класс, описывающий сущность "Место отдыха"
    {
        public Place(int _id, string _name, int _maxPeople, int _pricePerPerson, int _timeOfRest)
        {
            Id = _id;
            Name = _name;
            MaxPeople = _maxPeople;
            PricePerPerson = _pricePerPerson;
            TimeOfRest = _timeOfRest;
        }

        public int Id { get; private set; }             // ID места отдыха
        public string Name { get; private set; }        // Его название
        public int MaxPeople { get; private set; }      // Максимальное количество человек в этом месте
        public int PricePerPerson { get; private set; } // Цена отдыха за человека
        public int TimeOfRest { get; private set; }     // Время отдыха в этом месте
    }
}
