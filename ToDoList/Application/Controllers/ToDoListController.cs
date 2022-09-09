using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ToDoList.Application.Services;
using ToDoList.Infrastructure.Data.Entities;

namespace ToDoList.Application.Controllers
{
    public class ToDoListController : Controller
    {

        private const string SessionUserName = "_UserName";
        private IToDoListService _toDoListService { get; }
        private readonly ILogger<ToDoListController> _logger;

        public ToDoListController(IToDoListService toDoListService, ILogger<ToDoListController> logger)
        {
            _toDoListService = toDoListService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new User();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(User model)
        {
            if (model == null)
                _logger.LogError("login failed.");
            var loggedUser = _toDoListService.GetUser(model.UserName, model.Password);
            if (loggedUser == null)
            {
                _logger.LogError("Not found.");
                return View(model);
            }

            HttpContext.Session.SetString(SessionUserName, loggedUser.UserName);

            return RedirectToActionPermanent("List");
        }

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                var loggedUser = _toDoListService.GetUserByUsername(HttpContext.Session.GetString(SessionUserName));
                if (loggedUser == null)
                {
                    _logger.LogError("No user found.");
                    return RedirectToActionPermanent("Index");
                }

                var toDoList = _toDoListService.GetToDoItems(loggedUser);
                foreach (var item in toDoList)
                {
                    item.User = loggedUser;
                }

                if (toDoList == null || toDoList.Count == 0)
                    ViewBag.Message = $"No items for {loggedUser.UserName}";
                return View(toDoList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred when loading items. {ex.Message}");
                return View("Error");
            }
            return View("Error");
        }

        [HttpPost]
        public IActionResult AddItem(ToDoItem item)
        {
            _logger.LogInformation($"Start: {nameof(AddItem)}.");
            if (item == null)
                _logger.LogError("Error occurred.");

            try
            {
                item.UpdatedOn = DateTime.Now;
                var loggedUser = _toDoListService.GetUserByUsername(HttpContext.Session.GetString(SessionUserName));
                item.User = loggedUser;
                item.Done = false;
                _toDoListService.AddToDoItem(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred when adding item. {ex.Message}");
                return View("Error");
            }
            finally
            {
                _logger.LogInformation($"End: {nameof(AddItem)}.");
            }

            return RedirectToActionPermanent("List", "ToDoList", item.User);
        }

        [HttpPost]
        public IActionResult RemoveItem(int id)
        {
            _logger.LogInformation($"Start: {nameof(RemoveItem)}.");
            var toDoItem = new ToDoItem();

            try
            {
                toDoItem = _toDoListService.GetToDoItemById(id);
                List<ToDoItem> toDoItems = new List<ToDoItem>();
                toDoItems.Add(toDoItem);
                _toDoListService.RemoveItem(toDoItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred when deleting item. {ex.Message}");
                return View("Error");
            }
            finally
            {
                _logger.LogInformation($"End: {nameof(AddItem)}.");
            }

            return RedirectToActionPermanent("List", "ToDoList", toDoItem?.User);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionUserName);
            HttpContext.Session.Clear();

            return RedirectToActionPermanent("Index");
        }

    }
}
