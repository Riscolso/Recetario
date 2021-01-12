using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Models;
using Recetario.Areas.Usuarios.Models;

namespace Recetario.Areas.Administradores.Servicios
{
    /// <summary>
    /// Interfaz encargada de todas las posibles acciones que se pueden realizar con una clase VReceta
    /// </summary>
    /// <see cref="RecetaDTO"/>
    public interface IReceta
    {
        int Agregar(RecetaDTO receta);
        /// <summary>
        /// Con base a un ID dado, regresa la VReceta correspondiente
        /// </summary>
        /// <param name="Id">ID de VReceta</param>
        /// <returns>Clase VReceta</returns>
        RecetaDTO Obtener(int Id);
        RecetaModelo Obtener(int IdReceta, int IdUsuario);

        /// <summary>
        /// Obtiene la lista de VReceta disponibles en un repositorio.
        /// </summary>
        /// <returns>Lista de VReceta</returns>
        ICollection<RecetaDTO> Obtener();
        ICollection<RecetaDTO> ObtenerXUsuario(int IdUsuario);

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
        ICollection<RecetaDTO> BuscarFiltro(String Filtro);

        ICollection<RecetaDTO> BuscarFiltro(string Filtro, int IdUsuario);

        int Editar(RecetaDTO recetadto);
        void Calificar(int IdReceta, bool Gustar, int Idusuario);
        void ListarCocinarDespues(int IdReceta, bool Estado, int IdUsuario);
        ICollection<RecetaDTO> ObtenerPendientes(int IdUsuario);
        ICollection<RecetaDTO> ObtenerPendientes(int IdUsuario, string Filtro);
    }
}
