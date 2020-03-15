using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IPWService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IPWAdmin.Controllers
{
    public class UsersController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "http://localhost:29377/api/User/";
        // GET: Users
        public async Task<ActionResult> Index()
        {
            List<Users> userInfo = new List<Users>();

            using (var client = new HttpClient())
            {
                //passing service base url
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                //define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //sending request to find web api REST service resource GetUsers using HttpClient
                HttpResponseMessage res = await client.GetAsync("GetUsers");

                //checking the response is successful or not which is sent using HttpClient
                if (res.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api
                    var userResponse = res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the users list
                    userInfo = JsonConvert.DeserializeObject<List<Users>>(userResponse);
                }

            }
            //returning the user list to view
            return View(userInfo);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //if(id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Users user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var result = await client.GetAsync($"GetUser/{id}");
                if (result.IsSuccessStatusCode)
                {
                    user = await result.Content.ReadAsAsync<Users>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            //if(user == null)
            //{
            //    return HttpNotFound();
            //}
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        var response = await client.PostAsJsonAsync("AddUser", collection);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                }
                return View(collection);
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Users user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var result = await client.GetAsync($"Edit/{id}");
                if (result.IsSuccessStatusCode)
                {
                    user = await result.Content.ReadAsAsync<Users>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        var response = await client.PutAsJsonAsync($"UpdateUser/{id}", collection);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(collection);
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Users user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"DeleteUser/{id}");

                if (result.IsSuccessStatusCode)
                {
                    user = await result.Content.ReadAsAsync<Users>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            //if (user == null)
            //{
            //    return HttpNotFound();
            //}
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        var response = await client.PutAsJsonAsync($"DeleteUser/{id}", collection);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(collection);
            }
            catch
            {
                return View();
            }
        }
    }
}