using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data.Entities;

namespace ToDoList.Infrastructure.Data.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoListDbContext _db;

        public ToDoListRepository(ToDoListDbContext db)
        {
            _db = db;
        }

        public IQueryable<User> Users => _db.Users;
        public IQueryable<ToDoItem> ToDoItems => _db.ToDoItems;

        public void Add<EntityType>(EntityType entity) => _db.Add(entity);

        public void Remove<EntityType>(EntityType entity) => _db.Remove(entity);

        public void SaveChanges() => _db.SaveChanges();
    }
}
