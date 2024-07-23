using System;
using Models.MainModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IUserSelectionAccess // Доступ к списку пользователей
    {
        public List<User> GetUsers();

        public void AddUser(User user);
    }
}
