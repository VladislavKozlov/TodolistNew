using System;
using System.Linq;
using System.Web.Mvc;
using Todolist.Models;
using Todolist.Services;
using Todolist.ViewModels;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.Controllers
{
    public class TodolistController : Controller
    {
        private readonly ITaskService _taskService;

        public TodolistController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public ActionResult Index()
        {
            try
            {
                var tasks = _taskService.GetTasks();
                return View(tasks);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ошибка доступа к данным!";
                return View();
            }
        }

        public ActionResult PartialContent(string sortColumn = "", bool descending = false)
        {
            try
            {
                var tasks = _taskService.GetTasks(sortColumn, descending);
                return PartialView("_PartialContent", tasks);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ошибка доступа к данным!";
                return PartialView("_PartialContent");
            }
        }

        [HttpPost]
        public JsonResult CheckCoincidences(string taskDescription, int taskId)
        {
            if (_taskService.SearchTaskDescription(taskDescription, taskId))
            {
                return Json(new { EnableError = true, ErrorMsg = "Такая задача уже существует, введите другое название!" });
            }
            return Json(new { EnableSuccess = true, SuccessMsg = "" });
        }

        public ActionResult Create()
        {
            TaskVm taskVm = new TaskVm();
            taskVm.Title = "Добавление задачи";
            return PartialView("_Create", taskVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(TaskInput taskInput)
        {
            try
            {
                if (_taskService.SearchTaskDescription(taskInput.TaskDescription, 0))
                {
                    return Json(new { EnableError = true, ErrorMsg = "Такая задача уже существует, введите другое название!" });
                }
                if (ModelState.IsValid)
                {
                    _taskService.Add(taskInput);
                }
                else
                {
                    string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0).SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());
                    return Json(new { EnableError = true, ErrorMsg = validationErrors });
                }
                return Json(new { EnableSuccess = true, SuccessMsg = "Задача успешно создана!" });
            }
            catch (Exception)
            {
                return Json(new { EnableError = true, ErrorMsg = "Что-то идет неправильно, попробуйте ещё раз или обратитесь к системному администратору!" });
            }
        }

        public ActionResult Edit(int id)
        {
            TaskVm taskVm = _taskService.Get(id);
            taskVm.Title = "Редактирование задачи";
            return PartialView("_Edit", taskVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(TaskInput taskInput)
        {
            try
            {
                if (_taskService.SearchTaskDescription(taskInput.TaskDescription, taskInput.TodolistId))
                {
                    return Json(new { EnableError = true, ErrorMsg = "Такая задача уже существует, введите другое название!" });
                }
                if (ModelState.IsValid)
                {
                    _taskService.Edit(taskInput);
                }
                else
                {
                    string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0).SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());
                    return Json(new { EnableError = true, ErrorMsg = validationErrors });
                }
                return Json(new { EnableSuccess = true, SuccessMsg = "Задача успешно отредактирована!" });
            }
            catch (Exception)
            {
                return Json(new { EnableError = true, ErrorMsg = "Что-то идет неправильно, попробуйте ещё раз или обратитесь к системному администратору!" });
            }
        }

        public ActionResult Delete(int id)
        {
            TaskVm taskVm = _taskService.Get(id);
            taskVm.Title = "Удаление задачи";
            return PartialView("_Delete", taskVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteTask(int id)
        {
            try
            {
                _taskService.Remove(id);
            }
            catch (Exception)
            {
                return Json(new { EnableError = true, ErrorMsg = "Удаление не произошло, попробуйте ещё раз или обратитесь к системному администратору!" });
            }
            return Json(new { EnableSuccess = true, SuccessMsg = "Задача успешно удалена!" });
        }
    }
}
