using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projetofinal.Models;

namespace projetofinal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        // GET: api/Battle
        [HttpGet]
        [Route("getOpponents")]
        public OpponentService GetAsync()
        {
            List<Opponent> items = new List<Opponent>();

            List<Pokemon> pokemons = new List<Pokemon>();

            Opponent opp = new Opponent();

            pokemons.Add(new Pokemon()
            {
                name = "Bulba",
                url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png",
                habilidade = new Random().Next(1, 13)
        });

            pokemons.Add(new Pokemon()
            {
                name = "Poke2",
                url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png",
                habilidade = new Random().Next(1, 13)
        });

            pokemons.Add(new Pokemon()
            {
                name = "Poke3",
                url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png",
                habilidade = new Random().Next(1, 13)
        });

            opp.name = "Opponent Test";
            opp.pokes = pokemons;

            items.Add(opp);

            OpponentService ops = new OpponentService();

            ops.opponents = items;

            return ops;
        }
    }
}
