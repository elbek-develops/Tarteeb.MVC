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
    public class TimeController : Controller
    {
        private readonly string baseTimesUrl;
        private readonly string baseTasksUrl;
        private readonly ILogger<TimeController> logger;
        private readonly IConfiguration configuration;

        public TimeController(ILogger<TimeController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            baseTasksUrl = configuration.GetSection("HostNames").GetSection("TasksHost").Value;
            baseTimesUrl = configuration.GetSection("HostNames").GetSection("TimesHost").Value;
        }

        public async Task<IActionResult> Index()
        {
            List<TimeViewModel> times = new List<TimeViewModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(baseTimesUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees  using HttpClient(
                HttpResponseMessage response = await client.GetAsync("api/Times");
                //Checking the response is successful or not which is sent using
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var timesResponse = response.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into  the TimeViewModel list
                    times = JsonConvert.DeserializeObject<List<TimeViewModel>>(timesResponse);
                }
                //returning the TimeViewModel list to view
                return View(times);
            }
        }

        // GET: TimeController/Create
        public async Task<ActionResult> Create()
        {
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
                ViewBag.Tasks = tasks;
                ViewData["Title"] = "Create";
            }
            return View();
        }

        // POST: TimeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TimeViewModel timeViewModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = baseTimesUrl + "api/Times";
                    var postTask = await client.PostAsJsonAsync<TimeViewModel>(url, timeViewModel);
                    if (!postTask.IsSuccessStatusCode)
                    {
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured on create time:" + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: TimeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            #region Get Tasks
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
                ViewBag.Tasks = tasks;
                ViewData["Title"] = "Update";
            }
            #endregion
            TimeViewModel time = new TimeViewModel();
            using (var client = new HttpClient())
            {
                //Passing service base url
                string endpoint = "api/Times/" + id;
                client.BaseAddress = new Uri(baseTimesUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var timeResponse = await response.Content.ReadAsStringAsync();
                    //Deserializing the response recieved from web api and storing into  the TimeViewModel list
                    time = JsonConvert.DeserializeObject<TimeViewModel>(timeResponse);
                }
                //returning the TimeViewModel list to view
                return View("Create",time);
            }
        }

        // PUT: TimeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,TimeViewModel timeViewModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = baseTimesUrl + "api/Times";
                    var putTask = await client.PutAsJsonAsync<TimeViewModel>(url, timeViewModel);
                    if (!putTask.IsSuccessStatusCode)
                    {
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured on update time:" + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // DELETE: TimeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = baseTimesUrl + "api/Times/" + id;
                    var deleteTask = await client.DeleteAsync(url);
                    if (!deleteTask.IsSuccessStatusCode)
                    {
                        logger.LogError("Error occured on delete time.");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                logger.LogError("Error occured on delete time.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
