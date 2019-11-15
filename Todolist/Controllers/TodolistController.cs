using Newtonsoft.Json.Linq;
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
                var tasks = _taskService.GetTasksPaging();
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

        public ActionResult PartialContentTest(int start, int length)
        {
            try
            {
                int page = 1;
                if (start != 0)
                {
                    page = start / length;
                }
                var tasks = _taskService.GetTasksPaging(page, length);
                return PartialView("_PartialContentTest", tasks);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ошибка доступа к данным!";
                return PartialView("_PartialContenTest");
            }
        }

        public JsonResult DataPagination(int start, int length)
        {
            try
            {
                int page = 0;
                if (start == 0)
                {
                    page = 1;
                }
                if (start == length)
                {
                    page = 2;
                }
                if (start > length)
                {
                    page = start / length + 1;
                }
                //string data = JsonConvert.SerializeObject(tasks.TasksPage);
                var tasks = _taskService.GetTasksPaging(page, length);
                //int recordsTotal = tasks.PagingInfoVm.TotalItems;                

                JArray jsonData = new JArray();
                foreach (var tasksItem in tasks.TasksPage)
                {
                    jsonData.Add(tasksItem.TaskDescription);
                    jsonData.Add(tasksItem.EnrollmentDate);
                    jsonData.Add(!tasksItem.Approved ? "В процессе" : "Решена");
                }
                int recordsTotal = jsonData.Count();
                int recordsFiltered = recordsTotal;
                JObject jObject = new JObject();
                jObject["data"] = jsonData;
                string data = jObject.ToString();

                return Json(new
                {
                    recordsTotal,
                    recordsFiltered,
                    data
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { ErrorMsg = "Ошибка доступа к БД!" });
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
