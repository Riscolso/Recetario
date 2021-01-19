using Microsoft.AspNetCore.Mvc;
using Recetario.Areas.Administradores.Servicios;
using Recetario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recetario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaSPAController : ControllerBase
    {
        private readonly IReceta _serviciosReceta;

        public RecetaSPAController(IReceta serviciosReceta)
        {
            _serviciosReceta = serviciosReceta;
        }
        // GET: api/<ValuesController>
        [HttpGet("ObtenerReceta/{idReceta}")]
        public RecetaDTO ObtenerReceta(int IdReceta)
        {
            return _serviciosReceta.Obtener(IdReceta);
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        /*[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
