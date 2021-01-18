using Microsoft.EntityFrameworkCore;
using Recetario.BaseDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Servicios
{
    public class ServiciosGenerales : IGeneral
    {
        private readonly ContextoBD _contextoBD;
        public ServiciosGenerales(ContextoBD contextoBD)
        {
            _contextoBD = contextoBD;
        }
        public ICollection<string> IngredientesFavoritos(int IdUsuario)
        {
            //Obtener los Ids de recetas
            List<int> recetas = _contextoBD.Visualizacion
                .Include(v => v.Receta)
                .Where(r => r.Calificacion == true && r.ActorIdActor == IdUsuario)
                .Select(r => r.RecetaIdReceta).ToList();
            var ingredientes = new List<string>();
            //Obtener los ingredientes de las recetas
            foreach (var idReceta in recetas)
            {
                //Obtener ingrediente de c/u de las recetas
                var ingreReceta = _contextoBD.Lleva
                    .Include(l => l.IngredienteIdIngredienteNavigation)
                .Where(l => l.RecetaIdReceta == idReceta)
                .Select(l => l.IngredienteIdIngredienteNavigation.Nombre)
                .ToList();
                //Agregar a la lista global
                ingredientes.AddRange(ingreReceta);
            }
            return ingredientes;
        }
    }
}
