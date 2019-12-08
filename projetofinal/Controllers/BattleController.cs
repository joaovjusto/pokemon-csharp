using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using System.Net.Http;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using projetofinal.Models;
using Newtonsoft.Json;

namespace projetofinal.Controllers
{
    public class BattleController : Controller
    {

        static String usuario { get; set; }
        static List<Pokemon> Lista { get; set; }

        public async Task<IActionResult> IndexAsync()
        {
            //Verificando Usuário Logado

            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var usuarioLogado = await firebaseClient.Child("usuarioLogado").OrderByKey().OnceAsync<UsuarioLogado>();

            foreach (var user in usuarioLogado)
            {
                usuario = user.Object.name;
            }

            //Verificando pokemons do time do usuário logado

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

            //Trazendo Pokémons do oponente

            string baseUrl = "https://localhost:44355/battle/getOpponents";

            using (HttpClient client = new HttpClient())

            using (HttpResponseMessage res = await client.GetAsync(baseUrl))

            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {

                    data = data.ToString();

                    OpponentService ops = new OpponentService();

                    ops = JsonConvert.DeserializeObject<OpponentService>(data);

                    var oponentes = ops.opponents;

                    List<Pokemon> pokelist = new List<Pokemon>();

                    foreach(Opponent o in oponentes)
                    {
                        o.pokes = pokelist;
                    }

                    Int32 pontos = 0;

                    int i = 0;
                    foreach (Pokemon pika in pokelist)
                    {
                        if (pika.habilidade <= Lista[i].habilidade) {
                            pontos += 1; 
                        }
                    }

                    if(pontos <= 2)
                    {
                        ViewData["Result"] = "Parabéns Você venceu";
                    }else
                    {
                        ViewData["Result"] = "Você perdeu";
                    }

                    return View();
                }
            }       

            return View();
        }
    }
}