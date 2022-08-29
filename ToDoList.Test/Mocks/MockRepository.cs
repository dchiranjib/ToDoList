using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data.Entities;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Test.Mocks
{
    internal class MockRepository : IToDoListRepository
    {
        private readonly List<User> _users = new List<User>();
        private readonly List<ToDoItem> _toDoItems = new List<ToDoItem>();

        public IQueryable<User> Users => _users.AsQueryable();
        public IQueryable<ToDoItem> ToDoItems => _toDoItems.AsQueryable();

        public void Add<EntityType>(EntityType entity)
        {
            switch (entity)
            {
                case ToDoItem toDoItem:
                    _toDoItems.Add(toDoItem);
                    break;
            }
        }

        public void Remove<EntityType>(EntityType entity)
        {
            switch (entity)
            {
                case ToDoItem toDoItem:
                    _toDoItems.Remove(toDoItem);
                    break;
            }
        }

        public void SaveChanges()
        {
        }
    }
}
