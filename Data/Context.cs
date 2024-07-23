using Microsoft.Data.SqlClient;
using Models.Interface;
using Models.MainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Data
{
    public class Context : IPlaceAccess, IUserSelectionAccess, IRepository // указываем, какие интерфейсы реализуем. У каждого свои методы. Это нужно для ограничения доступа
    {

        // Это строка подключения, по ней программа совершает подключение к базе данных.
        static readonly string connectionS = "Server=DANIEL-PC\\LOCAL;DataBase=Turisto;Trusted_Connection=true;TrustServerCertificate=True";

        // Строки с командами доступа к данным
        static readonly string PlacesAccess = "select * from Places";
        static readonly string UsersAccess = "select * from Users";
        static readonly string MultipliersAccess = "select * from PriceMultipliers";
        static readonly string RestingAccess = "select * from InPlaces";

        // Строка с командами добавления данных
        static readonly string PlaceSetOccupant = "insert into InPlaces(usid, pid, RestStarted) values({0}, {1}, \'{2}\')";
        static readonly string AddUserCommand = "insert into users(Name, TimesUsed) values(\'{0}\', 0)";

        // Строка с командой обновления данных
        static readonly string UserAddUsage = "update users set TimesUsed=TimesUsed+1 where id={0}";
        
        // Строка с командой удаления данных
        static readonly string PlaceResetOccupant = "delete from InPlaces where pid = {0}";

        // Метод обновления данных в бд. Увеличивает число применений приложения пользователем
        public void AddUsage(int userId)
        {
            using (SqlConnection con = new(connectionS))
            {
                string temp = string.Format(UserAddUsage, userId);

                SqlCommand command = new(temp, con);

                con.Open();

                command.ExecuteNonQuery(); // выполняет запрос, который не должен возвращать табличные данные
            }
        }

        public void AddUser(User user)
        {
            using (SqlConnection con = new(connectionS))
            {
                string temp = string.Format(AddUserCommand, user.Name);

                SqlCommand command = new(temp, con);

                con.Open();

                command.ExecuteNonQuery(); // выполняет запрос, который не должен возвращать табличные данные
            }
        }

        public List<Place> GetPlaces()
        {
            List<Place> result = new();

            using (SqlConnection con = new(connectionS)) // создание подключения к бд
            {
                SqlCommand command = new(PlacesAccess, con); // создание команды к бд

                con.Open(); // открытие подключения

                using (SqlDataReader reader = command.ExecuteReader()) // выполнение запроса на чтение данных
                {
                    if (reader.HasRows) // проверка на наличие прочитанных данных
                    {
                        while (reader.Read()) // цикл с выделением данных
                        {
                            // заполняет возвращаемый список данными
                            Place temp = new Place(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
                            result.Add(temp);
                        }
                    }
                }
            }

            // возвращает данные
            return result;
        }


        // Метод получения наценок дней недели
        public Dictionary<string, double> GetPrices()
        {
            Dictionary<string, double> result = new();

            using (SqlConnection con = new(connectionS))
            {
                SqlCommand command = new(string.Format(MultipliersAccess), con);

                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(1), reader.GetDouble(2));
                        }
                    }
                }
            }

            return result;
        }

        // Метод получения мест, занятых пользователями
        public List<InPlace> GetRest()
        {
            List<InPlace> result = new();

            using (SqlConnection con = new(connectionS))
            {
                SqlCommand command = new(RestingAccess, con);

                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            InPlace temp = new InPlace(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDateTime(3));
                            result.Add(temp);
                        }
                    }
                }
            }

            return result;
        }


        // Метод получения пользователей
        public List<User> GetUsers()
        {
            List<User> result = new();

            using (SqlConnection con = new(connectionS))
            {
                SqlCommand command = new(UsersAccess, con);

                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User temp = new User(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                            result.Add(temp);
                        }
                    }
                }
            }

            return result;
        }


        // Добавляет в БД запись о том, что пользователь отдыхает в конкретном месте
        public void GoRest(Place place, int userId, DateTime? time)
        {
            using (SqlConnection con = new(connectionS))
            {
                string temp = string.Format(PlaceSetOccupant, userId, place.Id, time);

                SqlCommand command = new(temp, con);

                con.Open();

                command.ExecuteNonQuery();
            }
        }


        // Метод удаления пользователя из места отдыха
        public void ResetRest(Place place)
        {
            using (SqlConnection con = new(connectionS))
            {
                string temp = string.Format(PlaceResetOccupant, place.Id);

                SqlCommand command = new(temp, con);

                con.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
