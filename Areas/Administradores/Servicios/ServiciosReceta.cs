﻿using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Recetario.Areas.Administradores.Servicios
{
    public class ServiciosReceta : IReceta
    {
        private readonly ContextoBD _contextoBD;
        public ServiciosReceta(ContextoBD contextoBD)
        {
            _contextoBD = contextoBD;
        }
        /// <inheritdoc/>
        public ICollection<RecetaDTO> BuscarFiltro(string Filtro)
        {
            int idCreador = 0;
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();

            //Buscar posible usuario creador en el filtro
            var creador = _contextoBD.Actor.FirstOrDefault(a =>
            a.NombreActor.ToLower().Contains(Filtro));
            //Obtener Ids del usuario creador
            if(creador != null)
                idCreador = creador.Id;

            //Buscar en la base de datos las recetas que conincidan con el filtro
            var recetas = _contextoBD.Receta.Where(r =>
            r.Nombre.ToLower().Contains(Filtro) ||
            r.ActorIdActor == idCreador);
            //Se convierte el IQueryable en lista de recetas
            List<Receta> listaRecetas = recetas.ToList();
            //Una lista para guardar las vistas que se van a regresar
            List<RecetaDTO> vrecetas = new List<RecetaDTO>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Receta receta in listaRecetas)
            {
                vrecetas.Add(CasteoVReceta(receta));
            }
            return vrecetas;
        }

        /// <inheritdoc/>
        public void Eliminar(int Id)
        {
            //Traer a la receta y sus tablas dependientes de la BD
            var receta = _contextoBD.Receta
                .Include(r => r.Usa)
                .Include(r => r.Lleva)
                .Include(r=> r.Paso)
                .FirstOrDefault(r => r.IdReceta == Id)
                ;
            //Sacarla del contexto
            _contextoBD.Receta.Remove(receta);
            //Aplicar los cambios a la BD
            _contextoBD.SaveChanges();
        }

        /// <inheritdoc/>
        public RecetaDTO Obtener(int Id)
        {
            return CasteoVReceta(_contextoBD.Receta.FirstOrDefault(r => r.IdReceta == Id));
        }

        /// <inheritdoc/>
        public ICollection<RecetaDTO> Obtener()
        {
            var recetas = _contextoBD.Receta.ToList();
            List<RecetaDTO> vrecetas = new List<RecetaDTO>();
            foreach (Receta receta in recetas) vrecetas.Add(CasteoVReceta(receta));
            return vrecetas;
        }
        
        public RecetaDTO CasteoVReceta(Receta receta)
        {
            return new RecetaDTO
            {
                IdReceta = receta.IdReceta,
                Nombre = receta.Nombre,
                ProcentajePromedio = receta.ProcentajePromedio,
                TiempoPrep = receta.TiempoPrep,
                ActorIdActor = receta.ActorIdActor,
                ActorNombreActor = _contextoBD.Actor.Find(receta.ActorIdActor).NombreActor
            };
        }
    }
}
