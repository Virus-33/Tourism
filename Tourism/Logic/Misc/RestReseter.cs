using Models.MainModels;
using System;
using System.Collections.Generic;

namespace Tourism.Logic.Misc
{
    static class RestReseter // вспомогательный статический класс, созданный для упрощения чтения класса TitleVM
    {

        /// <summary>
        /// Метод, который ищет занятые пользователями места отдыха, время отдыха в которых уже закончилось
        /// </summary>
        /// <param name="places">Список мест отдыха</param>
        /// <param name="ups">Список сущностей Пользователь-Место</param>
        /// <returns>Возвращает список мест с истёкшим сроком отдыха</returns>
        public static List<Place> ResetRest(List<Place> places, List<InPlace> ups)
        {
            List<Place> ToRemove = new();

            for (int i = 0; i < places.Count; i++)
            {
                for (int j = 0; j < ups.Count; j++)
                {
                    if (ups[j].Pid == places[i].Id)
                    {
                        DateTime somedate = ups[j].RestStarted;
                        if (somedate.AddDays(places[i].TimeOfRest) < DateTime.Now)
                        {
                            ToRemove.Add(places[i]);
                        }
                    }
                }
            }

            return ToRemove;
        }

        /// <summary>
        /// Метод, который ищет занятые пользователями места
        /// </summary>
        /// <param name="places">Список мест отдыха</param>
        /// <param name="ups">Список сущностей Пользователь-Место</param>
        /// <returns>Возвращает список мест, которые заняты пользователями</returns>
        public static List<Place> Exclude(List<Place> places, List<InPlace> ups)
        {
            List<Place> ToRemove = new();

            for (int i = 0; i < places.Count; i++)
            {
                for (int j = 0; j < ups.Count; j++)
                {
                    if (ups[j].Pid == places[i].Id)
                    {
                        ToRemove.Add(places[i]);
                    }
                }
            }

            return ToRemove;
        }
    }
}
