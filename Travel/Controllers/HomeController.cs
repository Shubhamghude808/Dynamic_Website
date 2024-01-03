using Consult.Models;
using Microsoft.AspNetCore.Mvc;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FireSharp.Response;

namespace Consult.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "D6qjehj9rjqdvgPcHbVu7MCCFt4YcwVEx7", //use your Auth ID
            BasePath = "https://hosp-565bf-default-rtdb.asia-southeast1.firebasedatabase.app"  //use your path link to your firebase database 
        };
        IFirebaseClient client;

        public IActionResult SignUp()
        {
            return View();
        }
        
        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public ActionResult Booking(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + id);
            UserModel data = JsonConvert.DeserializeObject<UserModel>(response.Body);
            return View(data);
        }
        [HttpPost]
        public IActionResult Booking(UserModel model)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var data = model;
                PushResponse response = client.Push("Users/", data);
                data.Id = response.Result.name;
                SetResponse setResponse = client.Set("Users/" + data.Id, data);
                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Booked Succesfully");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserModel model)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var data = model;
                PushResponse response = client.Push("News/", data);
                data.Id = response.Result.name;
                SetResponse setResponse = client.Set("News/" + data.Id, data);
                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Subscribed");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

    }
}