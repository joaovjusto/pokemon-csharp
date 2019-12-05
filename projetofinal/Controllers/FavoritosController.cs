using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using projetofinal.Models;

namespace projetofinal.Controllers
{
    public class FavoritosController : Controller
    {
        static List<Pokemon> Lista { get; set; }

        public async Task<IActionResult> IndexAsync()
        {
            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var favoritos = await firebaseClient.Child("favoritos").OrderByKey().OnceAsync<Pokemon>();

            List<Pokemon> local = new List<Pokemon>();

            foreach (var poke in favoritos)
            {
                local.Add(new Pokemon()
                {
                    name = poke.Object.name,
                    url = poke.Object.url,
                    sprites = poke.Object.sprites,
                    user = poke.Object.user,
                    time = poke.Object.time,
                    habilidade = poke.Object.habilidade
                });
            }

            Lista = local;

            return View(local);
        }

        [HttpPost]
        public async Task<IActionResult> alterName(String userName, String nomeReal)
        {
            Pokemon poke = new Pokemon();

            poke = Lista.Find(item => item.name == nomeReal);

            poke.name = userName;

            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var favoritos = await firebaseClient.Child("favoritos").OrderByKey().OnceAsync<Pokemon>();

            foreach (var pokemon in favoritos)
            {
                if (nomeReal == pokemon.Object.name)
                {
                    await firebaseClient
                      .Child("favoritos")
                      .Child(pokemon.Key)
                      .DeleteAsync();

                    await firebaseClient
                     .Child("favoritos")
                        .PostAsync(poke);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> addTime(String name)
        {
            Pokemon poke = new Pokemon();

            List<Pokemon> model = Lista as List<Pokemon>;

            poke = model.Find(item => item.name == name);

            poke.time = true;

            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var favoritos = await firebaseClient.Child("favoritos").OrderByKey().OnceAsync<Pokemon>();

            foreach (var pokemon in favoritos)
            {
                if (poke.name == pokemon.Object.name)
                {
                    await firebaseClient
                      .Child("favoritos")
                      .Child(pokemon.Key)
                      .DeleteAsync();

                    await firebaseClient
                     .Child("favoritos")
                        .PostAsync(poke);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> removeFavorites(String name)
        {
            Pokemon poke = new Pokemon();

            List<Pokemon> model = Lista as List<Pokemon>;

            poke = model.Find(item => item.name == name);

            poke.time = true;

            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var favoritos = await firebaseClient.Child("favoritos").OrderByKey().OnceAsync<Pokemon>();

            foreach (var pokemon in favoritos)
            {
                if (poke.name == pokemon.Object.name)
                {
                    await firebaseClient
                      .Child("favoritos")
                      .Child(pokemon.Key)
                      .DeleteAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}