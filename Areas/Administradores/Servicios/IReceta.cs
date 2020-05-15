﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Areas.Administradores.Models;

namespace Recetario.Areas.Administradores.Servicios
{
    /// <summary>
    /// Interfaz encargada de todas las posibles acciones que se pueden realizar con una clase VReceta
    /// </summary>
    /// <see cref="VReceta"/>
    public interface IReceta
    {
        /// <summary>
        /// Con base a un ID dado, regresa la VReceta correspondiente
        /// </summary>
        /// <param name="Id">ID de VReceta</param>
        /// <returns>Clase VReceta</returns>
        VReceta Obtener(int? Id);

        /// <summary>
        /// Obtiene la lista de VReceta disponibles en un repositorio.
        /// </summary>
        /// <returns>Lista de VReceta</returns>
        ICollection<VReceta> Obtener();

        /// <summary>
        /// Eliminar una receta del repositorio mediante su ID
        /// </summary>
        /// <param name="IdReceta"></param>
        void Eliminar(int Id);

        /// <summary>
        /// Realiza una búsqueda con base en un filtro dado
        /// La busqueda se hace por Nombre o usuario que la creó
        /// </summary>
        /// <param name="Filtro">Filtro de búsqueda</param>
        /// <returns>Una lista de recetas que coinciden con el filtro</returns>
        ICollection<VReceta> BuscarFiltro(String Filtro);
    }
}