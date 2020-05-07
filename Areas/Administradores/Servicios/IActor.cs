using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Areas.Administradores.Models;

namespace Recetario.Areas.Administradores.Servicios
{
    /// <summary>
    /// Interfaz encargada de todas las posibles acciones que se pueden realizar con una clase VActor
    /// </summary>
    /// <see cref="VActor"/>
    public interface IActor
    {
        /// <summary>
        /// Registra un actor en la Base de datos
        /// </summary>
        /// <param name="vactor">Clase VActor que se va a guardar</param>
        /// <returns>ID con el que se registró el actor dado</returns>
        int Registrar(VActor vactor);
        /// <summary>
        /// Con base a un ID dado, regresa un Vactor
        /// </summary>
        /// <param name="Id">ID de VActor</param>
        /// <returns>Clase VActor</returns>
        VActor Obtener(int? Id);
        /// <summary>
        /// Realiza una actualización de un VActor
        /// </summary>
        /// <param name="vactor">Clase a actualizar</param>
        /// <returns>ID Asocioado a la clase actualizada</returns>
        int Actualizar(VActor vactor);
        /// <summary>
        /// Obtiene una lista de VActor disponibles en un repositorio.
        /// </summary>
        /// <returns>Lista de VActor</returns>
        ICollection<VActor> Obtener();
    }
}
