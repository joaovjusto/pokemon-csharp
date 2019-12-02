using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using projetofinal.Models;

namespace projetofinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {

            string baseUrl = "https://pokeapi.co/api/v2/pokemon?offset=0&limit=40";

            using (HttpClient client = new HttpClient())

            using (HttpResponseMessage res = await client.GetAsync(baseUrl))

            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {

                    data = data.ToString();

                    var pokemon = new ServiceClass();

                    pokemon = JsonConvert.DeserializeObject<ServiceClass>(data);

                    var TEMP = pokemon.results;

                    List<Pokemon> items = new List<Pokemon>();

                    foreach (Pokemon poke in TEMP)
                    {
                        var pokemonn = new Pokemon();

                        pokemonn = await getPerId(poke);

                        items.Add(pokemonn);
                    }

                    return View(items);
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
