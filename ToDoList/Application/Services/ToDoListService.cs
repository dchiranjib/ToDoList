using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Entities;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Application.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _repository;

        public ToDoListService(IToDoListRepository repository)
        {
            _repository = repository;
        }

        public User GetUser(string userName, string password) => _repository.Users.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
        public User GetUserByUsername(string userName) => _repository.Users.Where(u => u.UserName == userName).FirstOrDefault();

        public IList<ToDoItem> GetToDoItems(User user) => _repository.ToDoItems.Where(i => i.User.UserName == user.UserName).ToList();

        public ToDoItem GetToDoItemById(int id) => _repository.ToDoItems.Where(i => (i.Id == id)).FirstOrDefault();

        public void AddToDoItem(ToDoItem item)
        {
            try
            {
                _repository.Add(item);
                _repository.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void RemoveItem(IList<ToDoItem> items)
        {
            try
            {
                items.ToList().ForEach(i => _repository.Remove(i));
                _repository.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }

    public interface IToDoListService
    {
        User GetUser(string userName, string password);
        User GetUserByUsername(string userName);
        IList<ToDoItem> GetToDoItems(User user);
        ToDoItem GetToDoItemById(int id);

        void AddToDoItem(ToDoItem item);
        void RemoveItem(IList<ToDoItem> items); 
    }
}
