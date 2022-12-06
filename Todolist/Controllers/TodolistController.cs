using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Todolist.Json;
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
                ViewBag.Error = "Data access error!";
                return View();
            }
        }

        public ActionResult PartialContent()
        {
            try
            {
                var tasks = _taskService.GetTasksPaging();
                return PartialView("_PartialContent", tasks);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Data access error!";
                return PartialView("_PartialContent");
            }
        }


        [HttpPost]
        public JsonResult TasksAsDataTables(JsonPostData jsonPostData)
        {
            try
            {
                string sortColumn = "";
                int sortColumnNumber = 0;
                string dir = "";
                string searchValue = jsonPostData.Search.Value;
                bool searchRegex = jsonPostData.Search.Regex;

                foreach (var orderRow in jsonPostData.Order)
                {
                    sortColumnNumber = orderRow.Column;
                    dir = orderRow.Dir;
                }
                sortColumn = jsonPostData.Columns[sortColumnNumber].Data;
                bool descending = (dir == "desc") ? true : false;
                int start = jsonPostData.Start;
                int length = jsonPostData.Length;
                var page = start / length + 1;
                var tasks = _taskService.GetTasksPaging(page, length, sortColumn, descending);
                int recordsTotal = tasks.PagingInfoVm.TotalItems;
                int recordsFiltered = recordsTotal;
                List<JsonData> data = new List<JsonData>();

                foreach (var tasksItem in tasks.TasksPage)
                {
                    JsonData dataRow = new JsonData
                    {
                        TaskDescription = tasksItem.TaskDescription,
                        EnrollmentDate = tasksItem.EnrollmentDate.ToString(string.Format("dd/MM/yyyy HH:mm")),
                        Approved = !tasksItem.Approved ? "In progress " : "Done",
                        Empty = "<a href=" + Url.Action("Edit", new { id = tasksItem.TodolistId }) + " class='ajaxLink'>Edit</a> | <a href=" +
                        Url.Action("Delete", new { id = tasksItem.TodolistId }) + " class='ajaxLink'>Delete</a>"
                    };
                    data.Add(dataRow);
                }
                return Json(new
                {
                    recordsTotal,
                    recordsFiltered,
                    data
                });
            }
            catch (Exception e)
            {
                return Json(new { ErrorMsg = "Data access error!" });
            }
        }

        [HttpPost]
        public JsonResult CheckCoincidences(string taskDescription, int taskId)
        {
            if (_taskService.SearchTaskDescription(taskDescription, taskId))
            {
                return Json(new { EnableError = true, ErrorMsg = "Task already exists, enter another name!" });
            }
            return Json(new { EnableSuccess = true, SuccessMsg = "" });
        }

        public ActionResult Create()
        {
            TaskVm taskVm = new TaskVm();
            taskVm.Title = "Task adding";
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
                    return Json(new { EnableError = true, ErrorMsg = "Task already exists, enter another name!" });
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
                return Json(new { EnableSuccess = true, SuccessMsg = "Task successfully created!" });
            }
            catch (Exception)
            {
                return Json(new { EnableError = true, ErrorMsg = "Something went wrong, try again or contact your system administrator!" });
            }
        }

        public ActionResult Edit(int id)
        {
            TaskVm taskVm = _taskService.Get(id);
            taskVm.Title = "Task editing";
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
                    return Json(new { EnableError = true, ErrorMsg = "Task already exists, enter another name!" });
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
                return Json(new { EnableSuccess = true, SuccessMsg = "Task successfully edited!" });
            }
            catch (Exception)
            {
                return Json(new { EnableError = true, ErrorMsg = "Something went wrong, try again or contact your system administrator!" });
            }
        }

        public ActionResult Delete(int id)
        {
            TaskVm taskVm = _taskService.Get(id);
            taskVm.Title = "Task deleting";
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
                return Json(new { EnableError = true, ErrorMsg = "Deletion did not, try again or contact your system administrator!" });
            }
            return Json(new { EnableSuccess = true, SuccessMsg = "Task successfully deleted!" });
        }
    }
}
