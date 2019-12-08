using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace projetofinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("getuser")]
        public async Task<UserService> GetAsync()
        {
            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var usuarios = await firebaseClient.Child("usuarios").OrderByKey().OnceAsync<User>();

            List<User> items = new List<User>();

            foreach (var usuario in usuarios)
            {
                if (usuario.Object.tipo != "Funcionário")
                {
                    items.Add(new User()
                    {
                        nome = usuario.Object.nome,
                        idade = usuario.Object.idade,
                        senha = usuario.Object.senha,
                        usuario = usuario.Object.usuario,
                        tipo = usuario.Object.tipo
                    });
                }
            }

            UserService users = new UserService();

            users.usuarios = items;

            return users;
        }

        [HttpPost]
        [Route("Cadastrar")]
        public async Task<IActionResult> CadastrarAsync([FromBody] User p)
        {
            if (ModelState.IsValid)
            {
                if (await CadastrarUsuario(p))
                {
                    return Created("", p);
                }
                return Conflict(new { msg = "Esse Usuario já existe!" });
            }
            return BadRequest(ModelState);
        }

        public async Task<bool> BuscarUsuarioPorNome(User u)
        {

            var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

            var users = await firebaseClient.Child("usuarios").OrderByKey().OnceAsync<User>();

            Console.WriteLine($"{u.nome}");

            foreach (var user in users)
            {

                if (u.usuario == user.Object.usuario)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CadastrarUsuario(User u)
        {
            if (await BuscarUsuarioPorNome(u))
            {

                var firebaseClient = new FirebaseClient("https://pokesharp-219d8.firebaseio.com/");

                await firebaseClient
                 .Child("usuarios")
                .PostAsync(u);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
