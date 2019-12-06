using System;
using System.Collections.Generic;
using System.Diagnostics;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using projetofinal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace projetofinal.Controllers
{
    public class HomeController : Controller
    {
        static List<Pokemon> Lista { get; set; }
        static String usuario { get; set; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync(string username)
        {
            usuario = username;

            string baseUrl = "https://pokeapi.co/api/v2/pokemon?offset=0&limit=40";

            using (HttpClient client = new HttpClient())

            using (HttpResponseMessage res = await client.GetAsync(baseUrl))

            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {

                    data = data.ToString();

                    ServiceClass pokemon = new ServiceClass();

                    pokemon = JsonConvert.DeserializeObject<ServiceClass>(data);

                    var TEMP = pokemon.results;

                    List<Pokemon> local = new List<Pokemon>();

                    foreach (Pokemon poke in TEMP)
                    {
                        Pokemon pokemonn = new Pokemon();

                        pokemonn = await getPerId(poke);

                        local.Add(pokemonn);
                    }

                    //TempData["items"] = local;

                    //ViewBag.items = local;

                    Lista = local;

                    return View(local);
                }
            }
            return View();
        }

        public async Task<Pokemon> getPerId(Pokemon poke)
        {
            string baseUrl = poke.url;

            using (HttpClient client = new HttpClient())

            using (HttpResponseMessage res = await client.GetAsync(baseUrl))

            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();

                data = data.ToString();

                var pokemon = new Pokemon();

                pokemon = JsonConvert.DeserializeObject<Pokemon>(data);

                return pokemon;
            }
        }

        public async Task<IActionResult> addFavorites(String name)
        {
            Pokemon poke = new Pokemon();

            List<Pokemon> model = Lista as List<Pokemon>;

            poke = model.Find(item => item.name == name);

            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            await firebaseClient
                .Child("favoritos")
            .PostAsync(poke);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
