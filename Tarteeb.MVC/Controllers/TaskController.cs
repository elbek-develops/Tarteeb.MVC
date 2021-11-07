using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Tarteeb.MVC.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Tarteeb.MVC.Controllers
{
    public class TaskController : Controller
    {

        private readonly ILogger<TaskController> logger;
        private readonly IConfiguration configuration;
        private readonly string baseTasksUrl;

        public TaskController(ILogger<TaskController> logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration;
            baseTasksUrl = configuration.GetSection("HostNames").GetSection("TasksHost").Value;
        }

        public async Task<IActionResult> Index()
        {
            //Hosted web API REST Service base url
            List<TaskViewModel> tasks = new List<TaskViewModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(baseTasksUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees  using HttpClient(
                HttpResponseMessage response = await client.GetAsync("api/Tasks");
                //Checking the response is successful or not which is sent using
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var tasksResponse = response.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into  the TaskViewModel list
                    tasks = JsonConvert.DeserializeObject<List<TaskViewModel>>(tasksResponse);
                }
                //returning the TaskViewModel list to view
                return View(tasks);
            }
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            ViewData["Title"] = "Create";
            return View();
        }

        // POST: taskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaskViewModel taskViewModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = baseTasksUrl + "api/Tasks";
                    var postTask = await client.PostAsJsonAsync<TaskViewModel>(url, taskViewModel);
                    if (!postTask.IsSuccessStatusCode)
                    {
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured on create task:" + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: taskController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            TaskViewModel task = new TaskViewModel();
            using (var client = new HttpClient())
            {
                //Passing service base url
                string endpoint = "api/Tasks/" + id;
                client.BaseAddress = new Uri(baseTasksUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var taskResponse = await response.Content.ReadAsStringAsync();
                    //Deserializing the response recieved from web api and storing into  the TaskViewModel list
                    task = JsonConvert.DeserializeObject<TaskViewModel>(taskResponse);
                }
                //returning the TaskViewModel list to view
                ViewData["Title"] = "Update";
                return View("Create", task);
            }
        }

        // PUT: taskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TaskViewModel taskViewModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = baseTasksUrl + "api/Tasks";
                    var putTask = await client.PutAsJsonAsync<TaskViewModel>(url, taskViewModel);
                    if (!putTask.IsSuccessStatusCode)
                    {
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured on update task:" + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // DELETE: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = baseTasksUrl + "api/Tasks/" + id;
                    var deleteTask = await client.DeleteAsync(url);
                    if (!deleteTask.IsSuccessStatusCode)
                    {
                        logger.LogError("Error occured on delete task.");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                logger.LogError("Error occured on delete task.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
