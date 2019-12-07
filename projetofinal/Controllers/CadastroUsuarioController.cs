using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using projetofinal.Models;

namespace projetofinal.Controllers
{
    public class CadastroUsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> realizarLogin(String userName, String password, String nome, Int32 idade)
        {
            User teste = new User();
            teste.idade = idade;
            teste.usuario = userName;
            teste.nome = nome;
            teste.senha = password;

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44355");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonContent = JsonConvert.SerializeObject(teste);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new
            MediaTypeHeaderValue("application/json");


            HttpResponseMessage response = await client.PostAsync("/user/cadastrar", contentString);

            return RedirectToAction("Index", "Login");
        }
    }
}