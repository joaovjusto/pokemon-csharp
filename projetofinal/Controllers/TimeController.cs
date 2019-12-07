using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using projetofinal.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace projetofinal.Controllers
{
    public class TimeController : Controller
    {
        static List<Pokemon> Lista { get; set; }

        static String usuario { get; set; }

        public async Task<IActionResult> IndexAsync()
        {
            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var usuarioLogado = await firebaseClient.Child("usuarioLogado").OrderByKey().OnceAsync<UsuarioLogado>();

            foreach (var user in usuarioLogado)
            {
                usuario = user.Object.name;
            }

            var favoritos = await firebaseClient.Child("favoritos").OrderByKey().OnceAsync<Pokemon>();

            List<Pokemon> local = new List<Pokemon>();

            foreach (var poke in favoritos)
            {
                if (poke.Object.user == usuario && poke.Object.time)
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
            }

            Lista = local;

            return View(local);
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