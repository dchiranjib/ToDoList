using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data.Entities;

namespace ToDoList.Infrastructure.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "chdas",
                    Password = "pass123"
                },
                new User
                {
                    Id = 2,
                    UserName = "chida",
                    Password = "pass312"
                }
            );
        }
    }
}
