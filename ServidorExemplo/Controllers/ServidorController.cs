using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServidorExemplo.Models;
using ServidorExemplo.Data;
using System.Threading.Tasks;
using MySqlConnector;
using System.Diagnostics;

namespace ServidorExemplo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServidorController : ControllerBase
    {
        private readonly ApiContext _context;

        public ServidorController(ApiContext context) 
        { 
            _context = context;
        }

        static async Task Chamada()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "SERVIDOR.mysql.database.azure.com",
                Database = "NOME BANCO ",
                UserID = "",
                Password = "",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Debug.Write("Opening connection");
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO usuarios (id, nome, admin) VALUES (123, ""teobaldo"", 0)";
                    int rowCount = await command.ExecuteNonQueryAsync();
                    Debug.Write(String.Format("Number of rows inserted={0}", rowCount));
                }

            }


        }

            //Criar/Editar Post/Put
            [HttpPost]
        public JsonResult CriarEditar(Usuario usuario) 
        {
            if(usuario.ID == 0)
            {
                _context.usuarios.Add(usuario);
            }else
            {
                var usuarioNoBD = _context.usuarios.Find(usuario.ID);
                if(usuarioNoBD == null)
                {
                    return new JsonResult(NotFound());
                }
                usuarioNoBD = usuario;
            }
            _context.SaveChanges();
            return new JsonResult(Ok(usuario));
        }
        //Pegar GET
        [HttpGet]
        public JsonResult Pegar(int id)
        {
            var result = _context.usuarios.Find(id);
            if(result == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));
        }
        
        //Deletar
        [HttpDelete]
        public JsonResult Deletar(int id) 
        {
            var result = _context.usuarios.Find(id);

            if(result == null)
            {
                return new JsonResult(NotFound());
            }
            _context.usuarios.Remove(result);
            _context.SaveChanges();
            return new JsonResult(NoContent());
        }
        
        //Pegar todos os dados
        [HttpGet("/GetAll")]
        public JsonResult Todos() 
        { 
            var result = _context.usuarios.ToList();
            return new JsonResult(Ok(result));
        }

        [HttpGet("/Mysql")]
        async public void Banco()
        {
            await Chamada();
        }
        
    }
}
