using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ToDoList.Application.Controllers;
using ToDoList.Application.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Entities;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Test
{
    public class Tests
    {
        private ToDoListController _controller;
        private IToDoListRepository _repository;
        private IToDoListService _service; 
        private ILogger<ToDoListController> _logger;

        private IToDoListRepository GetInMemoryRepository()
        { 
            var options = new DbContextOptionsBuilder<ToDoListDbContext>()
                            .UseInMemoryDatabase(databaseName: "MockDB")
                            .Options;
            var context = new ToDoListDbContext(options);

            var repository = new ToDoListRepository(context);

            return repository;
        }

        [SetUp]
        public void Setup()
        {
            _repository = GetInMemoryRepository();
            _service = new ToDoListService(_repository);

            var serviceProvider = new ServiceCollection()
                                .AddLogging()
                                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<ToDoListController>();

            _controller = new ToDoListController(_service, _logger);
        }

        [Test]
        public void Login()
        {
            var user = new User { UserName = "testuser", Password = "testpass" };
            _service.AddUser(user);
            Assert.AreEqual(1, _repository.Users.Where(u => u.UserName.Equals("testuser") && u.Password.Equals("testpass")).Count());

            Assert.Pass();
        }

        [Test]
        public void AddItem()
        {
            var user = new User { UserName = "testuser", Password = "testpass" };
            var toDoListItem = new ToDoItem { Id = 1, Description = "Test Desc", UpdatedOn = DateTime.Now, User = user, Done = false };
            _service.AddToDoItem(toDoListItem);

            Assert.AreEqual(1, _repository.ToDoItems.Where(i => i.Description.Equals("Test Desc")).Count());
            Assert.AreEqual("Test Desc", _repository.ToDoItems.First().Description);

            Assert.Pass();
        }

        [Test]
        public void RemoveItem()
        {
            var user = new User { UserName = "testuser", Password = "testpass" };
            var toDoListItem = new ToDoItem { Id = 1, Description = "Test Desc for delete", UpdatedOn = DateTime.Now, User = user, Done = false };
            _service.AddToDoItem(toDoListItem);

            Assert.AreEqual("Test Desc for delete", _repository.ToDoItems.First().Description);

            var toDoItems = _repository.ToDoItems.Where(i => i.Description.Equals("Test Desc for delete")).ToList();
            _service.RemoveItem(toDoItems);

            Assert.AreEqual(0, _repository.ToDoItems.Where(i => i.Description.Equals("Test Desc")).Count());
        }
    }
}