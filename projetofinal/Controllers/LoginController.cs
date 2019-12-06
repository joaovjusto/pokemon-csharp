using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using projetofinal.Models;

namespace projetofinal.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> realizarLogin(String userName, String password)
        {
            string baseUrl = "https://localhost:44355/user/getuser";

            using (HttpClient client = new HttpClient())

            using (HttpResponseMessage res = await client.GetAsync(baseUrl))

            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {

                    data = data.ToString();

                    UserService users = new UserService();

                    users = JsonConvert.DeserializeObject<UserService>(data);

                    var usuarios = users.usuarios;

                    foreach(User u in usuarios)
                    {
                        if(u.nome == userName || u.senha == password)
                        {
                            return RedirectToAction("Index", "Home", new { username = u.nome });
                        }
                    };  

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}