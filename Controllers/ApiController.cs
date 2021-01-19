using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recetario.BaseDatos;
using Recetario.Areas.Administradores.Servicios;
using Recetario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ApiController : ControllerBase
    {
        private readonly ContextoBD _contextoBD;
        private readonly IReceta _serviciosReceta;
        public ApiController(ContextoBD contextoBD, IReceta serviciosReceta)
        {
            _contextoBD = contextoBD;
            _serviciosReceta = serviciosReceta;
        }


        // GET: api/<ApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ApiController>/5
        [HttpGet("{IdReceta}")]
        public async Task<ActionResult<RecetaDTO>> Get(int IdReceta)
        {
            var receta = _serviciosReceta.Obtener(IdReceta);
            if(receta == null)
            {
                return NotFound();
            }
            return receta;
        }

        // POST api/<ApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
