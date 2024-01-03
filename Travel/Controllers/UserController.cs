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
    public class UserController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "D6qjehjMpkomM9rjqdvgPcHbVu7MCCFt4YcwVEx7",
            BasePath = "https://hosp-565bf-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        IFirebaseClient client;


        public IActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<UserModel>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<UserModel>(((JProperty)item).Value.ToString()));
                }
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(UserModel model)
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


        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + id);
            UserModel data = JsonConvert.DeserializeObject<UserModel>(response.Body);
            return View(data);
        }


        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Users/" + model.Id, model);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Users/" + id);
            return RedirectToAction("Index");
        }




    }
}
