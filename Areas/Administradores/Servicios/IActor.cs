using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Areas.Administradores.Models;
using Recetario.BaseDatos;

namespace Recetario.Areas.Administradores.Servicios
{
    /// <summary>
    /// Interfaz encargada de todas las posibles acciones que se pueden realizar con una clase VActor
    /// </summary>
    /// <see cref="ActorDTO"/>
    public interface IActor
    {
        /// <summary>
        /// Registra un actor en la Base de datos
        /// </summary>
        /// <param name="vactor">Clase VActor que se va a guardar</param>
        /// <returns>ID con el que se registró el actor dado</returns>
        int Registrar(ActorDTO vactor);

        /// <summary>
        /// Con base a un ID dado, regresa un Vactor
        /// </summary>
        /// <param name="Id">ID de VActor</param>
        /// <returns>Clase VActor</returns>
        ActorDTO Obtener(int? Id);
        /// <summary>
        /// Obtiene una lista de VActor disponibles en un repositorio.
        /// </summary>
        /// <returns>Lista de VActor</returns>
        ICollection<ActorDTO> ObtenerLista();
        /// <summary>
        /// Obtiene una lista de VActor disponibles en un repositorio.
        /// </summary>
        /// <param name="Tipo">Filtro con base al tipo de actor en la BD
        /// Usuario, Administrador o SuperAdministrador</param>
        /// <returns>Lista de VActor</returns>
        ICollection<ActorDTO> ObtenerLista(string rol);
        /// <summary>
        /// Regresa una lista de todos los usuarios VActor
        /// </summary>
        /// <returns>Lista de VActor</returns>
        /// 
        ICollection<ActorDTO> ObtenerUsuarios();

        /// <summary>
        /// Realiza una actualización de un VActor
        /// </summary>
        /// <param name="vactor">Clase a actualizar</param>
        /// <returns>ID Asocioado a la clase actualizada</returns>
        int Actualizar(ActorDTO vactor);
        

        /// <summary>
        /// Eliminar un actor del repositorio mediante su ID
        /// </summary>
        /// <param name="vactor"></param>
        void Eliminar(int Id);

        /// <summary>
        /// Realiza una búsqueda con base a un filtro dato
        /// La busqueda se da entre Nombre, correo o usuario
        /// </summary>
        /// <param name="Filtro">Filtro de búsqueda</param>
        /// <returns>Una lista de actores que coinciden con el filtro</returns>
        ICollection<ActorDTO> BuscarFiltro(String Filtro);

        /// <summary>
        /// Realiza una búsqueda con base a un filtro dato
        /// La busqueda se da entre Nombre, correo o usuario
        /// </summary>
        /// <param name="Filtro">Filtro de búsqueda</param>
        /// <param name="Tipo">Realiza Filtro de con base al tipo de actor </param>
        /// <returns>Una lista de actores que coinciden con el filtro</returns>
        ICollection<ActorDTO> BuscarFiltro(String Filtro, int Tipo);
        /// <summary>
        /// Realiza una búsqueda de usuario con base en un filtro dado
        /// La busqueda se da entre Nombre, correo o usuario
        /// </summary>
        /// <param name="Filtro">Filtro de búsqueda</param>
        /// <returns>Una lista de actores que coinciden con el filtro</returns>
        ICollection<ActorDTO> BuscarFiltroUsuarios(String Filtro);
        /// <summary>
        /// Regresa un VActor dado un id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActorDTO FindVActor(string user);
    }
}
