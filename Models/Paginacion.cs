using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Recetario.Models{
    /// <summary>
    /// Clase la cual permite y facilita la paginación al mostrar Vistas
    /// </summary>
    /// <see cref="https://docs.microsoft.com/es-es/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-3.1"/>
    /// <typeparam name="T">Lista de Clases de vista</typeparam>
    public class Paginacion<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public Paginacion(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            //Adds the elements of the specified collection to the end of the List<T>.
            this.AddRange(items);
        }
        /*
         * Agarrate, por que funciona así
         * Estos dos métodos lo único que hacen en retornar un True su lo que dice su nombre es cierto
         * Y False en caso contrario.
         * La cosa está en que las Razor pages tienen contidionales en donde 
         * manda a llamar estás funciones y con base a el retorno
         * habilita o apaga los botones respectivos. Neta que sí.
         */
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static Paginacion<T> Create(ICollection<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            // TODO: Pasar al constructor, esta aquí, pero por que async no funciona en constructores xD
            /*
             * Lo que hace la línea básicamente es:
             * Toma la página en la que está el usuario, calcula la cantidada
             * de registros que hay hasta esa página y los omite (Skip)
             * Después de los valores resultantes tomas los primeros[pageSize] y los guarda como lista
             */
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new Paginacion<T>(items, count, pageIndex, pageSize);
        }
    }
}
