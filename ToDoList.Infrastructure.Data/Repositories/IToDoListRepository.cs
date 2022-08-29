using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data.Entities;

namespace ToDoList.Infrastructure.Data.Repositories
{
    public interface IToDoListRepository
    {
        public IQueryable<User> Users { get; }
        public IQueryable<ToDoItem> ToDoItems { get; }

        void Add<EntityType>(EntityType entity);

        void Remove<EntityType>(EntityType entity);

        void SaveChanges();
    }
}
