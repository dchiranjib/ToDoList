using Microsoft.EntityFrameworkCore;
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

        private IToDoListRepository GetInMemoryRepository()
        { 
            var context = new ToDoListDbContext();

            var repository = new ToDoListRepository(context);

            return repository;
        }

        [SetUp]
        public void Setup()
        {
            _repository = GetInMemoryRepository();// new Mocks.MockRepository();
            _controller = new ToDoListController(_service);
        }

        [Test]
        public void AddItem()
        {
            var toDoListItem = new ToDoItem { Description = "Test Desc" };
            _controller.AddItem(toDoListItem);

            Assert.AreEqual(1, _repository.ToDoItems.Count());
            Assert.AreEqual("Test Desc", _repository.ToDoItems.First().Description);

            Assert.Pass();
        }

        [Test]
        public void RemoveItem()
        {
            var toDoListItem = new ToDoItem { Description = "Test Desc for delete" };
            _controller.AddItem(toDoListItem);

            var toDoItemId = _repository.ToDoItems.Where(i => i.Description.Equals("Test Desc for delete")).First().Id;
            _controller.RemoveItem(toDoItemId);

            Assert.AreEqual(1, _repository.ToDoItems.Count());
            Assert.AreEqual("Test Desc for delete", _repository.ToDoItems.First().Description);
        }
    }
}