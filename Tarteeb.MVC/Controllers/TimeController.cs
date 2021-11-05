using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Tarteeb.MVC.Models;

namespace Tarteeb.MVC.Controllers
{
    public class TimeController : Controller
    {
        private const string baseUrl = "http://localhost:5050/";


        public async Task<IActionResult> Index()
        {
            //Hosted web API REST Service base url
            List<TimeViewModel> times = new List<TimeViewModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(baseUrl);
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
    }
}
